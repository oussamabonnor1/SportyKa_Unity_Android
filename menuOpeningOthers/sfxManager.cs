using UnityEngine;
using System.Collections;

public class sfxManager : MonoBehaviour {

    public AudioClip[] sounds;
    public AudioSource b;

    public static sfxManager Instance;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ok()
    {
        if(PlayerPrefs.GetInt("sound")== 1)
        {
            b.clip = sounds[0];
            b.Play();
        }
    }

    public void quit()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            b.clip = sounds[1];
            b.Play();
        }
    }

    public void appear()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            b.clip = sounds[2];
            b.Play();
        }
    }

    public void error()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            b.clip = sounds[3];
            b.Play();
        }
    }

    public void slide()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            b.clip = sounds[4];
            b.Play();
        }
    }

    public void arrowShot()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            b.clip = sounds[5];
            b.Play();
        }
    }

    public void stageWon()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            b.clip = sounds[6];
            b.Play();
        }
    }

    public void fall()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            b.clip = sounds[7];
            b.Play();
        }
    }

    public void kill()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            b.clip = sounds[8];
            b.Play();
        }
    }

    void Awake()
    {

        if (Instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }
}

