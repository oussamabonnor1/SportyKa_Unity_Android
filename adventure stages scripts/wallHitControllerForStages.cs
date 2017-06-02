using UnityEngine;
using System.Collections;

public class wallHitControllerForStages : MonoBehaviour
{
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
    }
}
