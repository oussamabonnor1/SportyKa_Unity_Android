using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using System.Collections;

public class RewardedAds : MonoBehaviour {
    public GameObject soundSprite;
    public GameObject adSorry;
    public GameObject starText;
    public GameObject heartText;
    public GameObject noStarsPannel;
    public GameObject starsPannelOne;
    public GameObject starsPannelTwo;
    public GameObject goldPannelpack;
    public GameObject rewardPannel;
    public GameObject rewardText;
    public GameObject rewardButton;
    GameObject sfx;

    AudioSource c;


    void Start()
    {
        c = GameObject.Find("Music Manager").GetComponent<AudioSource>();
        sfx = GameObject.Find("SFX Manager");


        if (PlayerPrefs.GetInt("sound") == 1)
        {
            soundSprite.GetComponent<Image>().enabled = false;
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
            soundSprite.GetComponent<Image>().enabled = true;
            c.Pause();
        }

    
        starText.GetComponent<Text>().text = "" + PlayerPrefs.GetInt("stars");
        heartText.GetComponent<Text>().text = "" + PlayerPrefs.GetInt("hearts");
        
        if (PlayerPrefs.GetInt("rewarded") == 0 && checkPackageAppIsPresent("com.Oussama.BallFall"))
        {
            PlayerPrefs.SetInt("rewarded", 1);
            sfx.GetComponent<sfxManager>().appear();
            rewardPannelTextSetting("You downloaded our app:\nYou earned 10 sportyHearts !", "Collect !", 10, -250);
        }
    }

    private bool checkPackageAppIsPresent(string package)
    {
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject appList = packageManager.Call<AndroidJavaObject>("getInstalledPackages", 0);
        int num = appList.Call<int>("size");
        for (int i = 0; i < num; i++)
        {
            AndroidJavaObject appInfo = appList.Call<AndroidJavaObject>("get", i);
            string packageNew = appInfo.Get<string>("packageName");

            if (packageNew.CompareTo(package) == 0)
            {
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sfx.GetComponent<sfxManager>().quit();
            SceneManager.LoadScene("MenuScene");
        }
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
        else if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            sfx.GetComponent<sfxManager>().error();
            rewardPannelTextSetting("Check your internet connection !", "Okay", 0, 0);
        }
        else
        {
            sfx.GetComponent<sfxManager>().error();
            adSorry.SetActive(true);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                sfx.GetComponent<sfxManager>().appear();
                rewardPannelTextSetting("You finished an Ad video:\nYou earned 1 sportyHeart !", "Collect !", 1, 0);
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                adSorry.SetActive(true);
                break;
        }
    }

    public void exit()
    {
        sfx.GetComponent<sfxManager>().quit();
        PlayerPrefs.SetString("PreviousScene", "ShopScene");
        SceneManager.LoadScene("MenuScene");
    }

    public void hide()
    {
        sfx.GetComponent<sfxManager>().quit();
        adSorry.SetActive(false);
        starsPannelOne.SetActive(false);
        noStarsPannel.SetActive(false);
        starsPannelTwo.SetActive(false);
        goldPannelpack.SetActive(false);
    }

    public void sound()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            Debug.Log("off");
            soundSprite.GetComponent<Image>().enabled = true;
            PlayerPrefs.SetInt("sound", 0);
            AudioSource c = GameObject.Find("Music Manager").GetComponent<AudioSource>();
            c.Pause();

        }
        else
        {
            Debug.Log("On");
            soundSprite.GetComponent<Image>().enabled = false;
            PlayerPrefs.SetInt("sound", 1);
            if (c.isPlaying)
            {
                c.UnPause();
            }
            else
            {
                c.Play();
            }
        }
    }

    public void packOne()
    {
        if(PlayerPrefs.GetInt("stars")< 100)
        {
            sfx.GetComponent<sfxManager>().error();
            noStarsPannel.SetActive(true);
        }else
        {
            sfx.GetComponent<sfxManager>().appear();
            starsPannelOne.SetActive(true);
        }
    }


    public void packTwo()
    {
        if (PlayerPrefs.GetInt("stars") < 250)
        {
            sfx.GetComponent<sfxManager>().error();
            noStarsPannel.SetActive(true);
        }
        else
        {
            sfx.GetComponent<sfxManager>().appear();
            starsPannelTwo.SetActive(true);
        }
    }

    public void packTwoYes()
    {
        sfx.GetComponent<sfxManager>().ok();

        rewardPannelTextSetting("You purchased Pack two:\nYou earned 3 sportyHearts !", "Collect !", 3, 250);
        
        hide();
    }

    public void packOneYes()
    {

        sfx.GetComponent<sfxManager>().ok();
        rewardPannelTextSetting("You purchased Pack one:\nYou earned 1 sportyHearts !", "Collect !", 1, 100);
        
        hide();
    }

    public void packGold()
    {
        sfx.GetComponent<sfxManager>().appear();
        goldPannelpack.SetActive(true);
    }
    

    public void packGoldYes()
    {

        
        hide();
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            sfx.GetComponent<sfxManager>().error();

            rewardPannelTextSetting("Check your internet connection !","Okay", 0, 0);
        }
        else { 
            if (PlayerPrefs.GetInt("rewarded") == 0)
            {
                sfx.GetComponent<sfxManager>().ok();

                Application.OpenURL("https://play.google.com/store/apps/details?id=com.Oussama.BallFall");
            }
            else
             {
                sfx.GetComponent<sfxManager>().error();
                rewardPannelTextSetting("You already collected this sportyBonus !","Okay", 0, 0);
            }
        }
    }

    void rewardPannelTextSetting(string reward, string button, int hearts, int stars)
    {
        rewardPannel.SetActive(true);
        rewardText.GetComponent<Text>().text = reward;
        rewardButton.GetComponent<Text>().text = button;

        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") - stars);
        PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") + hearts);

    }

    public void collect()
    {
        sfx.GetComponent<sfxManager>().ok();
        rewardPannel.SetActive(false);
        starText.GetComponent<Text>().text = "" + PlayerPrefs.GetInt("stars");
        heartText.GetComponent<Text>().text = "" + PlayerPrefs.GetInt("hearts");
    }
}
