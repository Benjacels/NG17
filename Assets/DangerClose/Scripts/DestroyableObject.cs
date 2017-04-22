using UnityEngine;
using System.Collections;

public class DestroyableObject : MonoBehaviour {

	public enum DestroyableObjectState
	{
		Alive,
		Destroyed
	}

	private Vector3 _startPos;

	public static int TargetsDestroyed;

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

		if (gameObject.CompareTag("Target"))
			TargetsDestroyed++;

		if (TargetsDestroyed >= _player.TargetGoal)
			Debug.Log("VICTORY");
	}

	public virtual void ReviveObject()
	{
		GetComponent<SpriteRenderer>().sprite = AliveSprite;
		_currentDestroyState = DestroyableObjectState.Alive;

		transform.position = _startPos;
	}
}
