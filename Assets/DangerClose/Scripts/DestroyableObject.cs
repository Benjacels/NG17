using UnityEngine;
using System.Collections;

public class DestroyableObject : MonoBehaviour {

	public enum DestroyableObjectState
	{
		Alive,
		Destroyed
	}

	protected Vector3 _startPos;

	private Player _player;

	public Sprite AliveSprite;
	public Sprite DestroyedSprite;

	public DestroyableObjectState CurrentDestroyState;

	protected virtual void Start()
	{
		_startPos = transform.position;

		ReviveObject();
	}

	public virtual void DestroyObject()
	{
		GetComponent<SpriteRenderer>().sprite = DestroyedSprite;
		CurrentDestroyState = DestroyableObjectState.Destroyed;

		_player = FindObjectOfType<Player>();
	}

	public virtual void ReviveObject()
	{
		GetComponent<SpriteRenderer>().sprite = AliveSprite;
		CurrentDestroyState = DestroyableObjectState.Alive;

		transform.position = _startPos;
	}
}
