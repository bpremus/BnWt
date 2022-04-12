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
    [SerializeField]
    public int total_score = 0;
    public int good_d = 0;
    public int bad_d = 0;
    public int derailed = 0;
    public int final_grade = 0;
    public int good_delivery =  10;
    public int bad_delivery  = -10;
    public int derail_cart   = -20;


    [SerializeField]
    private float timerMinutes = 10.0f;
    private bool  timeRunning = false;
    private float remainingTime, totalTime;

    public float good_time_add = 30;
    public float bad_time_add = -30;


    public void OnCartReachDestination(int cargo_valid)
    {
        if (cargo_valid > 0)
        {
            UILayer.Instance.SetBottomText("Good delivery");
            total_score += good_delivery;

            remainingTime += good_time_add;
            good_d++;
        }
        else
        {
            UILayer.Instance.SetBottomText("Bad delivery");
            total_score += derail_cart;

            remainingTime += bad_time_add;
            bad_d++;
        }
    }

    public void OnCartDerailed()
    {
        UILayer.Instance.SetBottomText("Not cool");
        total_score += bad_delivery;
        remainingTime += bad_time_add;
        derailed++;
    }

    public void Start()
    {
        OnTimerStart();
    }

    public void OnTimerStart()
    {
        timeRunning = true;
        totalTime = timerMinutes * 60;
        remainingTime = totalTime;
    }

    public void RunTimer()
    {
        if (timeRunning && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
            timeRunning = false;
        }

        UILayer.Instance.SetCountdownText(remainingTime);
    }

    public void OnTimerPause()
    {
        timeRunning = false;
    }

   
    public void OnGameStart()
    {
        // start main game
        SceneManager.LoadScene(1);
        MusicPlayer.instance.PlayMainMusic();
        OnTimerStart();
        _gameEnd = false;
    }

    public void LoadScoreScreen()
    {
        remainingTime = 0;
        SceneManager.LoadScene(2);
    }

    public void OnGamePause()
    {
        OnTimerPause();
    }

    bool _gameEnd = false;

    public void CheckScore()
    {
      
       // total_score = 0;
       // good_d = 0;
       // bad_d = 0;
       // derailed = 0;

        if (remainingTime <= 0)
        { 
            // Game has ended

            // lets count the score 
            if (total_score < 100)
            {
                OnGameEnd(0);
            }
            if (total_score > 50)
            {
                OnGameEnd(1);
            }
            if (total_score > 150)
            {
                OnGameEnd(2);
            }
            if (total_score > 200)
            {
                OnGameEnd(3);
            }

            return;
        }

        
        // show positive score but keep the real one 
        int positive_score = total_score;
        if (positive_score < 0) positive_score = 0;
        UILayer.Instance.SetScore(positive_score);
    }


    public void OnGameEnd(int grade)
    {
        UILayer.Instance.ClearScreen();
        // load the score screen
        this.final_grade = grade;

        _gameEnd = true;
        
    }

    public void RestartGame()
    { 
    
    }


    public void Update()
    {
        if (UILayer.Instance) { RunTimer(); }

        if (Input.GetKeyDown(KeyCode.Escape) && UILayer.Instance)
        {
            if (UILayer.Instance.PauseMenu == null) return;
            PauseMenu pm = UILayer.Instance.PauseMenu.GetComponent<PauseMenu>();
            if (pm == null) return;

            if (SceneManager.GetActiveScene().buildIndex == 1) // only on game scene
            {
                if (UILayer.Instance.PauseMenu.gameObject.activeInHierarchy)
                {
                    pm.ResumeGame();
                }
                else
                {
                    pm.PauseGame();
                }
            }
        }

        if (SceneManager.GetActiveScene() != SceneManager.GetSceneAt(0)) { CheckScore(); }
    }

}
