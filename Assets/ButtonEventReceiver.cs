using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonEventReceiver : MonoBehaviour {

	[HideInInspector]public bool CommandoForwardPressed = false;
	[HideInInspector]public bool CommandoBackPressed = false;
	[HideInInspector]public bool CommandoRightPressed = false;
	[HideInInspector]public bool CommandoLeftPressed = false;

	private Button _commandoForwardButton;
	private Button _commandoBackButton;
	private Button _commandoRightButton;
	private Button _commandoLeftButton;

	public void OnCommandoForwardDown()
	{
		CommandoForwardPressed = true;
		Debug.Log("Commando moving forward!");
	}
	public void OnCommandoForwardUp()
	{
		CommandoForwardPressed = false;
	}

	public void OnCommandoBackDown()
	{
		CommandoBackPressed = true;
		Debug.Log("Commando moving back!");
	}
	public void OnCommandoBackUp()
	{
		CommandoBackPressed = false;
	}

	public void OnCommandoRightDown()
	{
		CommandoRightPressed = true;
		Debug.Log("Commando moving right!");
	}
	public void OnCommandoRightUp()
	{
		CommandoRightPressed = false;
	}

	public void OnCommandoLeftDown()
	{
		CommandoLeftPressed = true;
		Debug.Log("Commando moving left!");
	}
	public void OnCommandoLeftUp()
	{
		CommandoLeftPressed = false;
	}
}
