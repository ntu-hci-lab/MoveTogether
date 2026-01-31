// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Fusion;
// using Meta.XR.MultiplayerBlocks.Fusion;
// using UnityEngine.SceneManagement;
// using Meta.XR.InputActions;

// using System.Data.Common;
// using System.Drawing;
// using UnityEngine.UI;

// public class WaterObjectSpawner : MonoBehaviour
// {
//     public Transform leftHandTransform;
//     public Transform rightHandTransform;
//     public static WaterObjectSpawner instance;
//     public GameObject prefab;
//     public GameObject bubble;
//     bool isBubblegunSpawn = false;
//     public GameObject Bubblegun;
//     public GameObject Blower;
//     private NetworkRunner _networkRunner;
//     private bool _sceneLoaded;
//     private bool _secondsceneLoaded;
//     public Vector3 bubbleSize = Vector3.one;

//     private bool isBlowerSpawned = false;
//     public float maxBlowForce = 5.0f; // Maximum force applied to the bubble
//     public float blowRange = 2.0f;    // Maximum distance for full effect
//     int level;

//     // private int currentSpawnCount = 0; // Current number of spawned prefabs


//     [Networked]
//     private float timer { get; set; } // Timer to track spawn intervals


//     // = 100; // Maximum number of spawned prefabs
//     private void OnEnable()
//     {
//         FusionBBEvents.OnSceneLoadDone += OnLoaded;
//     }

//     private void OnDisable()
//     {
//         FusionBBEvents.OnSceneLoadDone -= OnLoaded;
//     }

//     private void OnLoaded(NetworkRunner networkRunner)
//     {
//         _sceneLoaded = true;
//         _networkRunner = networkRunner;

//     }
//     void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//         }

//     }
//     // Start is called before the first frame update
//     void Start()
//     {

//     }
//     // public void SetStart(){
//     //     start = true;
//     // }
//     public void DespawnObject(NetworkObject no)
//     {
//         if (_networkRunner != null)
//         {
//             _networkRunner.Despawn(no);
//         }
//     }
//     // Update is called once per frame
//     void Update()
//     {
//         level = ChangeLevel.instance.GetLevel();
//         if (level == 2) {
//             if (isBubblegunSpawn && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
//             {
//                 Bubblegun.transform.position = rightHandTransform.position;
//                 Bubblegun.transform.rotation = rightHandTransform.rotation;
//             }

//             // if (isBlowerSpawned && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
//             // {
//             //     Blower.transform.position = rightHandTransform.position;
//             //     Blower.transform.rotation = rightHandTransform.rotation;
//             // }


//             if (OVRInput.GetDown(OVRInput.Button.Three))
//             {
//                 // GameObject spawnedBall = Instantiate(prefab, transform.position, transform.rotation);
//                 _networkRunner.Spawn(
//                     prefab, 
//                     Vector3.zero,
//                     Quaternion.identity,
//                     inputAuthority: null,
//                     (runner, obj) => // onBeforeSpawned
//                     {
//                         obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
//                         obj.transform.rotation = leftHandTransform.rotation;
//                         obj.transform.position = leftHandTransform.position;

//                     }
//                 );

//             }

//             if (_networkRunner.LocalPlayer.PlayerId == GetLowestPlayerId())
//             {
//                 if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
//                 {
                
//                     if (!isBubblegunSpawn)
//                     {
//                         _networkRunner.Spawn(
//                             Bubblegun,
//                             Vector3.zero,
//                             Quaternion.identity,
//                             _networkRunner.LocalPlayer,
//                             (runner, obj) => // onBeforeSpawned
//                             {
//                                 obj.transform.rotation = rightHandTransform.rotation;
//                                 obj.transform.position = rightHandTransform.position;
//                             }
//                         );
//                         isBubblegunSpawn = true;
//                     }
            
//                     // Spawn a bubble prefab
//                     else if(IsBubblegunGrabbed())
//                     {

//                         Vector3 spawnPosition = Bubblegun.transform.position + Bubblegun.transform.forward * 0.7f; // Adjust the distance as needed
//                         _networkRunner.Spawn(
//                         bubble,
//                         spawnPosition,
//                         Bubblegun.transform.rotation,
//                         inputAuthority: null,
//                         (runner, obj) => // onBeforeSpawned
//                         {
//                             obj.transform.localScale = bubbleSize; // Adjust size as needed
//                             // release state authority
//                             //obj.GetComponent<NetworkObject>().ReleaseStateAuthority();
//                         }
//                         );
//                     }


