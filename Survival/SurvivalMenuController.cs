using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SurvivalMenuController : MonoBehaviour {
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exit();
        }
    }

    public void basketball()
    {
        SceneManager.LoadScene("BasketballScene");
    }

    public void football()
    {
        SceneManager.LoadScene("FootballScene");
    }

    public void baseball()
    {
        SceneManager.LoadScene("BaseballScene");
    }

    public void tennis()
    {
        SceneManager.LoadScene("TennisScene");
    }

    public void exit()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
