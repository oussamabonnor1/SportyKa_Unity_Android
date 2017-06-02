using UnityEngine;
using System.Collections;

public class LoserForStages : MonoBehaviour {

    public bounce a;
    GameObject sfx;

    void Start()
    {
        sfx = GameObject.Find("SFX Manager");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player")) {
            a.gameOver = true;
            a.start = false;
            a.lose = true;
            sfx.GetComponent<sfxManager>().fall();
        }else
        {
            Destroy(other.gameObject);
            a.arrowFired = false;
        }
    }
}
