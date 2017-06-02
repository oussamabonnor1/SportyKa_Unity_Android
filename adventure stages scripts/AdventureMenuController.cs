using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class AdventureMenuController : MonoBehaviour {
    public GameObject menuHolder;
    public GameObject starsText;
    public GameObject heartsText;
    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject bestScore;
    public GameObject playButton;
    GameObject sfx;

    AudioSource c;

    // Use this for initialization
    void Start () {
        heartsText.GetComponent<Text>().text = "" + PlayerPrefs.GetInt("hearts");
        starsText.GetComponent<Text>().text = "" + PlayerPrefs.GetInt("stars");

        sfx = GameObject.Find("SFX Manager");
        
        if (PlayerPrefs.GetString("PreviousScene").Equals("basketballSurvival"))
        {
            c = GameObject.Find("Basketball Music Manager").GetComponent<AudioSource>();
            c.Pause();
        }

        if (PlayerPrefs.GetString("PreviousScene").Equals("footballSurvival"))
        {
            c = GameObject.Find("Football Music Manager").GetComponent<AudioSource>();
            c.Pause();
        }

        if (PlayerPrefs.GetInt("sound") == 1)
        {
            c = GameObject.Find("Music Manager").GetComponent<AudioSource>();
            if (c.isPlaying)
            {
                c.UnPause();
            }
            else
            {
                c.Play();
            }
        }

        int val;
        string text;

        switch (PlayerPrefs.GetString("adventureMenuLocation"))
        {
            case "football":
                val = 0;
                text = ""+ PlayerPrefs.GetInt("bestScoreFootball");
                break;
            case "basketball":
                val = -800;
                text = "" + PlayerPrefs.GetInt("bestScoreBasketball");
                break;
            default:
                val = 0;
                text = "" + PlayerPrefs.GetInt("bestScoreFootball");
                PlayerPrefs.SetString("adventureMenuLocation","football");
                break;
        }

        bestScore.GetComponentInChildren<Text>().text = text;

        menuHolder.transform.localPosition = new Vector3(val, menuHolder.gameObject.transform.localPosition.y, menuHolder.gameObject.transform.localPosition.z);
        leftButton.SetActive(true);
        rightButton.SetActive(true);

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exit();
        }
	}

    public void keyright()
    {
        if (menuHolder.transform.localPosition.x > -800)
        {
            sfx.GetComponent<sfxManager>().slide();
            StartCoroutine(swip(-50));
        }
    }
    public void keyleft()
    {
        if (menuHolder.transform.localPosition.x < 0)
        {
            sfx.GetComponent<sfxManager>().slide();
            StartCoroutine(swip(50));
        }
    }

    public IEnumerator swip(int j)
    {
        leftButton.SetActive(false);
        rightButton.SetActive(false);

        for (int i=0;i<16 ; i++)
        {
            yield return new WaitForSeconds(0.0001f);
            menuHolder.gameObject.transform.localPosition = new Vector3(menuHolder.gameObject.transform.localPosition.x+j, menuHolder.gameObject.transform.localPosition.y, menuHolder.gameObject.transform.localPosition.z);
        }
        float val = menuHolder.gameObject.transform.localPosition.x;
        if (Mathf.Approximately(val,0))
        {
            bestScore.GetComponentInChildren<Text>().text = "" + PlayerPrefs.GetInt("bestScoreFootball");
            PlayerPrefs.SetString("adventureMenuLocation", "football");
        }
        if (Mathf.Approximately(val, -800))
        {
            bestScore.GetComponentInChildren<Text>().text = "" + PlayerPrefs.GetInt("bestScoreBasketball");
            PlayerPrefs.SetString("adventureMenuLocation", "basketball");
        }
       
        leftButton.SetActive(true);
        rightButton.SetActive(true);
    }

    public void play()
    {

        sfx.GetComponent<sfxManager>().ok();
        float val = menuHolder.gameObject.transform.localPosition.x;
        if (val == 0)
        {
            football();
        }
        if (Mathf.Approximately(val,-800))
        {
            basketball();
        }
        
    }

    public void basketball()
    {
        if (PlayerPrefs.GetString("PreviousScene").Equals("Survival"))
             {

            PlayerPrefs.SetString("PreviousScene", "basketballSurvival");
            SceneManager.LoadScene("BasketballScene");
             }
        else 
        {
            PlayerPrefs.SetString("PreviousScene", "basketball");
            SceneManager.LoadScene("BasketballStagesScene");
             }
    }

    public void football()
    {

        if (PlayerPrefs.GetString("PreviousScene").Equals("Survival"))
        {

            PlayerPrefs.SetString("PreviousScene", "footballSurvival");
            SceneManager.LoadScene("FootballScene");
        }
        else
        {
            PlayerPrefs.SetString("PreviousScene", "football");
            SceneManager.LoadScene("FootballStagesScene");
        }
    }

    public void exit()
    {
        sfx.GetComponent<sfxManager>().quit();
        SceneManager.LoadScene("MenuScene");
    }

    public void shop()
    {
        sfx.GetComponent<sfxManager>().appear();
        SceneManager.LoadScene("ShopScene");
    }

}
