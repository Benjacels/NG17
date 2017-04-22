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

	private DestroyableObjectState _currentDestroyState;

	protected virtual void Start()
	{
		_startPos = transform.position;

		ReviveObject();
	}

	public virtual void DestroyObject()
	{
		GetComponent<SpriteRenderer>().sprite = DestroyedSprite;
		_currentDestroyState = DestroyableObjectState.Destroyed;

		_player = FindObjectOfType<Player>();
	}

	public virtual void ReviveObject()
	{
		GetComponent<SpriteRenderer>().sprite = AliveSprite;
		_currentDestroyState = DestroyableObjectState.Alive;

		transform.position = _startPos;
	}
}
