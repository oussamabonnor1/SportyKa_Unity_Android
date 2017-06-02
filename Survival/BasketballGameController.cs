using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BasketballGameController : MonoBehaviour {


    public Animator animator;
    static GameObject canvas;
    AnimatorClipInfo[] clip;


    // Use this for initialization
    void Start () {

        AudioSource c = GameObject.Find("Music Manager").GetComponent<AudioSource>();
        
            if (c.isPlaying)
            {
                c.Pause();
            }

        c = GameObject.Find("Basketball Music Manager").GetComponent<AudioSource>();

        if (PlayerPrefs.GetInt("sound") == 0)
        {
            c.Pause();
        }else if(!c.isPlaying)
        {
            c.Play();
        }

        canvas = GameObject.Find("Canvas");
        GameObject animatedText = (GameObject)Instantiate(animator.gameObject, canvas.transform.position, Quaternion.identity);
        animatedText.transform.position = new Vector3(animatedText.transform.position.x, animatedText.transform.position.y - 500, animatedText.transform.position.z);
        if (SceneManager.GetActiveScene().name.Equals("BasketballScene"))
        {
            animatedText.GetComponent<Text>().text = "Swip up to shoot !";
        }
        else if (SceneManager.GetActiveScene().name.Equals("BasketballStagePlayScene"))
        {
            animatedText.GetComponent<Text>().text = "Score 10 or more !";
        }
        animatedText.transform.SetParent(canvas.transform, false);
        clip = animatedText.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        Destroy(animatedText, clip[0].clip.length - 0.1f);

    }

    // Update is called once per frame
    void Update () {
	
	}
}
