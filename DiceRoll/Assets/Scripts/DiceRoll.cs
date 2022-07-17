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
    public AnimationCurve breakImpulseCurve;
    public GameObject innerObject;
    public float bobHeight;
    public bool locationReached;
    public bool bCanDash;
    public List<string> Abilities;

    public bool bIsAimingAbility = false;
            
    
    int currentFace = 1;
    
    private int dir = 0;
    private bool bMovePressed = false;
    // Start is called before the first frame update
    void Start()
    {
        currentGrid = GridLocationMarker.GetGridLocationMarkerAtLocation(transform.position);
        transform.position = currentGrid.transform.position + (Vector3.up * 0.4f);
        currentGrid.occupied = true;
        currentFace = checkRotationValue();
    }

    // Update is called once per frame
    void Update()
    {
        bMovePressed = false;
        dir = 0;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Abilities.Contains("dash") && currentFace  == 2)
                bIsAimingAbility = true;
            
            if(Abilities.Contains("break") && currentFace  == 3)
                bIsAimingAbility = true;
        }
        
        TickAbilityAim();
        
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

    void TickAbilityAim()
    {
        if (!bIsAimingAbility)
            return;

        if (bMovePressed)
        {
            bIsAimingAbility = false;
            bMovePressed = false;
            if (currentFace == 2)
            {
                DashOver(dir);
            }
            
            if (currentFace == 3)
            {
                BreakDown(dir);
            }
        }
    }

    public void BreakDown(int _dir)
    {
        if (!Abilities.Contains("break"))
            return;
        
        if (isMoving)
            return;

        if (currentGrid.links[_dir] == null)
            return;

        if (currentGrid.links[_dir].activateable == null)
            return;

        currentGrid.links[_dir].activateable.Activate(this);
        
        StartCoroutine(BreakDownCoro(_dir));
    }

    private Vector3[] checkOffset =
    {
        new Vector3(0, 0, -1), // right
        new Vector3(-1, 0, 0), // down
        new Vector3(0, 0, 1), // left
        new Vector3(1, 0, 0), // up
    };
    
    public float breakTime = 0.3f;
    
    IEnumerator BreakDownCoro(int _dir)
    {
        float currentAnimationTime = 0;
        var startPosition = innerObject.transform.position;
        while (currentAnimationTime < 1.0f)
        {
            float breakAnimFloat = breakImpulseCurve.Evaluate(currentAnimationTime);
            innerObject.transform.position = startPosition + (checkOffset[_dir] * breakAnimFloat);
            
            yield return null;
            currentAnimationTime += Time.deltaTime / breakTime;
        }
    }
    
    
    public bool isRotating = false;
    public bool isMoving = false;
    public bool isBobbing = false;
    

    void MoveToTargetPosition(int moveDir)
    {
        if (isMoving)
            return;

        if (currentGrid.links[moveDir] == null)
            return;
        
        if (currentGrid.links[moveDir].occupied)
            return;

        if (currentGrid != null)
            currentGrid.occupied = false;
            
        isMoving = true;
        
        PrepareMove(currentGrid.links[moveDir]);

        StartCoroutine(MoveToTargetCoro());
        StartCoroutine(RotateOnMoveCoro(moveDir));
        StartCoroutine(BobOnMoveCoro());
    }

    public void PrepareMove(GridLocationMarker marker)
    {
        currentTarget = marker.gameObject;
        currentGrid = marker;
        currentGrid.occupied = true;
    }

    IEnumerator BobOnMoveCoro()
    {
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

    void DashOver(int dir)
    {
        if (isMoving)
            return;

        if (currentGrid.links[dir] == null)
            return;

        if (currentGrid.links[dir].links[dir] == null)
            return;
        
        if (currentGrid.links[dir].links[dir].occupied)
            return;

        if (currentGrid != null)
            currentGrid.occupied = false;
        
        PrepareMove(currentGrid.links[dir].links[dir]);
        
        StartCoroutine(MoveToTargetCoro());
        StartCoroutine(RotateOnMoveCoro(dir));
        StartCoroutine(BobOnMoveCoro());
    }

    //  0, 1, 2, 3, => right down left up
    IEnumerator RotateOnMoveCoro(int direction)
    {
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
        currentFace = checkRotationValue();
        
        Debug.Log("current side up: " + currentFace);
        
        travelVector = Vector3.zero;
        isMoving = false;
    }
}
