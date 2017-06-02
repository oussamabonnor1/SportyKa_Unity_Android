using UnityEngine;
using System.Collections;

public class FootballGameControllerStages : MonoBehaviour {

	 public GameObject moverClouds;
    public GameObject moverBirds;
    public GameObject moverBoat;
    public GameObject moverPlane;

    bool directionCloud;
    bool directionBoat;
    bool directionBird;
    bool directionPlane;

    bool cloudStarted;
    bool birdStarted;
    bool boatStarted;
    bool planeStarted;

    // Use this for initialization
    void Start()
    {

        AudioSource c = GameObject.Find("Music Manager").GetComponent<AudioSource>();

        
        if (PlayerPrefs.GetInt("sound") == 0)
        {
            c.Pause();
        }
        else if (!c.isPlaying)
        {
            c.Play();
        }


        cloudStarted = false;
        birdStarted = false;
        boatStarted = false;
        planeStarted = false;

        launchCloud();
        launchBird();
        launchBoat();
        launchPlane();
    }

    // Update is called once per frame
    void Update()
    {

        //cloud movement
        if (directionCloud)
        {
            moverClouds.transform.localPosition = new Vector3(moverClouds.transform.localPosition.x + 1, moverClouds.transform.localPosition.y, moverClouds.transform.localPosition.z);
        }
        else
        {
            moverClouds.transform.localPosition = new Vector3(moverClouds.transform.localPosition.x - 1, moverClouds.transform.localPosition.y, moverClouds.transform.localPosition.z);
        }

        if ((moverClouds.transform.localPosition.x >= 250 || moverClouds.transform.localPosition.x <= -1500) && !cloudStarted)
        {
            StartCoroutine(targetReachedCloud());
            cloudStarted = true;
        }

        //birds movement
        if (directionBird)
        {
            moverBirds.transform.localPosition = new Vector3(moverBirds.transform.localPosition.x - 2, moverBirds.transform.localPosition.y, moverBirds.transform.localPosition.z);
        }
        else
        {
            moverBirds.transform.localPosition = new Vector3(moverBirds.transform.localPosition.x + 2, moverBirds.transform.localPosition.y, moverBirds.transform.localPosition.z);
        }

        if ((moverBirds.transform.localPosition.x >= -120 || moverBirds.transform.localPosition.x <= -1200) && !birdStarted)
        {
            StartCoroutine(targetReachedBird());
            birdStarted = true;
        }

        //boat movement
        if (directionBoat)
        {
            moverBoat.transform.localPosition = new Vector3(moverBoat.transform.localPosition.x - 0.5f, moverBoat.transform.localPosition.y, moverBoat.transform.localPosition.z);
        }
        else
        {
            moverBoat.transform.localPosition = new Vector3(moverBoat.transform.localPosition.x + 0.5f, moverBoat.transform.localPosition.y, moverBoat.transform.localPosition.z);
        }

        if ((moverBoat.transform.localPosition.x >= 20 || moverBoat.transform.localPosition.x <= -1300) && !boatStarted)
        {
            StartCoroutine(targetReachedBoat());
            boatStarted = true;
        }

        //plane movement
        if (directionPlane)
        {
            moverPlane.transform.localPosition = new Vector3(moverPlane.transform.localPosition.x - 3, moverPlane.transform.localPosition.y, moverPlane.transform.localPosition.z);
        }
        else
        {
            moverPlane.transform.localPosition = new Vector3(moverPlane.transform.localPosition.x + 3, moverPlane.transform.localPosition.y, moverPlane.transform.localPosition.z);
        }

        if ((moverPlane.transform.localPosition.x >= -20 || moverPlane.transform.localPosition.x <= -1300) && !planeStarted)
        {
            StartCoroutine(targetReachedPlane());
            planeStarted = true;
        }

    }

    void launchCloud()
    {
        directionCloud = Random.value >= 0.5;

        if (directionCloud)
        {
            moverClouds.transform.localPosition = new Vector3(-1180, moverClouds.transform.localPosition.y, moverClouds.transform.localPosition.z);
            moverClouds.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            moverClouds.transform.localPosition = new Vector3(-85, moverClouds.transform.localPosition.y, moverClouds.transform.localPosition.z);
            moverClouds.transform.localRotation = new Quaternion(0, 180, 0, 0);
        }
    }

    IEnumerator targetReachedCloud()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        launchCloud();
        cloudStarted = false;
    }

    void launchBoat()
    {
        directionBoat = Random.value >= 0.5;

        if (directionBoat)
        {
            moverBoat.transform.localPosition = new Vector3(17, moverBoat.transform.localPosition.y, moverBoat.transform.localPosition.z);
            moverBoat.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            moverBoat.transform.localPosition = new Vector3(-1270, moverBoat.transform.localPosition.y, moverBoat.transform.localPosition.z);
            moverBoat.transform.localRotation = new Quaternion(0, 180, 0, 0);
        }
    }

    IEnumerator targetReachedBoat()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        launchBoat();
        boatStarted = false;
    }


    void launchBird()
    {
        directionBird = Random.value >= 0.5;

        if (directionBird)
        {
            moverBirds.transform.localPosition = new Vector3(-130, moverBirds.transform.localPosition.y, moverBirds.transform.localPosition.z);
            moverBirds.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            moverBirds.transform.localPosition = new Vector3(-1140, moverBirds.transform.localPosition.y, moverBirds.transform.localPosition.z);
            moverBirds.transform.localRotation = new Quaternion(0, 180, 0, 0);
        }
    }

    IEnumerator targetReachedBird()
    {
        yield return new WaitForSeconds(Random.Range(2f, 4f));
        launchBird();
        birdStarted = false;
    }


    void launchPlane()
    {
        directionPlane = Random.value >= 0.5;

        if (directionPlane)
        {
            moverPlane.transform.localPosition = new Vector3(-27, moverPlane.transform.localPosition.y, moverPlane.transform.localPosition.z);
            moverPlane.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            moverPlane.transform.localPosition = new Vector3(-1250, moverPlane.transform.localPosition.y, moverPlane.transform.localPosition.z);
            moverPlane.transform.localRotation = new Quaternion(0, 180, 0, 0);
        }
    }

    IEnumerator targetReachedPlane()
    {
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        launchPlane();
        planeStarted = false;
    }


}


