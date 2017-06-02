using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class loser : MonoBehaviour {
    public BasketballSurvival a;
    public hoopScoreController b;
    public GameObject lifes;

    public Sprite[] balls;

    public int currentLife;
    int score;

    void Start()
    {
        score = a.score;
        currentLife = 3; 
        lifes.GetComponent<Image>().sprite = balls[2];
    }

	void OnTriggerEnter2D(Collider2D other)
    {
        if (!a.lose){
            a.start = false;
            b.enteredOnce = false;
            a.lose = true;

            if (currentLife > 0 && score == a.score)
            {
                if (PlayerPrefs.GetInt("sound") == 1)
                {
                    GetComponent<AudioSource>().Play();
                }
                --currentLife;

                switch (currentLife)
                {
                    case 1:
                        lifes.GetComponent<Image>().sprite = balls[0];
                        break;
                    case 2:
                        lifes.GetComponent<Image>().sprite = balls[1];
                        break;
                }

                if (currentLife == 0)
                {
                    lifes.gameObject.SetActive(false);
                    a.gameOver = true;
                }
            }

            score = a.score;
        }
    }
}
