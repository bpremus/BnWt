using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILayer : MonoBehaviour
{
    private static UILayer _instance;
    public Text _bottom_text;

    public static UILayer Instance
    {
        get { return _instance; }
    }

    public void SetBottomText(string str)
    {
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
