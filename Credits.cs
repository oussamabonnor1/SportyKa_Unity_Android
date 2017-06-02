using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;



public class Credits : MonoBehaviour {
    public GameObject scrollBar;
    GameObject sfx;

    
	// Use this for initialization
	void Start () {
        scrollBar.GetComponent<Scrollbar>().value = 1;
        sfx = GameObject.Find("SFX Manager");
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exit();
        }
    }

    public void facebook()
    {
        Application.OpenURL("https://www.facebook.com/JetLightstudio");
    }

    public void googlePlus()
    {
        Application.OpenURL("https://plus.google.com/u/0/106078600308560786022");
    }

    public void officielSite()
    {
        Application.OpenURL("https://www.jetlightstudio.ga");
    }

    public void exit()
    {
        sfx.GetComponent<sfxManager>().quit();
        PlayerPrefs.SetString("PreviousScene", "Credits");
        SceneManager.LoadScene("MenuScene");
    }
}
