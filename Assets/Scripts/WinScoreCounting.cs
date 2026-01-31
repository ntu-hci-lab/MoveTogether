using Fusion;
using Meta.XR.MultiplayerBlocks.Fusion;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class WinScoreCounting : NetworkBehaviour
{
    [Networked] private int score { get; set; } = 0;
    public event Action<int> OnScoreChanged;
    [Networked] private int countdown { get; set; } = 60;
    public event Action<int> OnCountdownChanged;

    public static WinScoreCounting instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    private float t = 0;
    private int level = 0;
    private bool stop = true; // if true, the countdown will stop

    [Header("Tag Settings")]
    public string targetTag = "spirit"; // Tag for objects that should increase the score
    public void AddPoints()
    {
        // Debug.Log("Adding Score");
        score++;
        // UpdateScoreText();
        OnScoreChanged?.Invoke(score);
    }

    public void Reset()
    {
        score = 0;
        OnScoreChanged?.Invoke(score);
        countdown = 60;
        OnCountdownChanged?.Invoke(countdown);
        TimedSpawner.instance.SetStop(false);
        WaterSpiritSpawner.instance.SetStop(false);
        stop = false;
    }

    public int get_Score(){
        return score; 
    }
    public int get_Countdown(){
        return countdown;
    }

    private void UpdateCountdown() {
        t += Time.deltaTime;
        if (!stop && t >= 1.0f && countdown > 0)
        {
            t = 0f;
            countdown--;
            OnCountdownChanged?.Invoke(countdown);
        }
    }

    void Update()
    {
        level = ChangeLevel.instance.GetLevel();

        // start game after pressing A
        if (PositionalControl.instance.get_canStartPlaying() && OVRInput.GetDown(OVRInput.Button.One))
        {
            Reset();
            PositionalControl.instance.set_canStartPlaying(false);
        }

        // the Earth level
        Debug.Log("Level: " + level + "Score: " + score);
        if (level == 1){
            if (score >= 4)
            {
                TimedSpawner.instance.SetStop(true);
                stop = true;
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    Reset();
                    ObjectSpawner.instance.ResetScene();
                    ChangeLevel.instance.SetLevel(2);
                    stop = true;
                    WaterSpiritSpawner.instance.SetStop(true);
                }
            }
            else if (countdown == 0)
            {
                TimedSpawner.instance.SetStop(true);
                stop = true;
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    Reset();
                }
            }
            
            UpdateCountdown();

        // the Water level
        } else if (level == 2){
            if (score >= 3)
            {
                WaterSpiritSpawner.instance.SetStop(true);
                stop = true;
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    Reset();
                    ObjectSpawner.instance.ResetScene();
                    ChangeLevel.instance.SetLevel(3);
                    stop = true;
                }
            }
            else if (countdown == 0)
            {
                WaterSpiritSpawner.instance.SetStop(true);
                stop = true;
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    Reset();
                }
            }
            
            UpdateCountdown();

        // the Fire level
        } else if (level == 3){
            if (score >= 3)
            {
                stop = true;
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    Application.Quit();
                }
            }
            else if (countdown == 0)
            {
                stop = true;
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    Reset();
                }
            }
            
            UpdateCountdown();
        }   
    }
}
