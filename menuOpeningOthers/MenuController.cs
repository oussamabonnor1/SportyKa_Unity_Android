using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class MenuController : MonoBehaviour {

    public GameObject SoundBar;
    public GameObject blackImage;
    public GameObject menuImage;
    public Animator menuSlide;
    public GameObject rateUsPannel;
    public GameObject quitPannel;
    public GameObject timePannel;
    public GameObject leaderBoardPannel;
    GameObject sfx;

    bool rateShow;
    bool quit;

    AudioSource c;

    // Use this for initialization
    void Start () {
        
        sfx = GameObject.Find("SFX Manager");

        c = GameObject.Find("Music Manager").GetComponent<AudioSource>();

        rateShow = false;
        quit = false;

        int day = System.DateTime.Now.Date.DayOfYear;

        PlayerPrefs.SetInt("rate", PlayerPrefs.GetInt("rate") + 1);

        if (PlayerPrefs.GetInt("firstTimer") == 0)
        {
            PlayerPrefs.SetInt("sound", 1);
            PlayerPrefs.SetInt("date", System.DateTime.Now.DayOfYear);
            PlayerPrefs.SetInt("firstTimer", 1);
            PlayerPrefs.SetInt("hearts", 1);
        }else if(day.CompareTo(PlayerPrefs.GetInt("date")) == 1)
        {
            PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + 50);
            timePannel.SetActive(true);
            sfx.GetComponent<sfxManager>().appear();
            PlayerPrefs.SetInt("date", System.DateTime.Now.DayOfYear);
        }
        else if(day.CompareTo(PlayerPrefs.GetInt("date")) > 1)
        {
            PlayerPrefs.SetInt("date", System.DateTime.Now.DayOfYear);
        }

        blackImage.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        if (PlayerPrefs.GetString("PreviousScene").Equals("Opening"))
        {
            menuSlide.Play(0);
            StartCoroutine(slide());
            StartCoroutine(fading());
        }
        else
        {
            Destroy(blackImage);
            menuSlide.Stop();
        }

        if(PlayerPrefs.GetInt("rate") <100 && PlayerPrefs.GetInt("rate") % 5 == 0)
        {
            sfx.GetComponent<sfxManager>().appear();
            ratePannel();
            rateShow = true;
        }

        if (PlayerPrefs.GetInt("sound") == 1)
        {
            SoundBar.GetComponent<Image>().enabled = false;
            if (c.isPlaying)
            {
                c.UnPause();
            }
            else
            {
                c.Play();
            }
        }
        else
        {
            SoundBar.GetComponent<Image>().enabled = true;
            c.Pause();
        }

        }

        IEnumerator fading()
    {
        for (int i = 255; i >= 0 ; i -= 5)
        {
            yield return new WaitForSeconds(0.001f);
            blackImage.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)i);
        }
        Destroy(blackImage);
    }

    IEnumerator slide()
    {
        yield return new WaitForSeconds(menuSlide.GetCurrentAnimatorClipInfo(0).Length -0.1f);
        menuSlide.Stop();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (rateShow)
            {
                sfx.GetComponent<sfxManager>().quit();
                RateMaybe();
            }
            else if (quit)
            {
                sfx.GetComponent<sfxManager>().ok();
                noQuit();
            }
            else
            {
                sfx.GetComponent<sfxManager>().appear();
                quitPannel.SetActive(true);
                quit = true;
            }
            
        }
    }

    public void leaderBoard()
    {
        if (leaderBoardPannel.activeSelf)
        {
            leaderBoardPannel.SetActive(false);
            sfx.GetComponent<sfxManager>().quit();
        }
        else
        {
            leaderBoardPannel.SetActive(true);
            sfx.GetComponent<sfxManager>().appear();
        }
    }
    public void timeCollect()
    {
        sfx.GetComponent<sfxManager>().ok();
        timePannel.SetActive(false);
    }

    public void survival()
    {
        sfx.GetComponent<sfxManager>().ok();
        PlayerPrefs.SetString("PreviousScene", "Survival");
        SceneManager.LoadScene("AdventureScene");
    }

    public void adventure()
    {
        sfx.GetComponent<sfxManager>().ok();
        PlayerPrefs.SetString("PreviousScene", "Adventure");
        SceneManager.LoadScene("AdventureScene");
    }

    public void shop()
    {
        sfx.GetComponent<sfxManager>().ok();
        PlayerPrefs.SetString("PreviousScene", "Shop");
        SceneManager.LoadScene("ShopScene");
    }

    public void exitButtonClicked()
    {
        sfx.GetComponent<sfxManager>().appear();
        quitPannel.SetActive(true);
        quit = true;
    }

    public void exit()
    {
        sfx.GetComponent<sfxManager>().quit();
        Application.runInBackground = false;
        Application.Quit();
    }

    public void noQuit()
    {
        sfx.GetComponent<sfxManager>().ok();
        quitPannel.SetActive(false);
        quit = false;
    }

    public void sound()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            Debug.Log("off");
            SoundBar.GetComponent<Image>().enabled = true;
            PlayerPrefs.SetInt("sound", 0);
            AudioSource c = GameObject.Find("Music Manager").GetComponent<AudioSource>();
            c.Pause();
            
        }else
        {
            Debug.Log("On");
            SoundBar.GetComponent<Image>().enabled = false;
            PlayerPrefs.SetInt("sound", 1);
            if (c.isPlaying)
            {
                c.UnPause();
            }else
            {
                c.Play();
            }
        }
    }
    
    public void credit()
    {
        sfx.GetComponent<sfxManager>().ok();
        SceneManager.LoadScene("CreditScene");
    }

    void ratePannel()
    {
        sfx.GetComponent<sfxManager>().appear();
        rateUsPannel.SetActive(true);
    }

    public void RateYes()
    {
        sfx.GetComponent<sfxManager>().ok();
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.JetLightstudio.SportyKa");
        RateNo();
    }

    public void RateMaybe()
    {
        sfx.GetComponent<sfxManager>().quit();
        rateUsPannel.SetActive(false);
    }

    public void RateNo()
    {
        sfx.GetComponent<sfxManager>().quit();
        rateUsPannel.SetActive(false);
        PlayerPrefs.SetInt("rate", 100);
    }
}
