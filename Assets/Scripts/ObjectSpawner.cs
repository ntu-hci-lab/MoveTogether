using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Meta.XR.MultiplayerBlocks.Fusion;
using UnityEngine.SceneManagement;
using System.Data.Common;
using System.Drawing;
using UnityEngine.UI;
using System.Linq;

public class ObjectSpawner : MonoBehaviour
{   
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    public static ObjectSpawner instance;
    public float shootForce = 0.1f;
    public GameObject prefab;
    bool isSpawn = false;
    // public GameObject Trampoline;
    private NetworkRunner _networkRunner;
    private bool _sceneLoaded;
    private bool _secondsceneLoaded;
    public Text winText;
    int level;
    public Transform spawnPoint; // The transform where objects will be spawned
    public float spawnInterval = 5f; // Time interval between spawns

    // private int currentSpawnCount = 0; // Current number of spawned prefabs
    
    
    [Networked]
    private float timer { get; set; } // Timer to track spawn intervals
    [Networked]
    public int maxSpawnCount { get; set; } 
    
    [Networked]
    public int currentSpawnCount { get; set; } 

    // = 100; // Maximum number of spawned prefabs
    private void OnEnable()
    {
        FusionBBEvents.OnSceneLoadDone += OnLoaded;
    }

    private void OnDisable()
    {
        FusionBBEvents.OnSceneLoadDone -= OnLoaded;
    }

    private void OnLoaded(NetworkRunner networkRunner)
    {
        _sceneLoaded = true;
        _networkRunner = networkRunner;

        //LoadInitialScene();

    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void DespawnObject(NetworkObject no)
    {
        if (_networkRunner != null)
        {
            _networkRunner.Despawn(no);
            // currentSpawnCount--; // Decrement the spawn count
        }
    }
    // Update is called once per frame
    void Update()
    {   
        level = ChangeLevel.instance.GetLevel();
        if (level == 1){
            if (OVRInput.GetDown(OVRInput.Button.Three))
            {
                // spawning creatures
                // GameObject spawnedBall = Instantiate(prefab, transform.position, transform.rotation);
                _networkRunner.Spawn(
                    prefab,
                    Vector3.zero,
                    Quaternion.identity,
                    inputAuthority: null,
                    (runner, obj) => // onBeforeSpawned
                    {
                        obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        obj.transform.rotation = rightHandTransform.rotation;
                        obj.transform.position = rightHandTransform.position;
                    }
                );
            }

            // // spawning trampoline
            // if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && !isSpawn){
            //     Trampoline.transform.position = transform.position;
            //     Trampoline.transform.rotation = transform.rotation;
            //     _networkRunner.Spawn(
            //         Trampoline,
            //         Vector3.zero,
            //         Quaternion.identity,
            //         _networkRunner.LocalPlayer,
            //         // null,
            //         (runner, obj) => // onBeforeSpawned
            //         {
            //             obj.transform.rotation = leftHandTransform.rotation;
            //             obj.transform.position = leftHandTransform.position;

            //         }
            //     );
            //     isSpawn = true;
            // }

            
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                ResetScene();
            }
        }
    }

    public void ResetScene()
    {
        if (_networkRunner != null && _networkRunner.IsSceneAuthority)
        {
            // Find all network objects in the scene
            var networkObjects = FindObjectsOfType<NetworkObject>();

            // Check if this object should be reset or despawned
            foreach (var networkObject in networkObjects)
            {
                Debug.Log($"Checking NetworkObject: {networkObject.name}");
                if (networkObject.CompareTag("Resettable"))
                {
                    // Reset object instead of destroying
                    ResetNetworkObject(networkObject);
                }
                // else if (networkObject.HasStateAuthority)
                else if (networkObject.CompareTag("Trampoline") || networkObject.CompareTag("spirit"))
                {
                    // Despawn objects with trampoline tag and spirit tag
                    _networkRunner.Despawn(networkObject);
                }
            }

            // Reset spawn count and timers
            currentSpawnCount = 0;
            timer = 0f;
            isSpawn = false;

            Debug.Log("Scene reset completed.");
        }
        else
        {
            Debug.LogWarning("ResetScene called, but this instance is not the scene authority.");
        }
    }

    private void ResetNetworkObject(NetworkObject networkObject)
    {
        Debug.Log($"Resetting NetworkObject: {networkObject.name}");

        // Example: Reset position and rotation
        networkObject.transform.position = Vector3.zero;
        networkObject.transform.rotation = Quaternion.identity;

        // Example: Reset custom component states
        var myComponent = networkObject.GetComponent<TimedSpawner>();
        if (myComponent != null)
        {
            myComponent.ResetSpawner(); // Implement ResetState() in your component
        }

        var myscoreComponent = networkObject.GetComponent<WinScoreCounting>();
        if (myscoreComponent != null)
        {
            myscoreComponent.Reset(); // Implement ResetState() in your component
        }


        var mychangeLevelComponent = networkObject.GetComponent<ChangeLevel>();
        if (mychangeLevelComponent != null)
        {
            // mychangeLevelComponent.Reset(); // Implement ResetState() in your component
        }

        // You can reset other properties or components here
    }

    private void LoadInitialScene()
    {
        string scenePath = "Assets/Scenes/TestScene.unity"; // Replace with your desired scene path
        SceneRef sceneRef = SceneRef.FromIndex(SceneUtility.GetBuildIndexByScenePath(scenePath));

        if (!_secondsceneLoaded)
        {
            Debug.Log("Loading initial scene...");
            _networkRunner.LoadScene(sceneRef, LoadSceneMode.Additive);
            _secondsceneLoaded = true;
        }
    }

    private int GetLowestPlayerId()
    {
        int lowestId = int.MaxValue;

        foreach (var player in _networkRunner.ActivePlayers)
        {
            if (player.PlayerId < lowestId)
            {
                lowestId = player.PlayerId;
            }
        }

        return lowestId;
    }

}



