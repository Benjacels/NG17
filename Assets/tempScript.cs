using UnityEngine;
using System.Collections;

public class tempScript : MonoBehaviour {

    private bool _facingUp = true, _facingDown = false, _facingLeft = false, _facingRight = false;

    Transform _origPos;
	// Use this for initialization
	void Start () {
        _origPos = transform;	
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(transform.rotation);
        if (Input.GetKey(KeyCode.W))
        {
            if (!_facingUp)
            {
                transform.localScale = _origPos.localScale;
                transform.rotation = Quaternion.identity; 
                _facingUp = true;
                _facingDown = false;
                _facingLeft = false;
                _facingRight = false;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (!_facingDown)
            {
                // negate scale y coordinate
                transform.localScale = new Vector3(_origPos.localScale.x, -_origPos.localScale.y, _origPos.localScale.z);
                transform.rotation = Quaternion.identity;
                _facingUp = false;
                _facingDown = true;
                _facingLeft = false;
                _facingRight = false;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (!_facingLeft)
            {
                //Rotate 90 degrees, orig scale
                transform.localScale = _origPos.localScale;
                transform.rotation = new Quaternion(0, 0, 0.7f, 0.7f);
                _facingUp = false;
                _facingDown = false;
                _facingLeft = true;
                _facingRight = false;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (!_facingRight)
            {
                //rotate 90 degrees around z, negate scale y
                transform.localScale = new Vector3(_origPos.localScale.x, -_origPos.localScale.y, _origPos.localScale.z);
                transform.rotation = new Quaternion(0, 0, 0.7f, 0.7f);
                _facingUp = false;
                _facingDown = false;
                _facingLeft = false;
                _facingRight = true;
            }
        }
        	
        	
	}
}
