using UnityEngine;
using System.Collections;
using System;

public class TankScript : MonoBehaviour {

    public GameObject path;
    public float speedFraction = 0.01f;
    public float rotationSpeed = 360f;

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
	}


    // Update is called once per frame
    void Update () {
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
}
