using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float timeToLive = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Timer();
    }

    private void Timer()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive < 0)
        {
            Destroy(gameObject);
        }
    }
}
