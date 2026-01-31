using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Meta.XR.MultiplayerBlocks.Fusion;
using UnityEngine.SceneManagement;
using System.Data.Common;
using System.Drawing;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    bool isSpawn = false;
    private NetworkRunner _networkRunner;
    private bool _sceneLoaded;
    private bool _secondsceneLoaded;


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

        LoadInitialScene();

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
        //if (_sceneLoaded)
        //{
        //    // Start the spawn coroutine once the scene is loaded
        //    StartCoroutine(SpawnRoutine());
        //}
    }
    // Update is called once per frame

    private void LoadInitialScene()
    {
        string scenePath = "Assets/Scenes/GameScene.unity"; // Replace with your desired scene path
        SceneRef sceneRef = SceneRef.FromIndex(SceneUtility.GetBuildIndexByScenePath(scenePath));

        if (!_secondsceneLoaded)
        {
            Debug.Log("Loading initial scene...");
            _networkRunner.LoadScene(sceneRef, LoadSceneMode.Additive);
            _secondsceneLoaded = true;
        }
    }


}

