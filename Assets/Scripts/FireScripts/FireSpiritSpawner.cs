using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

using Meta.XR.MultiplayerBlocks.Fusion;

public class FireSpiritSpawner : NetworkBehaviour
{
    public GameObject prefab; // Prefab to spawn
    public Transform spawnPoint; // Spawn location
    private NetworkRunner _networkRunner;
    private bool _sceneLoaded;
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
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SpawnPrefab();
        }
    }

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
                // Rigidbody rb = obj.GetComponent<Rigidbody>();
                // if (rb != null)
                // {
                //     rb.AddForce(shootDirection.normalized * shootForce, ForceMode.Impulse);
                // }
                // else
                // {
                //     Debug.LogWarning("Rigidbody not found on the spawned prefab.");
                // }
            }
        );
    }

}
