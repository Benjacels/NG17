using UnityEngine;
using System.Collections;

public class ObjectDestroyer : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<DestroyableObject>() != null)
			other.GetComponent<DestroyableObject>().DestroyObject();
	}
}
