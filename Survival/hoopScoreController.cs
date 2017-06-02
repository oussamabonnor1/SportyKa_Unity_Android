using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hoopScoreController : MonoBehaviour {
    public BasketballSurvival a;
    public GameObject scoreTxt;
    static GameObject canvas;
    public bool enteredOnce;
    // Use this for initialization
    void Start () {
        enteredOnce = false;
        //canvas fetching
        canvas = GameObject.Find("Canvas");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!a.leftEdge.GetComponent<EdgeCollider2D>().isTrigger && !a.rightEdge.GetComponent<EdgeCollider2D>().isTrigger) {
           
            //creates the score floating text
            GameObject floatingText = (GameObject)Instantiate(scoreTxt, canvas.transform.position, Quaternion.identity);
            if (!enteredOnce)
            {
                if (PlayerPrefs.GetInt("sound") == 1)
                {
                    GetComponent<AudioSource>().Play();
                }
                a.score++;
                enteredOnce = true;
                floatingText.GetComponentInChildren<TxtController>().score = a.score;
                floatingText.transform.SetParent(canvas.transform, false);
                floatingText.transform.position = new Vector3(transform.position.x, transform.position.y - 200, 0);
                a.scoreTxt.GetComponent<Text>().text = "Score: " + a.score;
            }

        }
    }
}
