using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Meta.XR.MultiplayerBlocks.Fusion;

public class NewBehaviourScript : MonoBehaviour
{
    // Attatched on the controllers without pushing the button
    public Transform leftController;
    public Transform rightController;
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

    }

    // Update is called once per frame
    void Update()
    {
        // if (_networkRunner.LocalPlayer.PlayerId == GetLowestPlayerId())
        // {
            this.transform.position = (leftController.position + rightController.position) / 2;
            // Vector3 direction = rightController.position - leftController.position;
            // this.transform.rotation = Quaternion.LookRotation(direction);
            Vector3 direction = rightController.position - leftController.position;
            Vector3 upDirection = (leftController.up + rightController.up).normalized;
            this.transform.rotation = Quaternion.LookRotation(direction, upDirection);
        // }
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
