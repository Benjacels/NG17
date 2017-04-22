using UnityEngine;
using System.Collections;

public class Commando : DestroyableObject {

	private Vector3 _startPos;

	protected override void Start()
	{
		base.Start();

		_startPos = transform.position;
	}

	public override void ReviveObject()
	{
		base.ReviveObject();

		transform.position = _startPos;
	}
}
