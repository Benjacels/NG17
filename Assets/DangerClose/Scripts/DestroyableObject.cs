using UnityEngine;
using System.Collections;

public class DestroyableObject : MonoBehaviour {

	public enum DestroyableObjectState
	{
		Alive,
		Destroyed
	}

	private SpriteRenderer _spriteRenderer;

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
	}

	public virtual void ReviveObject()
	{
		_spriteRenderer.sprite = AliveSprite;
		_currentDestroyState = DestroyableObjectState.Alive;
	}
}
