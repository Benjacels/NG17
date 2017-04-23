using UnityEngine;
using System.Collections;

public class ObjectDestroyer : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<DestroyableObject>() != null)
		{
			if (Vector3.Distance(transform.position, other.transform.position) < 0.7f)
				other.GetComponent<DestroyableObject>().DestroyObject();
		}

		if (gameObject.CompareTag("Bullet") && other.gameObject.CompareTag("Building"))
			Destroy(gameObject);
	}
}
