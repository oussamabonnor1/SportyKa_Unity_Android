using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class hoopScoreStages : MonoBehaviour {
    public BasketballStages a;
    public wallHitControllerForStages hit;
    public GameObject scoreTxt;
    static GameObject canvas;
    public GameObject nextstagePannel;
    public GameObject[] missionTexts;
    public GameObject pannelMission;
    GameObject sfx;

    public leftHoopEdgeHit hitLeft;
    public rightHoopEdgeHit hitRight;

    public bool enteredOnce;
    public bool pannelShowed;


    // Use this for initialization
    void Start()
    {
        sfx = GameObject.Find("SFX Manager");

        enteredOnce = false;
        pannelShowed = false;
        //canvas fetching
        canvas = GameObject.Find("Canvas");


        //getting the stage and the text set and ready
        sfx.GetComponent<sfxManager>().appear();
        string holder = PlayerPrefs.GetString("stagesMenuLocation");
        char currentStageIndexChar = holder[holder.Length - 1];
        int currentStageIndex = (int)Char.GetNumericValue(currentStageIndexChar);

        missionTexts[currentStageIndex - 1].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!a.leftEdge.GetComponent<EdgeCollider2D>().isTrigger && !a.rightEdge.GetComponent<EdgeCollider2D>().isTrigger)
        {

            //creates the score floating text
            GameObject floatingText = (GameObject)Instantiate(scoreTxt, canvas.transform.position, Quaternion.identity);
            if (!enteredOnce)
            {
                if (PlayerPrefs.GetInt("sound") == 1)
                {
                    GetComponent<AudioSource>().Play();
                }
                a.score++;
                enteredOnce = true;
                floatingText.GetComponentInChildren<TxtController>().score = a.score;
                floatingText.transform.SetParent(canvas.transform, false);
                floatingText.transform.position = new Vector3(transform.position.x, transform.position.y - 200, 0);
                a.scoreTxt.GetComponent<Text>().text = "Score: " + a.score;

                string holder = PlayerPrefs.GetString("stagesMenuLocation");
                char currentStageIndexChar = holder[holder.Length - 1];
                int currentStageIndex = (int)Char.GetNumericValue(currentStageIndexChar);

                if (currentStageIndex < 4) {
                    stageWon(a.score, true);
                }else if(currentStageIndex == 4)
                {
                    stageWon(a.score, hit.hit);
                }else
                {
                    bool condition = hitLeft.hit && hitRight.hit;
                    stageWon(a.score, condition);
                }
            }

        }
    }

    void stageWon(int score,bool condition)
    {
        int scoreToWin = 0;
        string holder = PlayerPrefs.GetString("stagesMenuLocation");
        char currentStageIndexChar = holder[holder.Length - 1];
        int currentStageIndex = (int) Char.GetNumericValue(currentStageIndexChar);
        switch (currentStageIndex)
        {
            case 1: scoreToWin = 5;
                break;
            case 2:
                    scoreToWin = 10;
                break;
            case 3:
                scoreToWin = 15;
                break;
            case 4:
                scoreToWin = 1;
                
                break;
            case 5:
                scoreToWin = 1;
                break;
        }

        if(scoreToWin <= score && condition)
        {
            StartCoroutine(nextStage());
        }
    }

    IEnumerator nextStage()
    {

        sfx.GetComponent<sfxManager>().stageWon();
        yield return new WaitForSeconds(0.5f);
        nextstagePannel.SetActive(true);
        pannelShowed = true;
        int check = PlayerPrefs.GetInt("stage") + 1;
        ++check;

        Debug.Log(check +" "+ PlayerPrefs.GetInt("basketballFinished"));
        if (check >= 6)
        {
            if ((PlayerPrefs.GetInt("basketballFinished") == 0))
            {
                Debug.Log("happened");
                PlayerPrefs.SetInt("basketballFinished", 1);
            }
        }
        else
        {
            if (PlayerPrefs.GetString("stagesMenuLocation").Equals("stage " + (check-1)))
            {
                Debug.Log("stage increased");
                PlayerPrefs.SetInt("stage", PlayerPrefs.GetInt("stage") + 1);
                PlayerPrefs.SetString("stagesMenuLocation", "stage " + check);
            }
        }
        Debug.Log(check + " " + PlayerPrefs.GetInt("basketballFinished"));
    }

    public void nextStageYes()
    {

        sfx.GetComponent<sfxManager>().ok();
        a.ShowAd();
        SceneManager.LoadScene("BasketballStagesScene");

    }
    public void nextStageNo()
    {
        sfx.GetComponent<sfxManager>().quit();

        a.ShowAd();
        a.NoFontion();
    }

    public void gotIt()
    {
        sfx.GetComponent<sfxManager>().ok();

        pannelMission.SetActive(false);
        pannelShowed = false;
    }

}
