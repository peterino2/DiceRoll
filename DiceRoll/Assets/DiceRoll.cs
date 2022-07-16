using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    public GameObject currentTarget;
    public GridLocationMarker currentGrid;
    public Rigidbody rigidBody;
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
        
        isMoving = true;
        
        currentTarget = currentGrid.links[dir].gameObject;
        currentGrid = currentGrid.links[dir];
        
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

    //  0, 1, 2, 3, => right down left up
    IEnumerator RotateOnMoveCoro(int direction)
    {
        if (isRotating)
            yield break;

        isRotating = true;
        var currentAnimationTime = 0.0f;
        var startingRotation = innerObject.transform.localRotation;
        var e = startingRotation.eulerAngles;
        
        while (currentAnimationTime < 1.0)
        {
            var newRotation = startingRotation;
            switch (direction)
            {
                case 0:  // right
                    innerObject.transform.eulerAngles = new Vector3(e.x - currentAnimationTime * 90f, e.y, e.z);
                    break;
                
                case 1:  // down
                    innerObject.transform.eulerAngles = new Vector3(e.x, e.y, e.z + currentAnimationTime * 90f);
                    break;
                
                case 2:  // left
                    innerObject.transform.eulerAngles = new Vector3(e.x + currentAnimationTime * 90f, e.y, e.z);
                    break;
                
                case 3:  // up
                    innerObject.transform.eulerAngles = new Vector3(e.x, e.y, e.z - currentAnimationTime * 90f);
                    break;
            }
            
            yield return null;
            currentAnimationTime += Time.deltaTime / travelTime;
        }
        
            switch (direction)
            {
                case 0:  // right
                    innerObject.transform.eulerAngles = new Vector3(e.x - 90f, e.y, e.z);
                    break;
                
                case 1:  // down
                    innerObject.transform.eulerAngles = new Vector3(e.x, e.y, e.z + 90f);
                    break;
                
                case 2:  // left
                    innerObject.transform.eulerAngles = new Vector3(e.x + 90f, e.y, e.z);
                    break;
                
                case 3:  // up
                    innerObject.transform.eulerAngles = new Vector3(e.x, e.y, e.z - 90f);
                    break;
            }
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
        targetLocation = new Vector3(targetVector.x, startingVector.y, targetVector.z);
        var direction = targetLocation - startingVector;
        
        travelVector = direction.normalized * direction.magnitude / travelTime;

        yield return new WaitUntil(() => locationReached == true);
        
        travelVector = Vector3.zero;
        isMoving = false;
    }
}
