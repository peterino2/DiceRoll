using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    public GameObject currentTarget;
    public Rigidbody rigidBody;
    public float travelTime = 1.0f;
    public Vector3 travelVector;
    public Vector3 targetLocation;

    public bool locationReached;
            
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToTargetPosition();
        }
        TickTravel();
    }

    private bool isRotating = false;
    private bool isMoving = false;

    void MoveToTargetPosition()
    {
        StartCoroutine(MoveToTargetCoro());
        StartCoroutine(RotateOnMoveCoro());
    }

    IEnumerator RotateOnMoveCoro()
    {
        if (isRotating)
            yield break;

        isRotating = true;
        var currentAnimationTime = 0.0f;
        var startingRotation = gameObject.transform.rotation;
        var euler = startingRotation.eulerAngles;
        while (currentAnimationTime < 1.0)
        {
            var newRotation = startingRotation;
            gameObject.transform.eulerAngles = new Vector3(
                euler.x - currentAnimationTime * 90f,
                euler.y,
                euler.z
            );
            
            yield return null;
            currentAnimationTime += Time.deltaTime / travelTime;
        }
        
        gameObject.transform.eulerAngles = new Vector3(
            euler.x - 90f,
            euler.y,
            euler.z
        );
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
        if (isMoving)
            yield break;
        
        isMoving = true;
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
