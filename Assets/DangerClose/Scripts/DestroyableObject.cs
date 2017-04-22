using UnityEngine;
using System.Collections;

public class DestroyableObject : MonoBehaviour {

	public enum DestroyableObjectState
	{
		Alive,
		Destroyed
	}

	public static int TargetsDestroyed;

	private SpriteRenderer _spriteRenderer;
	private Player _player;

	public Sprite AliveSprite;
	public Sprite DestroyedSprite;

	private DestroyableObjectState _currentDestroyState;

	protected virtual void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		ReviveObject();
	}

	public virtual void DestroyObject()
	{
		_spriteRenderer.sprite = DestroyedSprite;
		_currentDestroyState = DestroyableObjectState.Destroyed;

		_player = FindObjectOfType<Player>();

		if (gameObject.CompareTag("Target"))
			TargetsDestroyed++;

		if (TargetsDestroyed >= _player.TargetGoal)
			Debug.Log("VICTORY");
	}

	public virtual void ReviveObject()
	{
		_spriteRenderer.sprite = AliveSprite;
		_currentDestroyState = DestroyableObjectState.Alive;
	}
}
