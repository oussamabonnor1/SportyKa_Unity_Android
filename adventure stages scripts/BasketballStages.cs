using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BasketballStages : MonoBehaviour {


    public GameObject bestScoreTxt;
    public loserBasketballStage loserScript;
    public hoopScoreStages hoopScore;
    static GameObject canvas;
    public GameObject restartQuitPannel;
    public GameObject popUpScoreTxt;
    public GameObject losingPannel;
    public GameObject scoreTxt;
    public GameObject leftEdge;
    public GameObject rightEdge;
    public GameObject[] spawnPositions;
    public GameObject hoop;
    public GameObject finger;
    public GameObject pausePannel;

    public GameObject soundButton;
    public Sprite[] soundImages;
    GameObject sfx;

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
    bool pause;

    public Vector2 origins;
    int[] positions = { -250, -150, 0, 150, 250 };

    Vector3 t;
    Vector3 t2;

    //force amount submited to the ball
    int force;
    //width of the ball
    int width;
    //gravity that affects the ball
    int gravity;
    //altitude of the ball in the first shot
    int altitude;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(fingerAnimation());

        sfx = GameObject.Find("SFX Manager");

        if (PlayerPrefs.GetInt("sound") == 1)
        {
            soundButton.GetComponent<Button>().GetComponent<Image>().sprite = soundImages[1];
        }
        else
        {
            soundButton.GetComponent<Button>().GetComponent<Image>().sprite = soundImages[0];
        }

        PlayerPrefs.SetInt("force", 0);

        //making the hoop appears under the ball (layout speaking)
        hoop.transform.parent = GameObject.Find("Background image").transform;
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
        pause = false;
        losePannelShown = false;

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
            if (!losePannelShown && PlayerPrefs.GetInt("hearts") > 0)
            {

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
            //worst case scenario (ball not stopped by loser collider cause unity s engine has a mind of it s own, or i suck :v
            if (transform.localPosition.y < -1300)
            {
                GetComponent<Rigidbody2D>().position = origins;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }

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

        if (gameObject.transform.localPosition.y > hoop.gameObject.transform.localPosition.y +100)
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
            if (!pause && !waiting && !hoopScore.pannelShowed && Vector2.Distance(t2, t) > width * 0.5f && !clicked && Vector2.Distance(t, gameObject.transform.position) <= width && t2.y > t.y)
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
        SceneManager.LoadScene("BasketballStagesScene");
    }

    void restartQuit()
    {
        sfx.GetComponent<sfxManager>().appear();
        waiting = true;
        restartQuitPannel.SetActive(true);
    }
    public void restartQuitYes()
    {
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("bestScoreBasketball", score);

            bestScore = score;
            bestScoreTxt.GetComponent<Text>().text = "" + bestScore;
        }

        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + score);
        if (score != 0) score = 0;
        sfx.GetComponent<sfxManager>().ok();
        SceneManager.LoadScene("BasketballStagePlayScene");
    }
    public void restartQuitNo()
    {
        ShowAd();
        NoFontion();
    }

    IEnumerator fingerAnimation()
    {
        int limit = (int)finger.transform.localPosition.y;
        for (int i = 0; i < 3; i++)
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
        for (int i = 0; i < 40; i++)
        {
            yield return new WaitForSeconds(0.001f);
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - (0.01f), gameObject.transform.localScale.y - (0.01f), 1);
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
        Vector2 direction = new Vector2((Direction.x) / 14, 100).normalized;

        GetComponent<Rigidbody2D>().AddForce(direction * force * Time.fixedDeltaTime, ForceMode2D.Impulse);

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
            //pos is the lenght we want our ball to travel
            force = (((int)(Screen.width / 200)) * 10000) + 60000;
            PlayerPrefs.SetInt("force", force);

        }

        bestScore = PlayerPrefs.GetInt("bestScoreBasketball");
        gravity = 700;
        GetComponent<Rigidbody2D>().gravityScale = gravity;
        bestScoreTxt.GetComponent<Text>().text = "" + bestScore;
    }

    public void NoFontion()
    {
        sfx.GetComponent<sfxManager>().quit();
        restartQuitPannel.SetActive(false);
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

        SceneManager.LoadScene("BasketballStagesScene");
    }

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    public void yesFonction()
    {
        if (PlayerPrefs.GetInt("hearts") > 0)
        {
            sfx.GetComponent<sfxManager>().ok();
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
        waiting = true;
        losingPannel.SetActive(true);
    }

}