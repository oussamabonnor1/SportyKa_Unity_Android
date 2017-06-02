using UnityEngine;
using System.Collections;

public class FootballMusicManage : MonoBehaviour {

	
    public static FootballMusicManage Instance;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

