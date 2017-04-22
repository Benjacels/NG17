using UnityEngine;
using System.Collections;

public class BombBehavior : MonoBehaviour {

	public float TargetSize = 10;
	public float ScaleSpeed = 20;
	public float BombRadius = 3;
	public float DestroyDelay = 0.1f;

	private SphereCollider _collider;
	private SpriteRenderer _spriteRenderer;
	private bool _dropBomb = false;
	private float _startTime;

	private Vector3 _startSize;
	private Vector3 _targetSize;

	void Awake()
	{
		_collider = GetComponent<SphereCollider>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_collider.enabled = false;
		_collider.radius *= BombRadius;

		_startSize = transform.localScale;
		_targetSize = transform.localScale / TargetSize;
	}

	// Use this for initialization
	void Start () {
		_dropBomb = true;
		_startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		DropBomb();
	}

	void DropBomb()
	{
		if (!_dropBomb)
			return;

		float dist = Vector3.Distance(_startSize, _targetSize);
		float progressTime = (Time.time - _startTime) * ScaleSpeed;

		float progression = progressTime / dist;

		transform.localScale = Vector3.Lerp(_startSize, _targetSize, progression);

		if (transform.localScale == _targetSize)
			Explode();
	}

	void Explode()
	{
		_collider.enabled = true;
		_spriteRenderer.enabled = false;

		DestroyObject(gameObject, DestroyDelay);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<DestroyableObject>() != null)
			other.GetComponent<DestroyableObject>().DestroyObject();
	}
}
