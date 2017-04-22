using UnityEngine;
using System.Collections;

public class Commando : DestroyableObject {

    // gem originale transform værdier, så vi kan ændre orientering, uden at fucke inspector værdier op
    private Transform _origPos;

	public bool HasTriggerObject;

    private bool _facingUp = true, _facingDown = false, _facingLeft = false, _facingRight = false;

	protected override void Start()
	{
		base.Start();

		_startPos = transform.position;
        _origPos = transform;
	}

	public override void ReviveObject()
	{
		base.ReviveObject();

		transform.position = _startPos;
	}

	public override void DestroyObject()
	{
		base.DestroyObject();

		Invoke("ShowResetGameVisuals", 3);
	}

	void ShowResetGameVisuals()
	{
		GameObject.FindWithTag("ResetGameParent").transform.GetChild(0).gameObject.SetActive(true);
	}

    public void FaceUp()
    {
        // set back to orig transform, not position obviously
        if (!_facingUp)
        {
            transform.localScale = _origPos.localScale;
            //transform.rotation = _origPos.rotation;
            _facingUp = true;
            _facingDown = false;
            _facingLeft = false;
            _facingRight = false;
        }
    }
    
    public void FaceDown()
    {
        if (!_facingDown)
        {
            // negate scale y coordinate
            transform.localScale = new Vector3(_origPos.localScale.x, -_origPos.localScale.y, _origPos.localScale.z);
            //transform.rotation = _origPos.rotation;
            _facingUp = false;
            _facingDown = true;
            _facingLeft = false;
            _facingRight = false;
        }
    }

    public void FaceRight()
    {
        if (!_facingRight)
        {
            //rotate 90 degrees around z, negate scale y
            transform.localScale = new Vector3(_origPos.localScale.x, -_origPos.localScale.y, _origPos.localScale.z);
            //transform.rotation = new Quaternion(0, 0, 0.7f, 0.7f);
            _facingUp = false;
            _facingDown = false;
            _facingLeft = false;
            _facingRight = true;
        }
    }

    public void FaceLeft()
    {
        if (!_facingLeft)
        {
            //Rotate 90 degrees, orig scale
            transform.localScale = _origPos.localScale;
            //transform.rotation = new Quaternion(0, 0, 0.7f, 0.7f);
            _facingUp = false;
            _facingDown = false;
            _facingLeft = true;
            _facingRight = false;
        }
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("IntelObject"))
		{
			other.gameObject.SetActive(false);
			HasTriggerObject = true;
		}

		if (other.gameObject.CompareTag("LZObject") && HasTriggerObject)
		{
			other.gameObject.SetActive(false);
			FindObjectOfType<Player>().CmdEndGame();
			FindObjectOfType<EndGameScript>().EndGame();
		}
	}
}
