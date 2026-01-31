using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeEntranceWater: MonoBehaviour
{
    public Transform tubeStart; // Starting point of the tube
    public Transform tubeMid;   // Endpoint of the tube
    public Transform tubeEnd;
    public float suctionSpeed = 5f; // Speed at which the object moves through the tube
    private bool isMoving = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the prefab
        if (other.CompareTag("spirit")) // Ensure your prefab has the "Suckable" tag
        {


            bubbleBehavior bubble = other.transform.GetComponentInParent<bubbleBehavior>();

            if (bubble != null)
            {
                // Call the BreakBubble method on the parent bubble
                bubble.BreakBubble();
                Debug.Log("BreakBubble method called on the bubble because of a spirit collision!");
            }

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Disable Rigidbody to control movement manually
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }

            // Start moving the object through the tube
            StartCoroutine(MoveThroughTube(other.transform));
        }

        //else if (other.CompareTag("Bubble")) // Ensure bubbles have the "bubble" tag
        //{
        //    // Get the bubbleBehavior component from the bubble object
        //    bubbleBehavior bubble = other.GetComponent<bubbleBehavior>();

        //    if (bubble != null)
        //    {
        //        // Call the BreakBubble method
        //        bubble.BreakBubble();
        //        Debug.Log("BreakBubble method called on the bubble!");
        //    }
        //    else
        //    {
        //        Debug.LogWarning("bubbleBehavior component not found on the bubble!");
        //    }
        //}
    }

    private IEnumerator MoveThroughTube(Transform objectToMove)
    {
        isMoving = true;

        // Move towards the start of the tube
        while (Vector3.Distance(objectToMove.position, tubeStart.position) > 0.1f)
        {
            objectToMove.position = Vector3.MoveTowards(objectToMove.position, tubeStart.position, suctionSpeed * Time.deltaTime);
            yield return null;
        }

        // Move towards the start of the tube
        while (Vector3.Distance(objectToMove.position, tubeMid.position) > 0.1f)
        {
            objectToMove.position = Vector3.MoveTowards(objectToMove.position, tubeMid.position, suctionSpeed * Time.deltaTime);
            yield return null;
        }

        // Move through the tube to the endpoint
        while (Vector3.Distance(objectToMove.position, tubeEnd.position) > 0.1f)
        {
            objectToMove.position = Vector3.MoveTowards(objectToMove.position, tubeEnd.position, suctionSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;

        // Reactivate Rigidbody if needed
        Rigidbody rb = objectToMove.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}

