using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class OpeningSceneController : MonoBehaviour {
    public Animator anime1;
    public Animator anime2;
    public GameObject blackImage;
    public GameObject title;
	// Use this for initialization
	void Start () {
        anime1.Play(0);
        anime2.Play(0);
        StartCoroutine(animate());
    
    }
    
    // Update is called once per frame
    void Update () {

       
	}

    IEnumerator animate()
    {
        

        yield return new WaitForSeconds((float)anime1.GetCurrentAnimatorClipInfo(0).GetLength(0) * 2);
        anime1.Stop();
        anime2.Stop();

        for (int i = 1; i <= 255; i += 5)
        {
            yield return new WaitForSeconds(0.0001f);
            title.GetComponent<Image>().color = new Color32(255,255,255, (byte)i);
        }


        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i <= 255; i+=5)
        {
            yield return new WaitForSeconds(0.0001f);
            blackImage.GetComponent<Image>().color = new Color32(0,0,0,(byte) i);
        }
        
        PlayerPrefs.SetString("PreviousScene", "Opening");
            SceneManager.LoadScene("MenuScene");
        
        yield return new WaitForSeconds(0.45f);
        }
}
