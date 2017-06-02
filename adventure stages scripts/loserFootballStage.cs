using UnityEngine;
using System.Collections;

public class loserFootballStage : MonoBehaviour {

    public BounceForStage a;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player") && !a.pannelShowed)
        {
            a.gameOver = true;
            a.start = false;
            a.lose = true;
        }
        if(!other.gameObject.tag.Equals("Player"))
        {
            Destroy(other.gameObject);
        }
    }
}
