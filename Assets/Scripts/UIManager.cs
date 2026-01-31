using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using Meta.XR.MultiplayerBlocks.Fusion;
public class UIManager : MonoBehaviour
{
    public GameObject UIpressA;
    public GameObject UIwin;
    public GameObject UIlose;
    public GameObject UIwin_2;
    public GameObject UIlose_2;
    public GameObject UIwinquit;
    public Text scoreText1, scoreText2, scoreText3;
    public Text timerText1, timerText2, timerText3;

    private int score;
    private int countdown;
    private int level;
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

    private void UpdateScoreText()
    {
        scoreText1.text = $"{score}";
        scoreText2.text = $"{score}";
        scoreText3.text = $"{score}";
    }

    private void UpdateCountdownText()
    {
        timerText1.text = $"{countdown}";
        timerText2.text = $"{countdown}";
        timerText3.text = $"{countdown}";
    }
    void deactivatetext()
    {
        UIpressA.SetActive(false);
        UIwin.SetActive(false);
        UIwin_2.SetActive(false);
        UIlose.SetActive(false);
        UIlose_2.SetActive(false);
        UIwinquit.SetActive(false);
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

    void Update()
    {
        score = WinScoreCounting.instance.get_Score();
        countdown = WinScoreCounting.instance.get_Countdown();
        UpdateScoreText();
        UpdateCountdownText();
        deactivatetext();

        level = ChangeLevel.instance.GetLevel();
        if (level == 0){
            if (!PositionalControl.instance.get_IsVideoPlaying() && _networkRunner.LocalPlayer.PlayerId == GetLowestPlayerId()) {
                UIpressA.SetActive(true);
            }
        }
        else if (PositionalControl.instance.get_canStartPlaying()) {
            if (_networkRunner.LocalPlayer.PlayerId == GetLowestPlayerId()) {
                UIpressA.SetActive(true);
            }
        }

        if (level == 1){
            if (score >= 4) // Todo: set the winning condition
            {
                Debug.Log("You Win!");
                UIwin.SetActive(_networkRunner.LocalPlayer.PlayerId == GetLowestPlayerId());
                UIwin_2.SetActive(_networkRunner.LocalPlayer.PlayerId != GetLowestPlayerId());

            }
            else if (countdown == 0)
            {
                Debug.Log("You Lose!");
                UIlose.SetActive(_networkRunner.LocalPlayer.PlayerId == GetLowestPlayerId());
                UIlose_2.SetActive(_networkRunner.LocalPlayer.PlayerId != GetLowestPlayerId());
            }
        }

        // the Water level
        else if (level == 2){
            if (score >= 3)
            {
                Debug.Log("You Win!");
                UIwin.SetActive(_networkRunner.LocalPlayer.PlayerId == GetLowestPlayerId());
                UIwin_2.SetActive(_networkRunner.LocalPlayer.PlayerId != GetLowestPlayerId());
            }
            else if (countdown == 0)
            {
                Debug.Log("You Lose!");
                UIlose.SetActive(_networkRunner.LocalPlayer.PlayerId == GetLowestPlayerId());
                UIlose_2.SetActive(_networkRunner.LocalPlayer.PlayerId != GetLowestPlayerId());

            }

        // the Fire level
        } else if (level == 3) {
            if (score >= 3) {
                Debug.Log("You Win!");
                UIwinquit.SetActive(true);
            }
            else if (countdown == 0) {
                Debug.Log("You Lose!");
                UIlose.SetActive(_networkRunner.LocalPlayer.PlayerId == GetLowestPlayerId());
                UIlose_2.SetActive(_networkRunner.LocalPlayer.PlayerId != GetLowestPlayerId());
            }
        }
    }
}
