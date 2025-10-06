using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Single<GameManager>
{
    public Weather currentWeather;
    protected override void Awake()
    {
        base.Awake();
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0);
    }
}
