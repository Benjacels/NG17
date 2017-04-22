using UnityEngine;
using System.Collections;

public class ResetGame : MonoBehaviour {

	public void TriggerResetGame()
	{
		DestroyableObject[] destroyableObjects = (DestroyableObject[])FindObjectsOfType(typeof(DestroyableObject));

		foreach (DestroyableObject destroyObject in destroyableObjects)
			destroyObject.ReviveObject();

		FindObjectOfType<Commando>().HasTriggerObject = false;
		FindObjectOfType<Player>().CmdResetAmmo();
	}
}
