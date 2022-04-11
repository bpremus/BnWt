using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILayer : MonoBehaviour
{
    private static UILayer _instance;
    public Text _bottom_text;
    public Text _timer_text;
    public TMP_Text _countdown_text;

    public static UILayer Instance
    {
        get { return _instance; }
    }

    // we need a queue and coroutine 

    float start_time = 1;
    public void Update()
    {
        start_time += Time.deltaTime;
        SetTimerText(Mathf.Floor(start_time) + " sec");
    }

    public void SetCountdownText(float time)
    {
        TimeSpan interval = TimeSpan.FromSeconds(time);
        _countdown_text.text = interval.ToString("mm\\:ss\\:ff");
    }

    public void SetTimerText(string str)
    {
        if (_timer_text)
            _timer_text.text = str;
    }
    public void SetBottomText(string str)
    {
        if (_bottom_text)
        _bottom_text.text = str;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
