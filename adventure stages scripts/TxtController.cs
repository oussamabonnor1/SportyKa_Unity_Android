using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TxtController : MonoBehaviour {
    public Animator animator;
    public bool first;
    string[] welcomePhrases = { "Hello !", "I'm up !", "Hi !", "Let's go !","Go !"};
    public int score;

    // Use this for initialization
    void Start () {
        if (first)
        {
            GetComponent<Text>().text = welcomePhrases[Random.Range(0,welcomePhrases.Length)];
            first = false;
        }
        else
        {
            GetComponent<Text>().text = "" + score;
        }
        AnimatorClipInfo[] clipinfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject.transform.parent.gameObject, clipinfo[0].clip.length);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