//                 }
//             }
        

//             // if (_networkRunner.LocalPlayer.PlayerId == GetLowestPlayerId())
//             // {

//             //     // Spawn the blower when pressing the secondary trigger
//             //     if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
//             //     {
//             //         if (!isBlowerSpawned)
//             //         {
//             //             _networkRunner.Spawn(
//             //                 Blower,
//             //                 Vector3.zero,
//             //                 Quaternion.identity,
//             //                 _networkRunner.LocalPlayer,
//             //                 (runner, obj) => // onBeforeSpawned
//             //                 {
//             //                     obj.transform.rotation = rightHandTransform.rotation;
//             //                     obj.transform.position = rightHandTransform.position;
//             //                 }
//             //             );
//             //             isBlowerSpawned = true;
//             //         }
//             //     }

//             //     // Interact with the bubble when pressing the primary trigger while holding the blower
//             //     if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && IsBlowerGrabbed())
//             //     {
//             //         // Vector3 spawnPosition = Bubblegun.transform.position + Bubblegun.transform.forward * 0.7f; // Adjust the distance as needed
//             //         //_networkRunner.Spawn(
//             //         //bubble,
//             //         //spawnPosition,
//             //         //Bubblegun.transform.rotation,
//             //         //inputAuthority: null,
//             //         //(runner, obj) => // onBeforeSpawned
//             //         //{
//             //         //    obj.transform.localScale = bubbleSize; // Adjust size as needed
//             //         //}
//             //         //);
//             //         BlowBubble();
//             //     }
                
//             // }



//                 if (OVRInput.GetDown(OVRInput.Button.Two))
//                 {
//                     ResetScene();
//                 }
//         }
//     }

//     private bool IsBubblegunGrabbed()
//     {
//         // Add logic to determine if the Bubblegun is being held by the player.
//         // This could involve checking distance from the hand to the Bubblegun, or a grab state if you're using an XR interaction toolkit.
//         //return OVRInput.Get(OVRInput.Button.PrimaryHandTrigger); // Adjust button as needed
//         return (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)) && (Vector3.Distance(rightHandTransform.position, Bubblegun.transform.position) < 0.4f); // Example threshold
//         //return true;
//     }

//     private bool IsBlowerGrabbed()
//     {
//         // Add logic to determine if the Bubblegun is being held by the player.
//         // This could involve checking distance from the hand to the Bubblegun, or a grab state if you're using an XR interaction toolkit.
//         //return OVRInput.Get(OVRInput.Button.PrimaryHandTrigger); // Adjust button as needed
//         return (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)) && (Vector3.Distance(rightHandTransform.position, Blower.transform.position) < 0.4f); // Example threshold
//                                                                                                                                                                 //return true;
//     }

//     private void BlowBubble()
//     {
//         // Find the closest bubble to blow
//         GameObject[] bubbles = GameObject.FindGameObjectsWithTag("Bubble");
//         foreach (var bubble in bubbles)
//         {
//             float distance = Vector3.Distance(Blower.transform.position, bubble.transform.position);
//             if (distance <= blowRange)
//             {
//                 //get state authority
//                 NetworkObject bubbleNetworkObject = bubble.GetComponent<NetworkObject>();
//                 bubbleNetworkObject.RequestStateAuthority();
//                 // Calculate the blow direction and force
//                 Vector3 blowDirection = (bubble.transform.position - Blower.transform.position).normalized;
//                 float force = Mathf.Lerp(maxBlowForce, 0, distance / blowRange);

//                 // Apply force to the bubble's Rigidbody
//                 Rigidbody bubbleRb = bubble.GetComponent<Rigidbody>();
//                 if (bubbleRb != null)
//                 {
//                     bubbleRb.AddForce(blowDirection * force, ForceMode.Impulse);
//                     Debug.Log($"Bubble blown with force {force} at distance {distance}");
//                 }
//             }
//         }
//     }

