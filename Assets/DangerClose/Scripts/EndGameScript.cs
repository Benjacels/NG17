using UnityEngine;
using System.Collections;

public class EndGameScript : MonoBehaviour {

	public void EndGame()
	{
		GameObject.FindGameObjectWithTag("EndGameParent").transform.GetChild(0).gameObject.SetActive(true);
		TankScript[] tankScripts = FindObjectsOfType(typeof(TankScript)) as TankScript[];

		foreach (TankScript ts in tankScripts)
			ts.DestroyObject();

		if (FindObjectOfType<Commando>() != null)
			FindObjectOfType<Commando>().GetComponent<SphereCollider>().enabled = false;
	}
}
