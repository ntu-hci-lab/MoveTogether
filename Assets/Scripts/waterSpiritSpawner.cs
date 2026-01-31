using System;
using UnityEngine;
using Fusion;
using Meta.XR.MultiplayerBlocks.Fusion;
using Oculus.Interaction.Samples;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;
public class WaterSpiritSpawner : NetworkBehaviour
{
    [Header("Spawner Settings")]
    public GameObject prefab; // Prefab to spawn
    public Transform spawnPoint; // Spawn location
    public float spawnInterval = 5f; // Time interval between spawns
    public int maxSpawnCount = 100; // Maximum number of objects to spawn

    [Header("Physics Settings")]
    public Vector3 shootDirection = new Vector3(0f, -1f, 0f); // Direction to apply force
    public float shootForce = 10f; // Force to apply to the spawned prefab

    [Header("References")]
    private NetworkRunner _networkRunner;
    public static WaterSpiritSpawner instance;
    private bool _sceneLoaded;
    private bool _secondsceneLoaded;
    int level;
    private float timer; // Timer to track spawn intervals (networked for sync)
    // [Networked] private int currentSpawnCount { get; set; } // Number of spawned objects (networked)
    [Networked] public int remainingObjects { get; set; } = 100;

    public event Action<int> OnSpawnCountChanged; // Event to notify spawn count changes
    public bool stop = true;
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
    private void Start()
    {
        //_networkRunner = Runner; // Get reference to the NetworkRunner
    }
    public void SetStop(bool stop)
    {
        this.stop = stop;
        timer = 0f;
    }

    private void Update()
    {   
        level = ChangeLevel.instance.GetLevel();
        if (!stop && level == 2){
            // Increment the timer
            timer += Time.deltaTime;

            // Check if it's time to spawn and if the max spawn count hasn't been reached
            // if (timer >= spawnInterval && currentSpawnCount < maxSpawnCount)
            if (timer >= spawnInterval && remainingObjects > 0)
            {
                timer = 0f; // Reset the timer
                SpawnPrefab(); // Call the spawn logic
                // UpdateRemainingSpawnText();
            }
        }
    }
    // public void SetStart(){
    //     start = true;
    // }

    private void SpawnPrefab()
    {
        if (_networkRunner == null || prefab == null || spawnPoint == null)
        {
            Debug.LogWarning("Missing required references for spawning.");
            return;
        }
        Vector3 spawnPosition = spawnPoint.position;
        _networkRunner.Spawn(
            prefab,
            spawnPosition,
            spawnPoint.rotation,
            inputAuthority: null,
            (runner, obj) =>
            {
                // Set initial scale
                obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                // Apply force if Rigidbody is present
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(shootDirection.normalized * shootForce, ForceMode.Impulse);
                }
                else
                {
                    Debug.LogWarning("Rigidbody not found on the spawned prefab.");
                }
            }
        );

        // currentSpawnCount++; // Increment spawn count
        // OnSpawnCountChanged?.Invoke(currentSpawnCount); // Notify listeners
        // OnSpawnCountChanged?.Invoke(remainingObjects);
    }

    public void ResetSpawner()
    {
        timer = 0f;
        // currentSpawnCount = 0;
        // remainingObjects = 100;
        SetStop(false);
        Debug.Log("Spawner has been reset.");
        // OnSpawnCountChanged?.Invoke(currentSpawnCount);
        // OnSpawnCountChanged?.Invoke(remainingObjects);
    }
}
