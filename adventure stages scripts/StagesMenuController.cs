using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StagesMenuController : MonoBehaviour
{
    public GameObject menuHolder;
    public GameObject starsText;
    public GameObject heartsText;
    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject[] stagesPictures;
    public Sprite[] sprites;
    public GameObject basketstagesFinished;
    public GameObject footstagesFinished;
    GameObject sfx;

    // Use this for initialization
    void Start()
    {

        sfx = GameObject.Find("SFX Manager");

        if (PlayerPrefs.GetInt("basketballFinished") == 1)
        {
            PlayerPrefs.SetInt("basketballFinished", 2);
            basketStagesFinished();
        }

        if (PlayerPrefs.GetInt("footballFinished") == 1)
        {
            PlayerPrefs.SetInt("footballFinished", 2);
            footStagesFinished();
        }

        stagesChoice();

        heartsText.GetComponent<Text>().text = "" + PlayerPrefs.GetInt("hearts");
        starsText.GetComponent<Text>().text = "" + PlayerPrefs.GetInt("stars");


        int val = 0;

        if (PlayerPrefs.GetString("PreviousScene").Equals("basketball")) {

            switch (PlayerPrefs.GetString("stagesMenuLocation"))
            {
                case "stage 1":
                    val = 0;
                    break;
                case "stage 2":
                    val = -800;
                    break;
                case "stage 3":
                    val = -1600;
                    break;
                case "stage 4":
                    val = -2400;
                    break;
                case "stage 5":
                    val = -3200;
                    break;
                default:
                    val = 0;
                    PlayerPrefs.SetString("stagesMenuLocation", "stage 1");
                    break;
            }
        }else if(PlayerPrefs.GetString("PreviousScene").Equals("football"))
        {
            switch (PlayerPrefs.GetString("footballStagesMenuLocation"))
            {
                case "stage 1":
                    val = 0;
                    break;
                case "stage 2":
                    val = -800;
                    break;
                case "stage 3":
                    val = -1600;
                    break;
                case "stage 4":
                    val = -2400;
                    break;
                case "stage 5":
                    val = -3200;
                    break;
                default:
                    val = 0;
                    PlayerPrefs.SetString("footballStagesMenuLocation", "stage 1");
                    break;
            }
        }
        

        menuHolder.transform.localPosition = new Vector3(val, menuHolder.gameObject.transform.localPosition.y, menuHolder.gameObject.transform.localPosition.z);
        leftButton.SetActive(true);
        rightButton.SetActive(true);

    }

    void stagesChoice()
    {
        if (PlayerPrefs.GetString("PreviousScene").Equals("basketball"))
        {

            for (int i = 0; i <= PlayerPrefs.GetInt("stage"); i++)
            {
                stagesPictures[i].GetComponent<Image>().sprite = sprites[1];
            }

            for (int i = PlayerPrefs.GetInt("stage") + 1; i < 5; i++)
            {
                stagesPictures[i].GetComponent<Image>().sprite = sprites[0];
            }
        }
        else if (PlayerPrefs.GetString("PreviousScene").Equals("football"))
        {
            for (int i = 0; i <= PlayerPrefs.GetInt("footballStage"); i++)
            {
                stagesPictures[i].GetComponent<Image>().sprite = sprites[1];
            }

            for (int i = PlayerPrefs.GetInt("footballStage") + 1; i < 5; i++)
            {
                stagesPictures[i].GetComponent<Image>().sprite = sprites[0];
            }
            
       }

     }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exit();
        }
    }

    public void keyright()
    {
        if (menuHolder.transform.localPosition.x > -3200)
        {
            sfx.GetComponent<sfxManager>().slide();
            StartCoroutine(swip(-50));
        }else
        {
            sfx.GetComponent<sfxManager>().error();
        }
    }
    public void keyleft()
    {
        if (menuHolder.transform.localPosition.x < 0)
        {
            sfx.GetComponent<sfxManager>().slide();
            StartCoroutine(swip(50));
        }else
        {
            sfx.GetComponent<sfxManager>().error();
        }
    }

    public IEnumerator swip(int j)
    {
        leftButton.SetActive(false);
        rightButton.SetActive(false);

        for (int i = 0; i < 16; i++)
        {
            yield return new WaitForSeconds(0.0001f);
            menuHolder.gameObject.transform.localPosition = new Vector3(menuHolder.gameObject.transform.localPosition.x + j, menuHolder.gameObject.transform.localPosition.y, menuHolder.gameObject.transform.localPosition.z);
        }
        float val = menuHolder.gameObject.transform.localPosition.x;

        //im sorry for this :(
        if (val == 0)
        {
            if (PlayerPrefs.GetString("PreviousScene").Equals("basketball"))
            {
                PlayerPrefs.SetString("stagesMenuLocation", "stage 1");
            }else if (PlayerPrefs.GetString("PreviousScene").Equals("football"))
            {
                PlayerPrefs.SetString("footballStagesMenuLocation", "stage 1");
            }

        }
        if (Mathf.Approximately(val, -800))
        {
            if (PlayerPrefs.GetString("PreviousScene").Equals("basketball"))
            {
                PlayerPrefs.SetString("stagesMenuLocation", "stage 2");
            }
            else if (PlayerPrefs.GetString("PreviousScene").Equals("football"))
            {
                PlayerPrefs.SetString("footballStagesMenuLocation", "stage 2");
            }
        }
        if (Mathf.Approximately(val, -1600))
        {
            if (PlayerPrefs.GetString("PreviousScene").Equals("basketball"))
            {
                PlayerPrefs.SetString("stagesMenuLocation", "stage 3");
            }
            else if (PlayerPrefs.GetString("PreviousScene").Equals("football"))
            {
                PlayerPrefs.SetString("footballStagesMenuLocation", "stage 3");
            }
        }
        if (Mathf.Approximately(val, -2400))
        {
            if (PlayerPrefs.GetString("PreviousScene").Equals("basketball"))
            {
                PlayerPrefs.SetString("stagesMenuLocation", "stage 4");
            }
            else if (PlayerPrefs.GetString("PreviousScene").Equals("football"))
            {
                PlayerPrefs.SetString("footballStagesMenuLocation", "stage 4");
            }
        }
        if (Mathf.Approximately(val, -3200))
        {
            if (PlayerPrefs.GetString("PreviousScene").Equals("basketball"))
            {
                PlayerPrefs.SetString("stagesMenuLocation", "stage 5");
            }
            else if (PlayerPrefs.GetString("PreviousScene").Equals("football"))
            {
                PlayerPrefs.SetString("footballStagesMenuLocation", "stage 5");
            }
        }

        leftButton.SetActive(true);
        rightButton.SetActive(true);
    }

    public void play()
    {
        float val = menuHolder.gameObject.transform.localPosition.x;

        if (PlayerPrefs.GetString("PreviousScene").Equals("basketball"))
        {
            if (val > PlayerPrefs.GetInt("stage") * -800 || Mathf.Approximately(val, PlayerPrefs.GetInt("stage") * -800))
            {
                sfx.GetComponent<sfxManager>().ok();
                SceneManager.LoadScene("BasketballStagePlayScene");

            }
            else
            {
                sfx.GetComponent<sfxManager>().error();
            }
        }else if (PlayerPrefs.GetString("PreviousScene").Equals("football"))
        {
            if (val > PlayerPrefs.GetInt("footballStage") * -800 || Mathf.Approximately(val, PlayerPrefs.GetInt("footballStage") * -800))
            {
                sfx.GetComponent<sfxManager>().ok();
                SceneManager.LoadScene("FootballStagePlayScene");
            }else
            {
                sfx.GetComponent<sfxManager>().error();
            }
        }
    }

   
    public void exit()
    {
        sfx.GetComponent<sfxManager>().quit();
        SceneManager.LoadScene("AdventureScene");
    }

    public void shop()
    {
        sfx.GetComponent<sfxManager>().appear();
        SceneManager.LoadScene("ShopScene");
    }

    void basketStagesFinished()
    {
        sfx.GetComponent<sfxManager>().stageWon();
        basketstagesFinished.SetActive(true);
        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + 200);
    }

    void footStagesFinished()
    {
        sfx.GetComponent<sfxManager>().stageWon();
        footstagesFinished.SetActive(true);
        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + 200);
    }

    public void Collect()
    {
        sfx.GetComponent<sfxManager>().ok();
        starsText.GetComponent<Text>().text = "" + PlayerPrefs.GetInt("stars");
        basketstagesFinished.SetActive(false);
        footstagesFinished.SetActive(false);
    }

}
