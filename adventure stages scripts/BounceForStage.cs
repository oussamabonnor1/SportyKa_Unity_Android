using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BounceForStage: MonoBehaviour
{
    public GameObject scoreTxt;
    public GameObject bestScoreTxt;
    public GameObject heartsText;
    static GameObject canvas;
    public GameObject losingPannel;
    public GameObject restartQuitPannel;
    public GameObject startTutorialPannel;
    public GameObject enemySpawner;
    public GameObject enemyPrefab;
    public GameObject pausePannel;
    public GameObject nextStagePannel;
    GameObject sfx;


    public Sprite[] soundImages;
    public GameObject soundButton;
    public Sprite[] fingers;
    public GameObject[] missionTexts;
    public GameObject actualFinger;

    public int score;
    int bestScore;
    //player droped ball
    public bool gameOver;
    //player is playing
    public bool start;
    //first time player is playing
    public bool first;
    //player lost and ball is going back
    public bool lose;
    //player clicked somewhere (ball or out ball)
    bool clicked;
    //waiting for the player to answer is he uses a heart or not 
    bool waiting;
    //checking if the losing pannel has be instantiated in this session
    bool losePannelShown;
    //paused the game 
    bool pause;
    //next stage pannel showed
    public bool pannelShowed;

    Vector2 origins;

    //force amount submited to the ball
    int force;
    //original force submited to the ball 
    int originalForce;
    //width of the ball
    int width;
    //gravity that affects the ball
    int gravity;
    //original gravity of the ball 
    int originalGravity;
    //the stage that we r plaing
    int currentStageIndex;


    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            soundButton.GetComponent<Button>().GetComponent<Image>().sprite = soundImages[1];
        }
        else
        {
            soundButton.GetComponent<Button>().GetComponent<Image>().sprite = soundImages[0];
        }

        sfx = GameObject.Find("SFX Manager");

        StartCoroutine(fingerAnimation());

        //getting the stage and the text set and ready
        string holder = PlayerPrefs.GetString("footballStagesMenuLocation");
        char currentStageIndexChar = holder[holder.Length - 1];
        currentStageIndex = (int)System.Char.GetNumericValue(currentStageIndexChar);

        missionTexts[currentStageIndex - 1].SetActive(true);


        //defines the force and gravity for each ball type
        choice(GetComponent<Image>().sprite.name);
        width = (int)GetComponent<Button>().GetComponent<RectTransform>().rect.width;

        //canvas fetching
        canvas = GameObject.Find("Canvas");
        GetComponent<Rigidbody2D>().IsAwake();

        //hearts and best score texts set up
        heartsText.GetComponent<Text>().text = "X " + PlayerPrefs.GetInt("hearts");

        //the ball s original place
        origins = gameObject.transform.position;

        gameOver = false;
        start = false;
        first = true;
        lose = false;
        clicked = false;
        waiting = false;
        pause = false;
        losePannelShown = false;
        pannelShowed = true;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("enemy"))
        {
            gameOver = true;
            start = false;
            lose = true;
            sfx.GetComponent<sfxManager>().kill();
        }

    }

    public void soundControll()
    {
        AudioSource c = GameObject.Find("Music Manager").GetComponent<AudioSource>();

        if (PlayerPrefs.GetInt("sound") == 1)
        {
            soundButton.GetComponent<Button>().GetComponent<Image>().sprite = soundImages[0];
            PlayerPrefs.SetInt("sound", 0);
            c.Pause();
        }
        else
        {
            soundButton.GetComponent<Button>().GetComponent<Image>().sprite = soundImages[1];
            PlayerPrefs.SetInt("sound", 1);
            c.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver && !waiting)
        {
            if (!losePannelShown  && PlayerPrefs.GetInt("hearts") > 0)
            {
                pannelMaking();
                losePannelShown = true;
            }
            else
            {
                restartQuit();
            }
        }
        if (lose)
        {
            if (Vector2.Distance(GetComponent<Rigidbody2D>().transform.position, origins) >= 7)
            {
                GetComponent<Rigidbody2D>().MovePosition(origins);
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {

                lose = false;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            }

        }

        if (gameObject.transform.position.y >= (Screen.height * 0.8))
        {
            GetComponent<Rigidbody2D>().gravityScale = gravity + 100;
        }
        else
        {
            GetComponent<Rigidbody2D>().gravityScale = gravity;
        }

        if (Input.GetMouseButton(0) && gameOver == false)
        {

            //if the click happened
            if (!clicked)
            {

                //and the click wasnt suspended cause of the frames
                if (!pannelShowed && !pause && Vector2.Distance(Input.mousePosition, transform.position) <= width)
                {
                    //if the click inside the ball
                    jump();
                    clicked = true;
                    StartCoroutine(clickTimer(0.2f));
                }
                else
                {//if click not inside the ball
                    if (start)
                    {//if the ball isnt in lose phase (floating)
                        clicked = true;
                        StartCoroutine(clickTimer(0.22f));
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (waiting && losePannelShown)
            {
                ShowAd();
                NoFontion();
            }
            else if (pause)
            {
                pausePannelContinue();
            }
            else
            {
                pausePannelShow();
            }

        }

    }

    void restartQuit()
    {
        restartQuitPannel.SetActive(true);
    }
    public void restartQuitYes()
    {
        sfx.GetComponent<sfxManager>().ok();
        if (score > bestScore)
        {

            PlayerPrefs.SetInt("bestScoreFootball", score);

            bestScore = score;
            bestScoreTxt.GetComponent<Text>().text = "best score: " + bestScore;

        }

        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + score);
        SceneManager.LoadScene("FootballStagePlayScene");
    }
    public void restartQuitNo()
    {
        ShowAd();
        NoFontion();
    }

    IEnumerator fingerAnimation()
    {
        int i = 0;
        do
        {
            actualFinger.GetComponent<Image>().sprite = fingers[i % 2];
            yield return new WaitForSeconds(0.5f);
            i++;
        } while (i < 5);
        Destroy(actualFinger);
    }

    IEnumerator clickTimer(float time)
    {
        yield return new WaitForSeconds(time);
        clicked = false;
    }

    public void jump()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            GetComponent<AudioSource>().Play();
        }
        ++score;

        bool tester = Random.value >= 0.7f;
        
        //setting up the win conditions
        if (currentStageIndex < 3)
        {
            stageWon(score, false);
        }
        else
        {
            stageWon(score, true);
        }

        //start boolean is a var that determines wether or not we just started a new session of "tel9am"
        if (!start) start = true;
        //creates the score floating text
        GameObject floatingText = (GameObject)Instantiate(scoreTxt, canvas.transform.position, Quaternion.identity);
        if (first)
        {
            floatingText.GetComponentInChildren<TxtController>().first = first;
            first = false;
        }
        floatingText.GetComponentInChildren<TxtController>().score = score;
        floatingText.transform.SetParent(canvas.transform, false);
        floatingText.transform.position = new Vector3(transform.position.x, transform.position.y + 5, 0);

        //adds force to rigid body
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        Vector2 direction = new Vector2((transform.position.x - Input.mousePosition.x) / 2, 100).normalized;

        GetComponent<Rigidbody2D>().AddForce(direction * force);

    }

    void stageWon(int score, bool condition)
    {
        int scoreToWin = 0;

        switch (currentStageIndex)
        {
            case 1:
                scoreToWin = 5;
                break;
            case 2:
                scoreToWin = 10;
                break;
            case 3:
                scoreToWin = 10;
                break;
            case 4:
                scoreToWin = 20;

                break;
            case 5:
                scoreToWin = 30;
                break;
        }

        bool enemyPossibilities = Random.value >= 0.6;

        if (score >= 5 && condition && enemyPossibilities)
        {
            StartCoroutine(enemy());
        }

        if (scoreToWin <= score)
        {
            StartCoroutine(nextStage());
        }
    }

    IEnumerator nextStage()
    {
        sfx.GetComponent<sfxManager>().stageWon();
        yield return new WaitForSeconds(0.5f);
        nextStagePannel.SetActive(true);
        pannelShowed = true;
        int check = PlayerPrefs.GetInt("footballStage") + 1;
        ++check;
        Debug.Log(check);
        if (check >= 6)
        {
            if ((PlayerPrefs.GetInt("footballFinished") == 0))
            {
                Debug.Log("happened");
                PlayerPrefs.SetInt("footballFinished", 1);
            }
        }
        else
        {
            if (PlayerPrefs.GetString("footballStagesMenuLocation").Equals("stage " + (check-1)))
            {
                Debug.Log("nxt stage");
                PlayerPrefs.SetInt("footballStage", PlayerPrefs.GetInt("footballStage") + 1);
                PlayerPrefs.SetString("footballStagesMenuLocation", "stage " + check);
            }

        }
        //footballStagesMenuLocation + footballStage

    }

    public void nextStageYes()
    {
        sfx.GetComponent<sfxManager>().ok();
        
        
        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + score);
        SceneManager.LoadScene("FootballStagesScene");

    }
    public void nextStageNo()
    {

        ShowAd();
        NoFontion();
    }

    public void gotIt()
    {
        sfx.GetComponent<sfxManager>().ok();
        startTutorialPannel.SetActive(false);
        pannelShowed = false;
    }

    void pausePannelShow()
    {
        sfx.GetComponent<sfxManager>().appear();
        pause = true;
        Time.timeScale = 0;
        pausePannel.SetActive(true);
        pausePannel.transform.SetAsLastSibling();
    }

    public void pausePannelContinue()
    {
        sfx.GetComponent<sfxManager>().ok();
        pause = false;
        Time.timeScale = 1;
        pausePannel.SetActive(false);
    }

    public void pausePannelRestart()
    {
        sfx.GetComponent<sfxManager>().ok();
        Time.timeScale = 1;
        ShowAd();
        restartQuitYes();
    }

    public void pausePannelQuit()
    {
        sfx.GetComponent<sfxManager>().quit();
        Time.timeScale = 1;
        ShowAd();
        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + score);
        SceneManager.LoadScene("FootballStagesScene");
    }

    IEnumerator enemy()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        Vector2 position = new Vector2(enemySpawner.transform.position.x + Random.Range(-Screen.width / 2 + 150, Screen.width / 2 - 150), enemySpawner.transform.position.y);
        GameObject currentEnemy = (GameObject)Instantiate(enemyPrefab, position, enemyPrefab.transform.rotation);
        currentEnemy.transform.parent = canvas.transform;
    }

    void choice(string name)
    {

        bestScore = PlayerPrefs.GetInt("bestScoreFootball");
        //originalGravity = 450;
        gravity = (((int)(Screen.width / 200)) * 50) + 350;
        force = (((int)(Screen.width / 200)) * 5000) + 40000;
        PlayerPrefs.SetInt("force", force);
        GetComponent<Rigidbody2D>().gravityScale = gravity;

        bestScoreTxt.GetComponent<Text>().text = "best score: " + bestScore;
    }

    public void NoFontion()
    {
        sfx.GetComponent<sfxManager>().quit();

        if (PlayerPrefs.GetString("PreviousScene").Equals("Survival"))
        {
            force = originalForce;
            gravity = originalGravity;
        }

        losingPannel.SetActive(false);
        waiting = false;
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("bestScoreFootball", score);

            bestScore = score;
            bestScoreTxt.GetComponent<Text>().text = "best score: " + bestScore;

        }

        gameOver = false;

        Analytics.CustomEvent("gameOver", new Dictionary<string, object>
                 {
                 { "hearts", PlayerPrefs.GetInt("hearts")},
                 { "score", score }
                      });

        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + score);
        if (score != 0) score = 0;
        SceneManager.LoadScene("FootballStagesScene");
    }

    public void yesFonction()
    {
        sfx.GetComponent<sfxManager>().ok();
        if (PlayerPrefs.GetInt("hearts") > 0)
        {
            losingPannel.SetActive(false);
            waiting = false;
            PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") - 1);
            heartsText.GetComponent<Text>().text = "X " + PlayerPrefs.GetInt("hearts");
            gameOver = false;
            gameObject.SetActive(true);
        }
        else
        {
            NoFontion();
        }
    }

    void pannelMaking()
    {
        sfx.GetComponent<sfxManager>().appear();
        waiting = true;
        losingPannel.SetActive(true);
        losePannelShown = true;
    }

    public void hide()
    {
        sfx.GetComponent<sfxManager>().quit();
        clicked = false;
        startTutorialPannel.SetActive(false);
    }

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }
}
