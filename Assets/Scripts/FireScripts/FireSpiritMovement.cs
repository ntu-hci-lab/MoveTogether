using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpiritMovement : MonoBehaviour
{
    public bool walkForward = true;
    public static FireSpiritMovement instance;
    public float speed = 0.5f;
    public List<Vector3> endPoints;
    public float duration = 2f;
    private GameObject fireCircle;
    private Transform midPoint;  // Fire Circle Center
    private bool isDying = false;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // get the fire circle instance
        fireCircle = GameObject.Find("FireCircle");
        if (fireCircle == null)
        {
            Debug.LogError("FireCircle not found!");
        }

    }
    public void SetStop()
    {
        walkForward = false;
    }

    public void SetWalk()
    {
        walkForward = true;
    }

    // Update is called once per frame
    void Update()
    {
        midPoint = fireCircle.transform;

        Debug.Log("walkForward: " + walkForward);
        if(walkForward)
        {
            // Move the fire spirit forward
            transform.Translate(speed * Vector3.forward * Time.deltaTime);
        }
        // else
        // {
        //     // Stop the fire spirit
        //     GetComponent<Rigidbody>().velocity = Vector3.zero;
        // }

        if (!walkForward && OVRInput.GetDown(OVRInput.Button.Two))
        {
            // check if the fire circle is close enough to the fire spirit
            if (Vector3.Distance(transform.position, midPoint.position) < 1.5f)
            {
                StartCoroutine(JumpToTarget());
                SetWalk();
            }
        }
    }

    private IEnumerator JumpToTarget()
    {
        float timer = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = midPoint.position + Vector3.Scale(midPoint.position - startPos, new Vector3(1, -1, 1));
        Vector3 peakPos = midPoint.position;
        Debug.Log("Jump");
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            // Calculate the quadratic Bezier point
            // Vector3 a = Vector3.Lerp(startPos, peakPos, t);
            // Vector3 b = Vector3.Lerp(peakPos, endPos, t);
            // Vector3 currentPos = Vector3.Lerp(a, b, t);

            float adj = 1f;
            Vector3 midPos = (startPos + endPos) / 2;
            Vector3 adjustedPeakPos = peakPos + (peakPos - midPos) * adj;
            Vector3 currentPos = (1 - t) * (1 - t) * startPos + 2 * (1 - t) * t * adjustedPeakPos + t * t * endPos;

            transform.position = currentPos;
            
            yield return null;
        }

        // Ensure the fire spirit exactly reaches the end position at the end
        transform.position = endPos;
    }
}
