using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonEventReceiver : MonoBehaviour {

	public bool CommandoForwardPressed = false;
	public bool CommandoBackPressed = false;
	public bool CommandoRightPressed = false;
	public bool CommandoLeftPressed = false;

	private Button _commandoForwardButton;
	private Button _commandoBackButton;
	private Button _commandoRightButton;
	private Button _commandoLeftButton;

	public void OnCommandoForwardDown()
	{
		CommandoForwardPressed = true;
	}
	public void OnCommandoForwardUp()
	{
		CommandoForwardPressed = false;
	}

	public void OnCommandoBackDown()
	{
		CommandoBackPressed = true;
	}
	public void OnCommandoBackUp()
	{
		CommandoBackPressed = false;
	}

	public void OnCommandoRightDown()
	{
		CommandoRightPressed = true;
	}
	public void OnCommandoRightUp()
	{
		CommandoRightPressed = false;
	}

	public void OnCommandoLeftDown()
	{
		CommandoLeftPressed = true;
	}
	public void OnCommandoLeftUp()
	{
		CommandoLeftPressed = false;
	}
}
