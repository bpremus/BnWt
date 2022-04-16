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

    public GameObject[] grades;

    public void Update()
    { 
        // update scores 
        _good.text    = "" + GameManager.Instance.good_d;
        _bad.text     = "" + GameManager.Instance.bad_d;
        _deraild.text = "" + GameManager.Instance.derailed;
        int grade = GameManager.Instance.final_grade;

        for (int i = 0; i < 4; i++)
        {
      
            if (i == grade)
            {
                grades[i].gameObject.SetActive(true);
            }
            else
            {
                grades[i].gameObject.SetActive(false);
            }
        }

    }

    public void RestartGame()
    {
        Debug.Log("Restarting game");
        GameManager.Instance.RestartGame();
    }
}
