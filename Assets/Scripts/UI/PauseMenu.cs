using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    
    public void PauseGame()
    {
        this.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void RestartGame()
    { 
    
    }
}
