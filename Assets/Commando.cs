using UnityEngine;
using System.Collections;

public class Commando : DestroyableObject {

    // gem originale transform værdier, så vi kan ændre orientering, uden at fucke inspector værdier op
    private Transform _origPos;

	public bool HasTriggerObject;

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
        transform.localScale = _origPos.localScale;
        transform.rotation = _origPos.rotation;
    }
    
    public void FaceDown()
    {
        // negate scale y coordinate
        transform.localScale = new Vector3(_origPos.localScale.x, -_origPos.localScale.y, _origPos.localScale.z);
        transform.rotation = _origPos.rotation;
    }

    public void FaceRight()
    {
        //rotate 90 degrees around z, negate scale y
        transform.localScale = new Vector3(_origPos.localScale.x, -_origPos.localScale.y, _origPos.localScale.z);
        transform.rotation.SetEulerAngles(0, 0, 90); // Ugh, deprecated, håber det duer
    }

    public void FaceLeft()
    {
        //Rotate 90 degrees, orig scale
        transform.localScale = _origPos.localScale;
        transform.rotation.SetEulerAngles(0, 0, 90);
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("IntelObject"))
			HasTriggerObject = true;

		if (other.gameObject.CompareTag("LZObject") && HasTriggerObject)
			FindObjectOfType<Player>().CmdEndGame();
	}
}
