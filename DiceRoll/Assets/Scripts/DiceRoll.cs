using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DiceRoll : MonoBehaviour
{
    public GameObject currentTarget;
    public GridLocationMarker currentGrid;
    public float travelTime = 1.0f;
    public Vector3 travelVector;
    public Vector3 targetLocation;
    public AnimationCurve upDownCurve;
    public GameObject innerObject;
    public float bobHeight;
    public bool locationReached;
            
    // Start is called before the first frame update
    void Start()
    {
        currentGrid = GridLocationMarker.GetGridLocationMarkerAtLocation(transform.position);
        transform.position = currentGrid.transform.position + (Vector3.up * 0.4f);
        currentGrid.occupied = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool bMovePressed = false;
        int dir = 0;
        if (Input.GetKeyDown(KeyCode.D))
        {
            bMovePressed = true;
            dir = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            bMovePressed = true;
            dir = 1;
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            bMovePressed = true;
            dir = 2;
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            bMovePressed = true;
            dir = 3;
        }

        if (bMovePressed)
        {
            if (currentGrid != null)
            {
                MoveToTargetPosition(dir);
            }
            else
            {
                Debug.Log("Can't move, bad currentGrid");
            }
            
        }
        
        TickTravel();
    }

    private bool isRotating = false;
    private bool isMoving = false;
    private bool isBobbing = false;
    

    void MoveToTargetPosition(int dir)
    {
        if (isMoving)
            return;

        if (currentGrid.links[dir] == null)
            return;
        
        if (currentGrid.links[dir].occupied)
            return;

        if (currentGrid != null)
            currentGrid.occupied = false;
            
        isMoving = true;
        
        currentTarget = currentGrid.links[dir].gameObject;
        currentGrid = currentGrid.links[dir];
        currentGrid.occupied = true;

        StartCoroutine(MoveToTargetCoro());
        StartCoroutine(RotateOnMoveCoro(dir));
        StartCoroutine(BobOnMoveCoro());
    }

    IEnumerator BobOnMoveCoro()
    {
        if (isBobbing)
            yield break;

        isBobbing = true;
        var currentAnimationTime = 0.0f;
        var staringPosition = innerObject.transform.localPosition;
        while (currentAnimationTime < 1.0)
        {
            var upDownPosition = upDownCurve.Evaluate(currentAnimationTime) * bobHeight;
            innerObject.transform.localPosition = staringPosition + new Vector3(0, upDownPosition, 0);
            yield return null;
            currentAnimationTime += Time.deltaTime / travelTime;
        }

        isBobbing = false;
        innerObject.transform.localPosition = staringPosition;
    }
    
    private Vector3[] checkVectors =
    {
        Vector3.up, // 
        Vector3.forward, // 
        -Vector3.right, // 
        Vector3.right, // 
        -Vector3.forward, // 
        Vector3.down, // 
    };

    public int checkRotationValue()
    {
        var currentRotation = innerObject.transform.rotation;
        for (int i = 0; i < checkVectors.Length; i += 1)
        {
            var rotatedVector = currentRotation * checkVectors[i];
            if (Vector3.Dot(rotatedVector, Vector3.up) > 0.70f)
            {
                return i + 1;
            }
        }

        return -1; // this should never happen
    }
    
    private Vector3[] rotationVectors =
    {
        new Vector3(-1, 0, 0), // right
        new Vector3(0, 0, 1), // down
        new Vector3(1, 0, 0), // left
        new Vector3(0, 0, -1), // up
    };

    //  0, 1, 2, 3, => right down left up
    IEnumerator RotateOnMoveCoro(int direction)
    {
        if (isRotating)
            yield break;

        isRotating = true;
        var currentAnimationTime = 0.0f;
        var startingRotation = innerObject.transform.rotation;
        var e = startingRotation.eulerAngles;

        var rotateVector = rotationVectors[direction];
        
        while (currentAnimationTime < 1.0)
        {
            var newRotation = Quaternion.Euler(rotateVector * (currentAnimationTime * 90.0f)) * startingRotation;
            innerObject.transform.rotation = newRotation;
            yield return null;
            currentAnimationTime += Time.deltaTime / travelTime;
        }
        
        var x = Quaternion.Euler(rotateVector * ( 90.0f)) *startingRotation;
        innerObject.transform.rotation = x;
        
        isRotating = false;
    }

    void TickTravel()
    {
        var currentPosition = gameObject.transform.position;
        var distanceLeft = Vector3.Distance(currentPosition, targetLocation);
        var newPosition = currentPosition + travelVector * Time.deltaTime;
        if(Vector3.Distance(currentPosition, newPosition) > distanceLeft || distanceLeft < 0.01 )
        {
            locationReached = true;
            travelVector = Vector3.zero;
            gameObject.transform.position = targetLocation;
        }
        else
        {
            gameObject.transform.position += travelVector * Time.deltaTime;
        }
    }

    IEnumerator MoveToTargetCoro()
    {
        locationReached = false;
        
        var targetVector = currentTarget.transform.position;
        var startingVector = gameObject.transform.position;
        targetLocation = new Vector3(targetVector.x, targetVector.y + 0.4f, targetVector.z);
        var direction = targetLocation - startingVector;
        
        travelVector = direction.normalized * direction.magnitude / travelTime;

        yield return new WaitUntil(() => locationReached == true);
        
        currentGrid.OnSteppedOn(this);
        
        Debug.Log("current side up: " + checkRotationValue());
        
        travelVector = Vector3.zero;
        isMoving = false;
    }
}