//     // private void BlowBubble()
//     // {
//     //     // Find all bubbles in the scene
//     //     GameObject[] bubbles = GameObject.FindGameObjectsWithTag("Bubble");
//     //     foreach (var bubble in bubbles)
//     //     {
//     //         float distance = Vector3.Distance(Blower.transform.position, bubble.transform.position);
//     //         if (distance <= blowRange)
//     //         {
//     //             // Calculate the blow direction and force
//     //             Vector3 blowDirection = (bubble.transform.position - Blower.transform.position).normalized;
//     //             float force = Mathf.Lerp(maxBlowForce, 0, distance / blowRange);

//     //             // Use an RPC to notify the state authority to apply the force
//     //             bubbleBehavior bubbleComponent = bubble.GetComponent<bubbleBehavior>();
//     //             if (bubbleComponent != null)
//     //             {
//     //                 bubbleComponent.RPC_BlowBubble(blowDirection, force);
//     //             }
//     //             else
//     //             {
//     //                 Debug.LogWarning("BubbleBehavior component not found on bubble object.");
//     //             }
//     //         }
//     //     }
//     // }



//     [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
//     public void RPC_BlowBubble(Vector3 blowPosition, Vector3 blowDirection, float force)
//     {
//         Rigidbody bubbleRb = GetComponent<Rigidbody>();
//         if (bubbleRb != null)
//         {
//             bubbleRb.AddForce(blowDirection * force, ForceMode.Impulse);
//             Debug.Log($"Bubble blown with force {force} at position {blowPosition}");
//         }
//     }


//     private void ResetScene()
//     {
//         if (_networkRunner != null && _networkRunner.IsSceneAuthority)
//         {
//             // Find all network objects in the scene
//             var networkObjects = FindObjectsOfType<NetworkObject>();

//             foreach (var networkObject in networkObjects)
//             {
//                 // Check if this object should be reset or despawned
//                 if (ShouldReset(networkObject))
//                 {
//                     // Reset object instead of destroying
//                     ResetNetworkObject(networkObject);
//                 }
//                 else if (networkObject.HasStateAuthority)
//                 {
//                     // Despawn the object
//                     _networkRunner.Despawn(networkObject);
//                 }
//             }


//             // Reset spawn count and timers
//             timer = 0f;
//             isBubblegunSpawn = false;
//             isBlowerSpawned = false;

//             Debug.Log("Scene reset completed.");
//         }
//         else
//         {
//             Debug.LogWarning("ResetScene called, but this instance is not the scene authority.");
//         }
//     }

//     private bool ShouldReset(NetworkObject networkObject)
//     {
//         // Example: Reset only objects with a specific tag
//         return networkObject.CompareTag("Resettable");

//         // Example: Or check for a specific component
//         // return networkObject.GetComponent<MyResettableComponent>() != null;

//         // Example: Or use a list of object IDs to reset
//         // return resettableObjectIds.Contains(networkObject.NetworkObjectId);
//     }


//     private void ResetNetworkObject(NetworkObject networkObject)
//     {
//         Debug.Log($"Resetting NetworkObject: {networkObject.name}");

//         // Example: Reset position and rotation
//         networkObject.transform.position = Vector3.zero;
//         networkObject.transform.rotation = Quaternion.identity;

//         // Example: Reset custom component states
//         var myComponent = networkObject.GetComponent<WaterSpiritSpawner>();
//         if (myComponent != null)
//         {
//             myComponent.ResetSpawner(); // Implement ResetState() in your component
//         }

//         // You can reset other properties or components here
//     }

    

//     private void LoadInitialScene()
//     {
//         string scenePath = "Assets/Scenes/TestScene.unity"; // Replace with your desired scene path
//         SceneRef sceneRef = SceneRef.FromIndex(SceneUtility.GetBuildIndexByScenePath(scenePath));

//         if (!_secondsceneLoaded)
//         {
//             Debug.Log("Loading initial scene...");
//             _networkRunner.LoadScene(sceneRef, LoadSceneMode.Additive);
//             _secondsceneLoaded = true;
//         }
//     }

//     private int GetLowestPlayerId()
//     {
//         int lowestId = int.MaxValue;

//         foreach (var player in _networkRunner.ActivePlayers)
//         {
//             if (player.PlayerId < lowestId)
//             {
//                 lowestId = player.PlayerId;
//             }
//         }

//         return lowestId;
//     }



// }



