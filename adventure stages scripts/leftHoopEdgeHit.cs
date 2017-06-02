using UnityEngine;
using System.Collections;

public class leftHoopEdgeHit : MonoBehaviour {

    public bool hit;
    // Use this for initialization
    void Start()
    {
        hit = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        hit = true;
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
