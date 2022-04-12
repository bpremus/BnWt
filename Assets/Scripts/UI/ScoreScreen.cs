using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreScreen : MonoBehaviour
{
    public TMP_Text _good;
    public TMP_Text _bad;
    public TMP_Text _deraild;
    public TMP_Text _grade;

    public void Update()
    { 
        // update scores 
        _good.text    = "" + GameManager.Instance.good_d;
        _bad.text     = "" + GameManager.Instance.bad_d;
        _deraild.text = "" + GameManager.Instance.derailed;
        _grade.text   = "" + GameManager.Instance.final_grade;
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }
}
