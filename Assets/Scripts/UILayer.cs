using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UILayer : MonoBehaviour
{
    private static UILayer _instance;
    public Text _bottom_text;
    public Text _timer_text;
    public Text _gameScore;
    public TMP_Text _countdown_text;

    //private Tween messageFade;

    public static UILayer Instance
    {
        get { return _instance; }
    }

    // we need a queue and coroutine 

    float start_time = 1;
    public void Update()
    {
        //start_time += Time.deltaTime;
        //SetTimerText(Mathf.Floor(start_time) + " sec");
    }

    public void ClearScreen()
    {
        _bottom_text.text = "";
        _timer_text.text = "";
        _gameScore.text = "";
    }

    public void SetScore(int points)
    {
        _gameScore.text = points + "";
    }

    public void SetCountdownText(float time)
    {
        TimeSpan interval = TimeSpan.FromSeconds(time);
        _countdown_text.text = interval.ToString("mm\\:ss\\:ff");
        string t = interval.ToString("mm\\:ss").ToString();
        SetTimerText(t);
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

    // Past attempt to get a text fade method working with the masking shader. 
    /*
    public void FadeText(Text text, float endValue, float duration, TweenCallback action)
    {
        if (messageFade != null) { messageFade.Kill(false); }

        text.material.SetFloat("_Masking", endValue);
    }
    */

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
