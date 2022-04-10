using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // start the game 

    // controls on ground are visible until player moves

    // output cart / trolly is created and starts to move
    // spawn bubble on top of it for few sec with box type
    // 3 carts with boxes are created 
    // 2 elements that we dont need and one that we do
    // randomly positioned

    // if player drops good box 
    // if player drops bad box 
    // if player collide with cart 
    // time is added or removed / points 

    // if player looses all time / points 
    // game over 
    // if player passed the threshold 
    //  awarded with license 
    // else 
    // no license 
    // license screen / menu with restart game

    // main menu with start game
    // instead of main menu we can go directly in the game 
    // game starts when player moves 
    // esc menu to restart a game or pause 

    // general buttons


    // Singleton pattern
    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
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


    // Keeping scores 

    public void OnCartReachDestionation(int cargo_valid)
    {
        if (cargo_valid > 0)
        {
            UILayer.Instance.SetBottomText("Good delivery");
        }
        else
        {
            UILayer.Instance.SetBottomText("Bad delivery");
        }
    }

    public void OnCartDeraild()
    {
        UILayer.Instance.SetBottomText("Not cool");
    }


    public void OnGameStart()
    {
        // start main game
        SceneManager.LoadScene(1);
    }

    [SerializeField]
    GameObject pause_menu;
    public void OnGamePause()
    { 
    
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause_menu == null) return;
            PauseMenu pm = pause_menu.GetComponent<PauseMenu>();
            if (pm == null) return;

            if (pause_menu.gameObject.activeInHierarchy)
            {
                pm.ResumeGame();
            }
            else
            {
                pm.PauseGame();
            }
                
        }
    }

}
