using Fusion;
using UnityEngine;

public class bubbleBehavior : NetworkBehaviour
{
    [Networked] private Vector3 NetworkedPosition { get; set; } // Sync bubble's position
    [Networked] private TickTimer NetworkedLifetime { get; set; } // Lifetime timer
    [Networked] private bool IsBroken { get; set; } // Track bubble breaking state

    public NetworkObject networkObject;

    public float floatSpeed = 1.0f;
    public float floatDrift = 0.2f;
    public float floatDuration = 5.0f;
    public float friction = 0.98f;

    private float driftTimer = 0f;
    public float driftInterval = 0.2f;
    private Vector3 driftOffset = Vector3.zero;
    private Rigidbody _rigidbody;
    public GameObject bubblePopEffect;

    private void Start()
    {   

        networkObject = GetComponent<NetworkObject>();
        _rigidbody = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.drag = 0;

        if (Object.HasStateAuthority)
        {
            NetworkedLifetime = TickTimer.CreateFromSeconds(Runner, floatDuration);
        }
    }

    private void Update()
    {
        if (IsBroken) return;

        driftTimer += Time.deltaTime;

        if (Object.HasStateAuthority)
        {
            if (driftTimer >= driftInterval)
            {
                // Reset timer and calculate a new drift offset
                driftTimer = 0f;
                driftOffset = new Vector3(
                    Random.Range(-floatDrift * 0.5f, floatDrift * 0.5f), // Halve the range
                    floatSpeed * Time.deltaTime,
                    Random.Range(-floatDrift * 0.5f, floatDrift * 0.5f)
                );
            }

            // Add the drift offset to the position
            Vector3 newPosition = transform.position + driftOffset;
            NetworkedPosition = newPosition;

            if (NetworkedLifetime.Expired(Runner))
            {
                BreakBubble();
            }
        }

        // Update position and apply friction
        transform.position = NetworkedPosition;
        _rigidbody.velocity *= friction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority || IsBroken) return;

