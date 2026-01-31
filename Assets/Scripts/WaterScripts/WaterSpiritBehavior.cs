using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Meta.XR.MultiplayerBlocks.Fusion;

public class SpiritBehavior : NetworkBehaviour
{
    [Networked] public NetworkBool IsCaptured { get; set; } // Track capture state
    [Networked] public NetworkId ParentBubbleId { get; set; } // ID of the bubble capturing the spirit

    public NetworkObject networkObject;
    private Rigidbody _rigidbody;
    private bool isDying = false;
    private Transform _originalParent; // Store original parent for reset purposes

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
        _originalParent = transform.parent;

        // Ensure the Rigidbody starts with gravity enabled
        _rigidbody.useGravity = true;
        networkObject = GetComponent<NetworkObject>();
    }

    private void Update()
    {
        if (IsCaptured)
        {
            // Find the bubble with the matching NetworkId
            var parentBubble = FindBubbleById(ParentBubbleId);
            if (parentBubble != null)
            {
                // Align the spirit's position with the bubble's position
                transform.position = parentBubble.transform.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority || IsCaptured) return;

        if (other.CompareTag("Bubble"))
        {
            var bubble = other.GetComponent<bubbleBehavior>();
            if (bubble != null)
            {
                CaptureSpirit(bubble);
            }
        }
    }

        private void OnCollisionEnter(Collision collision)
    {   
        Debug.Log("Water Collided with " + collision.gameObject.name);
        
        if (collision.gameObject.name == "FLOOR_EffectMesh" && !isDying)
        {
            Debug.Log("Water hit the floor. Starting death sequence.");
            isDying = true;
            // Set the animation bool to trigger the dying animation
            Animator WaterAnimator = GetComponent<Animator>();
            WaterAnimator.SetBool("Die", true);

            // Start a coroutine to despawn after 5 seconds
            StartCoroutine(DespawnAfterDelay(5f));
        }
    }
    private IEnumerator DespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Earth Despawned");
        WaterObjectSpawner.instance.DespawnObject(networkObject);
    }


    public void CaptureSpirit(bubbleBehavior bubble)
    {
        if (IsCaptured) return;

        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;

        IsCaptured = true;
        ParentBubbleId = bubble.Object.Id;

        Debug.Log($"Spirit captured by bubble: {bubble.name}");
    }

    public void ReleaseSpirit()
    {
        if (!IsCaptured) return;

        IsCaptured = false;
        // Reset parent bubble reference
        // ParentBubbleId =  NetworkId.Invalid;
        _rigidbody.useGravity = true;

        Debug.Log("Spirit released from bubble.");
    }

    private bubbleBehavior FindBubbleById(NetworkId bubbleId)
    {
        foreach (var bubble in FindObjectsOfType<bubbleBehavior>())
        {
            if (bubble.Object.Id == bubbleId)
            {
                return bubble;
            }
        }
        return null;
    }
}



// using Fusion;
// using UnityEngine;

// public class SpiritBehavior : NetworkBehaviour
// {
//     [Networked] private NetworkBool IsCaptured { get; set; } // Track capture state
//     [Networked] private Vector3 NetworkedPosition { get; set; } // Sync position when released
//     [Networked] private NetworkId ParentBubbleId { get; set; } // ID of the bubble capturing the spirit

//     private Rigidbody _rigidbody;
//     private Transform _originalParent; // Store original parent for reset purposes

//     private void Start()
//     {
//         _rigidbody = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
//         _originalParent = transform.parent;

//         // Ensure the Rigidbody starts with gravity enabled
//         _rigidbody.useGravity = true;
//     }

//     private void Update()
//     {
//         // If the spirit is captured, sync its position with the bubble
//         if (IsCaptured)
//         {
//             // Find the bubble with the matching NetworkId
//             var parentBubble = FindBubbleById(ParentBubbleId);
//             if (parentBubble != null)
//             {
//                 transform.position = parentBubble.transform.position;

//                 // Optionally, set it as a child of the bubble (for visual attachment)
//                 if (transform.parent != parentBubble.transform)
//                 {
//                     transform.SetParent(parentBubble.transform);
//                 }
//             }
//         }
//         else
//         {
//             // Sync the position for non-authority clients when not captured
//             if (!Object.HasStateAuthority)
//             {
//                 transform.position = NetworkedPosition;
//             }
//         }
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         // Only the state authority handles capture logic
//         if (!Object.HasStateAuthority) return;

//         // Check if the spirit is entering a bubble
//         if (other.CompareTag("Bubble"))
//         {
//             var bubbleBehavior = other.GetComponent<bubbleBehavior>();
//             if (bubbleBehavior != null)
//             {
//                 CaptureSpirit(bubbleBehavior);
//             }
//         }
//     }

//     public void CaptureSpirit(bubbleBehavior bubble)
//     {
//         if (!IsCaptured)
//         {
//             // Disable gravity and set the parent bubble
//             _rigidbody.useGravity = false;
//             _rigidbody.velocity = Vector3.zero;

//             IsCaptured = true;
//             ParentBubbleId = bubble.Object.Id;

//             // Notify all clients about the capture
//             Debug.Log($"Spirit captured by bubble: {bubble.name}");
//         }
//     }

//     public void ReleaseSpirit()
//     {
//         if (IsCaptured)
//         {
//             // Enable gravity and clear parent bubble
//             _rigidbody.useGravity = true;

//             IsCaptured = false;
//             // ParentBubbleId = NetworkId.Invalid;

//             // Reset parent and sync position
//             transform.SetParent(_originalParent);
//             NetworkedPosition = transform.position;

//             Debug.Log("Spirit released from bubble.");
//         }
//     }

//     private bubbleBehavior FindBubbleById(NetworkId bubbleId)
//     {
//         foreach (var bubble in FindObjectsOfType<bubbleBehavior>())
//         {
//             if (bubble.Object != null && bubble.Object.Id == bubbleId)
//             {
//                 return bubble;
//             }
//         }
//         return null;
//     }
// }
