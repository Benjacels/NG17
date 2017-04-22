using UnityEngine;
using System.Collections;

public class tempScript : MonoBehaviour {

    private bool _facingUp = true, _facingDown = false, _facingLeft = false, _facingRight = false;
    AudioSource[] _stepSounds;
    Animator _anim;

    Transform _origPos;
	// Use this for initialization
	void Start () {
        _origPos = transform;
        _anim = GetComponent<Animator>();

        _stepSounds = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(transform.rotation);
        _anim.SetBool("walking", false);
        if (Input.GetKey(KeyCode.W))
        {
            _anim.SetBool("walking", true);
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
            _anim.SetBool("walking", true);
            if (!_facingDown)
            {
                // negate scale y coordinate
                transform.rotation = new Quaternion(0, 0, 1, 0);
                _facingUp = false;
                _facingDown = true;
                _facingLeft = false;
                _facingRight = false;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            _anim.SetBool("walking", true);
            if (!_facingLeft)
            {
                //Rotate 90 degrees, orig scale
                transform.rotation = new Quaternion(0, 0, 0.7f, 0.7f);
                _facingUp = false;
                _facingDown = false;
                _facingLeft = true;
                _facingRight = false;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            _anim.SetBool("walking", true);
            if (!_facingRight)
            {
                //rotate 90 degrees around z, negate scale y
                transform.rotation = new Quaternion(0, 0, 0.7f, -0.7f);
                _facingUp = false;
                _facingDown = false;
                _facingLeft = false;
                _facingRight = true;
            }
        }
	}
    void PlaySoundStepOne()
    {
        _stepSounds[0].Play();
    }

    void PlaySoundStedTwo()
    {
        _stepSounds[1].Play();
    }
}
