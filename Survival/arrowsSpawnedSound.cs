using UnityEngine;
using System.Collections;

public class arrowsSpawnedSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    void Awake()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            GetComponent<AudioSource>().Play();
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
