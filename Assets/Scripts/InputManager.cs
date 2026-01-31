// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Fusion;

// public class InputManager : MonoBehaviour
// {
//     public GameObject Trampoline;
//     public GameObject prefab;
//     bool isSpawn = false;
//     public static InputManager instance;
//     private NetworkRunner _networkRunner;
//     void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//         }
//     }
//     public void setIsSpawntoTrue()
//     {
//         isSpawn = true;
//     }
//     public void setIsSpawntoFalse()
//     {
//         isSpawn = false;
//     }
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
//         {
//             ObjectSpawner.instance.SpawnPrefab(prefab);
//         }
//         Trampoline.transform.position = transform.position;
//         Trampoline.transform.rotation = transform.rotation;
//         if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && !isSpawn)
//         {
//             ObjectSpawner.instance.SpawnTrampo(Trampoline, isSpawn);
//         }

//         if (OVRInput.GetDown(OVRInput.Button.Two))
//         {
//             ResetScene();
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
//             // currentSpawnCount = 0;
//             // timer = 0f;
//             isSpawn = false;

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
//         var myComponent = networkObject.GetComponent<TimedSpawner>();
//         if (myComponent != null)
//         {
//             myComponent.ResetState(); // Implement ResetState() in your component
//         }

//         // You can reset other properties or components here
//     }
// }
