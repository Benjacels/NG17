using UnityEngine;
using System.Collections;
using System;

public class TankScript : DestroyableObject {

    public GameObject path;
    public GameObject bullet;
    public float speedFraction = 0.01f;
    public float rotationSpeed = 360f;
    public float bulletSpeed = 100f;
    public float shootTime = 2; // secs between shots
    float timeSinceLastShot = 0;

	private bool _tankDisabled = false;

    AudioSource gunShot;
    // should i shoot vars
    Transform playerPosition;
    bool targetWithinRange = false;
    // Path finding vars
    float distCovered = 0;
    Transform[] nodes;
    Vector3 currentPos, nextPos;
    int index = 0;
    Vector3 direction;
	// Use this for initialization
	void Start () {
        PopulateNodes(path.transform.childCount);
        // hacky, just set current pos to element 0 in its path  
        currentPos = nodes[index].position;
        nextPos = nodes[index + 1].position;
        // sæt til currentpos i world
        transform.position = transform.TransformPoint(currentPos);
        // calc the direction, normalize for now
        direction = nextPos - currentPos;
        transform.LookAt(transform.position + new Vector3(0, 0, 1), direction);

        //get audio
        gunShot = GetComponent<AudioSource>();
	}


    // Update is called once per frame
    void Update ()
    {
		if (_tankDisabled)
			return;

        if (!targetWithinRange)
        {
            MoveEnemy();
        }
        else
        {
            //enemy is within range.
            //Shoot projectile towards his posision.
            Shoot();
        }
    }

    private void Shoot()
    {
        if (timeSinceLastShot <= 0)
        {
            Vector3 shootDirection = playerPosition.position - transform.position;
            shootDirection.Normalize();
            GameObject projectile = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            if (projectile != null)
            {
                projectile.GetComponent<Rigidbody>().AddForce(shootDirection * bulletSpeed);
            }
            timeSinceLastShot = shootTime;
            gunShot.Play();
        }
        else
        {
            timeSinceLastShot -= Time.deltaTime; 
        }
    }

    private void MoveEnemy()
    {
        transform.position = (Vector3.Lerp(currentPos, nextPos, distCovered));

        distCovered += speedFraction;
        if (distCovered > 1)
        {
            //Then we are at our pos
            distCovered = 0;
            currentPos = nextPos;
            index = GetNextIndex();
            nextPos = nodes[index].position;
            // recalc direction;
            direction = nextPos - currentPos;

            transform.LookAt(transform.position + new Vector3(0, 0, 1), direction);
        }
    }

    private void PopulateNodes(int childCount)
    {
        nodes = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            nodes[i] = path.transform.GetChild(i);
        }
    }

    int GetNextIndex()
    {
        if (index + 1 < nodes.Length)
        {
            return index + 1;
        }
        else
        {
            return 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            targetWithinRange = true;
            playerPosition = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            targetWithinRange = false;
        }
    }

	public override void DestroyObject()
	{
		base.DestroyObject();
		_tankDisabled = true;
	}

	public override void ReviveObject()
	{
		base.ReviveObject();
		_tankDisabled = false;
	}
}
