using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour, Activateable
{
    public GridLocationMarker marker;

    private bool obstacleDisabled = false;

    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        UpdateObstacle();
    }

    private void UpdateObstacle()
    {
        if (marker != null)
            marker.occupied = false;
        
        marker = GridLocationMarker.GetGridLocationMarkerAtLocation(transform.position);
        if (marker != null)
        {
            marker.occupied = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(DiceRoll player)
    {
        marker.occupied = false;
        StartCoroutine(HideAnimation());
    }

    IEnumerator HideAnimation()
    {
        if (obstacleDisabled)
            yield break;
        
        obstacleDisabled = true;

        if (clips.Length > 0)
        {
            audioSource.clip = (clips[Random.Range(0, clips.Length - 1)]);
            audioSource.Play();
        }
        
        var animationTime = 0.0f;
        while (animationTime < 1.0f)
        {
            transform.position += Vector3.down * Time.deltaTime;
            
            yield return null;
            animationTime += Time.deltaTime;
        }
    }
}
