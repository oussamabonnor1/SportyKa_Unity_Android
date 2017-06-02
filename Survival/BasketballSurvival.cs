using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasketballSurvival : MonoBehaviour {
    
    public GameObject bestScoreTxt;
    public loser loserScript;
    static GameObject canvas;
    public GameObject losingPannel;
    public GameObject restartQuitPannel;
    public GameObject popUpScoreTxt;
    public GameObject scoreTxt;
    public GameObject leftEdge;
    public GameObject rightEdge;
    public GameObject[] spawnPositions;
    public GameObject hoop;
    public GameObject finger;
    public GameObject backGround;
    public GameObject pausePannel;
    GameObject sfx;
    public Sprite[] soundImages;
    public GameObject soundButton;

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
    //hoop and net going right direction 
    bool goingRight;
    //pausing the game
    bool pause;

    public Vector2 origins;
    int[] positions = { -250, -150, 0, 150, 250 };

    Vector3 t;
    Vector3 t2;

    //force amount submited to the ball
    int force;
    //original force submited to the ball 
    //width of the ball
    int width;
    //gravity that affects the ball
    int gravity;
    //altitude of the ball in the first shot
    int altitude;

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

            StartCoroutine(fingerAnimation());

        PlayerPrefs.SetInt("force", 0);

        sfx = GameObject.Find("SFX Manager");

        //defines the force and gravity for each ball type
        choice(GetComponent<Image>().sprite.name);
        //finding the width of the ball to knw if the tap is inside
        width = (int)GetComponent<Image>().GetComponent<RectTransform>().rect.width / 2;
        //deactivating the edge colliders at first so that the ball can go through the hoop
        leftEdge.GetComponent<EdgeCollider2D>().isTrigger = true;
        rightEdge.GetComponent<EdgeCollider2D>().isTrigger = true;

        //canvas fetching
        canvas = GameObject.Find("Canvas");
        GetComponent<Rigidbody2D>().IsAwake();

        //original place of the ball so that it may retunr to it after the loss
        origins = gameObject.transform.position;

        scoreTxt.SetActive(false);

        altitude = 0;
        gameOver = false;
        start = false;
        first = true;
        lose = false;
        clicked = false;
        waiting = false;
        losePannelShown = false;
        pause = false;
        goingRight = Random.value >= 0.5f;
        
    }

    public void soundControll()
    {

        AudioSource c = GameObject.Find("Basketball Music Manager").GetComponent<AudioSource>();

        if (PlayerPrefs.GetInt("sound") == 1)
        {
            soundButton.GetComponent<Button>().GetComponent<Image>().sprite = soundImages[0];
            PlayerPrefs.SetInt("sound",0);
            c.Pause();
            Debug.Log(c.isPlaying + "  sound is " + PlayerPrefs.GetInt("sound"));
        }
        else
        {
            soundButton.GetComponent<Button>().GetComponent<Image>().sprite = soundImages[1];
            PlayerPrefs.SetInt("sound", 1);
            c.Play();
            Debug.Log(c.isPlaying + "  sound is " + PlayerPrefs.GetInt("sound"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(score >= 15 && !pause)
        {
            if (goingRight) {
                backGround.transform.localPosition = new Vector3(backGround.transform.localPosition.x + 1, backGround.transform.localPosition.y, backGround.transform.localPosition.z);
                hoop.transform.localPosition = new Vector3(hoop.transform.localPosition.x + 1, hoop.transform.localPosition.y, hoop.transform.localPosition.z);
                if (backGround.transform.localPosition.x >= 160) goingRight = false;
                //(Screen.width/2) - backGround.GetComponent<RectTransform>().rect.width / 2
            }
            else
            {
                backGround.transform.localPosition = new Vector3(backGround.transform.localPosition.x - 1, backGround.transform.localPosition.y, backGround.transform.localPosition.z);
                hoop.transform.localPosition = new Vector3(hoop.transform.localPosition.x - 1, hoop.transform.localPosition.y, hoop.transform.localPosition.z);
                if (backGround.transform.localPosition.x <= -165) goingRight = true;
            }
        }
        
        if (gameOver && !waiting)
        {
            if (score > bestScore)
            {
                PlayerPrefs.SetInt("bestScoreBasketball", score);

                bestScore = score;
                bestScoreTxt.GetComponent<Text>().text = "" + bestScore;
            }
            

            if (!losePannelShown && PlayerPrefs.GetInt("bestScoreBasketball") - score <= (int) (score / 10 * 2 +1) && PlayerPrefs.GetInt("hearts") > 0) {
                
                pannelMaking();
                losePannelShown = true;
                scoreTxt.SetActive(true);
            }
            else
            {
                scoreTxt.SetActive(true);
                restartQuit();
            }
        }
        if (lose)
        {
            if (Vector2.Distance(GetComponent<Rigidbody2D>().position, origins) >= 3)
            {
                GetComponent<Rigidbody2D>().MovePosition(origins);
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 1);
                lose = false;
                clicked = false;
                //freezing the ball so that it doesnt fall due to gravity
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            }

        }

        if (gameObject.transform.localPosition.y > hoop.gameObject.transform.localPosition.y+100)
        {
            leftEdge.GetComponent<EdgeCollider2D>().isTrigger = false;
            rightEdge.GetComponent<EdgeCollider2D>().isTrigger = false;
            hoop.transform.SetAsLastSibling();
        }
        else if (gameObject.transform.localPosition.y < -100)
        {
            leftEdge.GetComponent<EdgeCollider2D>().isTrigger = true;
            rightEdge.GetComponent<EdgeCollider2D>().isTrigger = true;
            hoop.transform.SetAsFirstSibling();
        }

        if (Input.GetMouseButtonDown(0))
        {
            t = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            t2 = Input.mousePosition;
            if (!pause && !waiting && Vector2.Distance(t2, t) > width * 0.5f && !clicked && Vector2.Distance(t, gameObject.transform.position) <= width && t2.y > t.y)
            {
                jump(t2 - t);
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (waiting)
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

        //worst case scenario
        if (transform.localPosition.y < -1300)
        {
            lose = true;
        }

    }


    void restartQuit()
    {
        sfx.GetComponent<sfxManager>().appear();
        restartQuitPannel.SetActive(true);
        gameObject.SetActive(false);
    }
    public void restartQuitYes()
    {
        sfx.GetComponent<sfxManager>().ok();
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("bestScoreBasketball", score);

            bestScore = score;
            bestScoreTxt.GetComponent<Text>().text = "" + bestScore;
        }
        
        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + score);
        if (score != 0) score = 0;


        AudioSource c = GameObject.Find("Basketball Music Manager").GetComponent<AudioSource>();
        c.Pause();
        SceneManager.LoadScene("BasketballScene");
    }
    public void restartQuitNo()
    {
        ShowAd();
        NoFontion();
    }

    IEnumerator fingerAnimation()
    {
        int limit = (int) finger.transform.localPosition.y;
           for (int i = 0; i< 3; i++)
         {
            do
            {
                yield return new WaitForSeconds(0.0001f);
                finger.GetComponent<Image>().transform.localPosition = new Vector3(finger.transform.localPosition.x, finger.transform.localPosition.y + 12, finger.transform.localPosition.z);
           } while (finger.transform.localPosition.y < limit + 600);
            

           finger.transform.localPosition = new Vector3(finger.transform.localPosition.x, limit, finger.transform.localPosition.z);

         }
        Destroy(finger);
    }

    IEnumerator visuelEffects()
    {
        for(int i = 0; i < 40; i++)
        {
            yield return new WaitForSeconds(0.001f);
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x- (0.01f), gameObject.transform.localScale.y - (0.01f), 1);
        }
    }

    public void jump(Vector3 Direction)
    {
        origins = spawnPositions[Random.Range(0, 5)].transform.position;

        StartCoroutine(visuelEffects());
        clicked = true;

        //start boolean is a var that determines wether or not we just started a new session of "tel9am"
        if (!start) start = true;

        //hi text instatiated only in first use
        if (first)
        {
            GameObject floatingText = (GameObject)Instantiate(popUpScoreTxt, canvas.transform.position, Quaternion.identity);
            floatingText.GetComponentInChildren<TxtController>().first = first;
            floatingText.transform.SetParent(canvas.transform, false);
            floatingText.transform.position = new Vector3(transform.position.x, transform.position.y + 5, 0);
            first = false;
        }
        //adds force to rigid body
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        //Vector2 direction = new Vector2((transform.position.x - Input.mousePosition.x) / 2, 100).normalized;
        Vector2 direction = new Vector2((Direction.x / 20), 100).normalized;

        GetComponent<Rigidbody2D>().AddForce(direction * force * Time.fixedDeltaTime,ForceMode2D.Impulse);

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
        Time.timeScale = 1;
        ShowAd();
        restartQuitYes();
    }

    public void pausePannelQuit()
    {
        sfx.GetComponent<sfxManager>().quit();
        Time.timeScale = 1;
        ShowAd();

        //part fo No fonction
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("bestScoreBasketball", score);

            bestScore = score;
            bestScoreTxt.GetComponent<Text>().text = "" + bestScore;
        }

        gameOver = false;

        Analytics.CustomEvent("gameOver", new Dictionary<string, object>
                 {
                 { "hearts", PlayerPrefs.GetInt("hearts")},
                 { "best_score", bestScore },
                 { "score", score }
                      });

        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + score);
        if (score != 0) score = 0;
        

        AudioSource c = GameObject.Find("Basketball Music Manager").GetComponent<AudioSource>();
        c.Pause();

        PlayerPrefs.SetString("PreviousScene", "Survival");

        SceneManager.LoadScene("AdventureScene");
    }

    void choice(string name)
    {

        //choosing a force
        if (PlayerPrefs.GetInt("force") == 0)
        {
            force = 65000;
        }
        else
        {
            force = PlayerPrefs.GetInt("force");
        }

        //very messy thing coming up: checking if the force is callibrated + te altitude isnt a 0 so that it doesnt callibrate untill u shoot:
        if (PlayerPrefs.GetInt("force") != force)
        {
            if (Screen.width <= 400)
            {
                //check 
                Debug.Log("0");
                force = (((int)(Screen.width / 200)) * 9000) + 45000;
                gravity = 650;
            }
            else if (Screen.width <= 600) {
                //check 
                Debug.Log("1");
                force = (((int)(Screen.width / 200)) * 9000) + 50000;
                gravity = 700;
            }
            else if(Screen.width <800)
            {
                Debug.Log("2");
                //check
                force = (((int)(Screen.width / 200)) * 9000) + 60000;
                gravity = 700;
            }
            else if (Screen.width < 1200)
            {
                Debug.Log("3");
                //check
                force = (((int)(Screen.width / 200)) * 10000) + 55000;
                gravity = 750;
            }
            else
            {
                Debug.Log("4");
                //still
                force = (((int)(Screen.width / 200)) * 9000) + 55000;
                gravity = 700;
            }
            
         }else
        {
            PlayerPrefs.SetInt("force", force);
        }

        bestScore = PlayerPrefs.GetInt("bestScoreBasketball");
        
        GetComponent<Rigidbody2D>().gravityScale = gravity;
        bestScoreTxt.GetComponent<Text>().text = "" + bestScore;
    }

    public void NoFontion()
    {
        sfx.GetComponent<sfxManager>().quit();
        losingPannel.SetActive(false);
        waiting = false;
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("bestScoreBasketball", score);
            
            bestScore = score;
            bestScoreTxt.GetComponent<Text>().text = "" + bestScore;
        }

        gameOver = false;

        Analytics.CustomEvent("gameOver", new Dictionary<string, object>
                 {
                 { "hearts", PlayerPrefs.GetInt("hearts")},
                 { "best_score", bestScore },
                 { "score", score }
                      });

        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + score);
        if (score != 0) score = 0;

        PlayerPrefs.SetString("PreviousScene", "Survival");

        AudioSource c = GameObject.Find("Basketball Music Manager").GetComponent<AudioSource>();
        c.Pause();

        SceneManager.LoadScene("AdventureScene");
    }

    public void yesFonction()
    {
        if (PlayerPrefs.GetInt("hearts") > 0)
        {
            sfx.GetComponent<sfxManager>().appear();
            losingPannel.SetActive(false);
            waiting = false;
            PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") - 1);
            loserScript.currentLife = 1;
            loserScript.lifes.GetComponent<Image>().sprite = loserScript.balls[0];
            loserScript.lifes.SetActive(true);
            gameOver = false;
            scoreTxt.SetActive(false);
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
    }

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }
   
}


