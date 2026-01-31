// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Fusion;
// using Meta.XR.MultiplayerBlocks.Fusion;

// public class Controller : MonoBehaviour
// {
//     bool isSpawn = false;
//     public GameObject Trampoline;
//     public Transform handTransform;
//     private NetworkRunner _networkRunner;
//     private bool _sceneLoaded;
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
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         Trampoline.transform.position = handTransform.position;
//         Trampoline.transform.rotation = handTransform.rotation;
//         if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && isSpawn == false){
//             _networkRunner.Spawn(
//                 Trampoline,
//                 Vector3.zero,
//                 Quaternion.identity,
//                 _networkRunner.LocalPlayer,
//                 // null,
//                 (runner, obj) => // onBeforeSpawned
//                 {
//                     obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
//                     obj.transform.rotation = transform.rotation;
//                     obj.transform.position = transform.position;

//                 }
//             );
//             isSpawn = true;
//         }
//     }
// }
