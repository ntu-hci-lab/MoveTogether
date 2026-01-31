using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using System;

public class ChangeLevel : NetworkBehaviour
{
    public GameObject WaterInputManager;
    public GameObject EarthInputManager;
    public GameObject Trampoline;
    public GameObject Tube1;
    public GameObject Tube2;
    public static ChangeLevel instance;
    [Networked] public int level { get; set; } = 0;
    public event Action<int> OnLevelChanged;
    private int prevLevel = -1;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public int GetLevel()
    {
        return level;
    }
    public void SetLevel(int l)
    {
        level = l;
        OnLevelChanged?.Invoke(level);
    }

    // Update is called once per frame
    void Update()
    {   
        if (level != prevLevel) {
            if (level == 0) {
                // 
            } else if (level == 1) {
                // WaterInputManager.SetActive(false);
                // EarthInputManager.SetActive(true);
                // PositionalControl.instance.set_canStartPlaying(false);
                Trampoline.SetActive(true);
            } else if (level == 2) {
                Trampoline.SetActive(false);    
                Tube1.SetActive(false);
                Tube2.SetActive(true);
                PositionalControl.instance.set_canStartPlaying(true);
            } else if (level == 3) {
                Tube2.SetActive(false);
            }
        }
        prevLevel = level;
    }
}