        if (other.CompareTag("spirit"))
        {
            var spirit = other.GetComponent<SpiritBehavior>();
            if (spirit != null)
            {
                spirit.CaptureSpirit(this);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {   
        Debug.Log("Bubble Collided with " + collision.gameObject.name);
        
        if (collision.gameObject.name == "FLOOR_EffectMesh")
        {
            BreakBubble();
        }
    }
    // public void BreakBubble()
    // {
    //     if (!Object.HasStateAuthority) return;

    //     IsBroken = true;

    //     if (bubblePopEffect != null)
    //     {
    //         Runner.Spawn(bubblePopEffect, transform.position, Quaternion.identity);
    //     }

    //     foreach (var spirit in GetComponentsInChildren<SpiritBehavior>())
    //     {
    //         spirit.ReleaseSpirit();
    //     }

    //     Runner.Despawn(Object);
    // }
    public void BreakBubble()
    {
        if (!Object.HasStateAuthority) return;

        IsBroken = true;

        if (bubblePopEffect != null)
        {
            Runner.Spawn(bubblePopEffect, transform.position, Quaternion.identity);
        }

        // Find and release all spirits captured by this bubble
        foreach (var spirit in FindObjectsOfType<SpiritBehavior>())
        {
            if (spirit.IsCaptured && spirit.ParentBubbleId == Object.Id)
            {
                spirit.ReleaseSpirit();
            }
        }

        Runner.Despawn(Object); // Despawn the bubble
    }
}



// using System.Collections.Generic;
// using Fusion;
// using UnityEngine;

// public class bubbleBehavior : NetworkBehaviour
// {
//     [Networked] private Vector3 NetworkedPosition { get; set; } // Sync bubble's position
//     [Networked] private TickTimer NetworkedLifetime { get; set; } // Lifetime timer
//     [Networked] private bool IsBroken { get; set; } // Track bubble breaking state

//     public float floatSpeed = 1.0f;
//     public float floatDrift = 0.2f;
//     public float floatDuration = 5.0f;
//     public float friction = 0.98f;

//     private Rigidbody _rigidbody;
//     public GameObject bubblePopEffect;
//     private List<GameObject> capturedObjects = new List<GameObject>();

//     private void Start()
//     {
//         _rigidbody = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();

//         // Set Rigidbody properties
//         _rigidbody.useGravity = false;
//         _rigidbody.drag = 0;

//         if (Object.HasStateAuthority)
//         {
//             NetworkedLifetime = TickTimer.CreateFromSeconds(Runner, floatDuration);
//         }
//     }

//     private void Update()
//     {
//         if (IsBroken) return;

//         if (Object.HasStateAuthority)
//         {
//             // Update bubble position and sync it
//             Vector3 newPosition = transform.position + new Vector3(
//                 Random.Range(-floatDrift, floatDrift),
//                 floatSpeed * Time.deltaTime,
//                 Random.Range(-floatDrift, floatDrift)
//             );
//             NetworkedPosition = newPosition;

//             // Destroy the bubble after its lifetime expires
//             if (NetworkedLifetime.Expired(Runner))
//             {
//                 BreakBubble();
//             }
//         }

//         // Sync position for all players
//         transform.position = NetworkedPosition;

//         foreach (GameObject obj in capturedObjects)
//         {
//             if (obj != null)
//             {
//                 obj.transform.position = transform.position;
//             }
//         }

//         // Apply friction to simulate drag
//         _rigidbody.velocity *= friction;
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         if (!Object.HasStateAuthority || IsBroken) return;

//         // Check if the object can be captured
//         if (other.CompareTag("spirit"))
//         {
//             CaptureObject(other.gameObject);
//         }
//     }

//     private void CaptureObject(GameObject obj)
//     {
//         if (!capturedObjects.Contains(obj))
//         {
//             Rigidbody rb = obj.GetComponent<Rigidbody>();
//             if (rb != null)
//             {
//                 rb.useGravity = false;
//                 rb.velocity = Vector3.zero;
//             }

//             obj.transform.position = transform.position;
//             obj.transform.SetParent(transform);
//             capturedObjects.Add(obj);
//         }
//     }

// public void BreakBubble()
// {
//     if (!Object.HasStateAuthority) return;

//     IsBroken = true;

//     // Optional: Trigger bubble pop effect
//     if (bubblePopEffect != null)
//     {
//         Runner.Spawn(bubblePopEffect, transform.position, Quaternion.identity);
//     }

//     // Release all captured objects
//     foreach (GameObject obj in capturedObjects)
//     {
//         if (obj != null)
//         {
//             // Release spirit if it has SpiritBehavior
//             SpiritBehavior spirit = obj.GetComponent<SpiritBehavior>();
//             if (spirit != null)
//             {
//                 spirit.ReleaseSpirit();
//             }
//             else
//             {
//                 // Detach other objects from the bubble
//                 obj.transform.SetParent(null);
//                 Rigidbody rb = obj.GetComponent<Rigidbody>();
//                 if (rb != null)
//                 {
//                     rb.useGravity = true;
//                 }
//             }
//         }
//     }

//     Runner.Despawn(Object); // Despawn the bubble for all clients
// }

// }











//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class bubbleBehavior : MonoBehaviour
//{
//    public float floatSpeed = 1.0f; // Speed at which the bubble floats upward
//    public float floatDuration = 5.0f; // Duration for which the bubble will float before breaking
//    public float floatDrift = 0.2f; // Random drift factor for horizontal movement
//    public float friction = 0.98f;
//    private Rigidbody _rigidbody;
//    public GameObject bubblePopEffect; // Optional: Particle effect or animation when the bubble breaks

//    private List<GameObject> capturedObjects = new List<GameObject>(); // List of objects inside the bubble
//    private float elapsedTime = 0.0f; // Time since the bubble was spawned

//    void Start()
//    {
//        // Add or get the Rigidbody component
//        _rigidbody = GetComponent<Rigidbody>();

//        if (_rigidbody == null)
//        {
//            _rigidbody = gameObject.AddComponent<Rigidbody>();
//        }

//        // Set initial Rigidbody properties
//        _rigidbody.useGravity = false;  // Disable gravity for floating
//        _rigidbody.drag = 0;  // Disable built-in drag; we'll handle it manually
//        _rigidbody.angularDrag = 0;
//    }
//    void Update()
//    {
//        // Make the bubble float upward
//        transform.position += new Vector3(
//            Random.Range(-floatDrift, floatDrift), // Random horizontal drift
//            floatSpeed * Time.deltaTime,          // Upward movement
//            Random.Range(-floatDrift, floatDrift) // Random horizontal drift
//        );

//        foreach (GameObject obj in capturedObjects)
//        {
//            if (obj != null)
//            {
//                obj.transform.position = transform.position; // Match bubble's position
//            }
//        }


//        _rigidbody.velocity *= friction;
//        // Update the elapsed time
//        elapsedTime += Time.deltaTime;

//        // Destroy the bubble after it has floated for the specified duration
//        if (elapsedTime >= floatDuration)
//        {
//            BreakBubble();
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        // Check if the object is eligible to be captured
//        if (other.CompareTag("spirit")) // Use a specific tag for objects that can be captured
//        {
//            Rigidbody rb = other.GetComponent<Rigidbody>();

//            if (rb != null)
//            {
//                // Disable gravity on the object's Rigidbody
//                rb.useGravity = false;
//                rb.velocity = Vector3.zero; // Stop any existing movement
//            }

//            // Move the object to the center of the bubble
//            other.transform.position = transform.position;

//            // Parent the object to the bubble to make it follow the bubble's movement
//            other.transform.SetParent(transform);

//            // Add the object to the list of captured objects
//            capturedObjects.Add(other.gameObject);
//        }
//    }

//    public void BreakBubble()
//    {
//        // Handle breaking the bubble
//        Debug.Log("Bubble breaking!");

//        // Optional: Play bubble pop effect
//        if (bubblePopEffect != null)
//        {
//            Instantiate(bubblePopEffect, transform.position, Quaternion.identity);
//        }

//        // Detach all captured objects
//        foreach (GameObject obj in capturedObjects)
//        {
//            if (obj != null)
//            {
//                // Detach the object from the bubble
//                obj.transform.SetParent(null);

//                // Re-enable gravity if the object has a Rigidbody
//                Rigidbody rb = obj.GetComponent<Rigidbody>();
//                if (rb != null)
//                {
//                    rb.useGravity = true;
//                }
//            }
//        }

//        // Destroy the bubble
//        Destroy(gameObject);
//    }
//}
