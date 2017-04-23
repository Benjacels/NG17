using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum PlayerTypeEnum
{
	Headquarters,
	Commando
}

public class Player : CaptainsMessPlayer {

	[SyncVar]
	private bool _hasStarted;

	public Image PlayerImage;
	public Text NameField;
	public Text ReadyField;

	public int TargetGoal;

	public GameObject BombPrefab;
	private GameObject _bomb;

	public GameObject CommandoPrefab;
	private GameObject _commando;

	private Vector3 _commandoStartPos;

	private int _prevtouchCount;

	private int _headquartersClickPos;
	private float _headquartersClickTime;

	private GameObject _buttonEventParent;

	private int _startBombAmmo = 50;

	[SyncVar]
	private int _bombAmmo;
	private Text _ammoNum;

	private PlayerTypeEnum _playerType;

	private GameObject _bombAmmoParent;

	private ButtonEventReceiver _buttonEventReceiver;

	private Rigidbody _rigidbody;
    Commando _commandoScript;

    public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();

		if (playerIndex == 0)
			_playerType = PlayerTypeEnum.Headquarters;
		else
		{
			Camera.main.orthographicSize /= 5;
			_playerType = PlayerTypeEnum.Commando;
		}
	}

	[Client]
	public override void OnStartClient()
	{
		base.OnStartClient();

		if(playerIndex == 0)
			ClientScene.RegisterPrefab(BombPrefab);
	}

	public override void OnClientEnterLobby()
	{
		base.OnClientEnterLobby();

		// Brief delay to let SyncVars propagate
		Invoke("ShowPlayer", 0.5f);
	}

	public override void OnClientReady(bool readyState)
	{
		if (readyState)
		{
			ReadyField.text = "READY!";
			ReadyField.color = Color.green;
		}
		else
		{
			ReadyField.text = "not ready";
			ReadyField.color = Color.red;
		}
	}

	void ShowPlayer()
	{
		transform.SetParent(GameObject.Find("Canvas/PlayerContainer").transform, false);

		NameField.text = deviceName;
		ReadyField.gameObject.SetActive(true);

		OnClientReady(IsReady());
	}

	public void Update()
	{
		if (_playerType == PlayerTypeEnum.Headquarters && isLocalPlayer && _hasStarted)
		{
			if (_bombAmmo < 1)
				return;

			Ray ray = new Ray();

			if (Input.GetMouseButtonDown(0))
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				CastRay(ray);
			}
			else if (Input.touchCount == 1 && _prevtouchCount == 0)
			{
				_prevtouchCount = Input.touchCount;

				Vector3 screenTouchPos = Input.GetTouch(0).position;
				ray = Camera.main.ScreenPointToRay(screenTouchPos);

				CastRay(ray);
			}
		}
		else if(_playerType == PlayerTypeEnum.Commando && isLocalPlayer && _hasStarted)
		{
			Button button = FindObjectOfType<Button>();

			if (_commando != null)
				Camera.main.transform.position = new Vector3(_commando.transform.position.x, _commando.transform.position.y, Camera.main.transform.position.z); ;
		}

		if (_hasStarted)
			NameField.gameObject.SetActive(false);
	}

	public void FixedUpdate()
	{
		if (_playerType == PlayerTypeEnum.Commando && isLocalPlayer && _hasStarted)
		{
			if (GameObject.FindObjectOfType<Commando>() == null)
				return;

			if (GameObject.FindObjectOfType<Commando>().CurrentDestroyState == DestroyableObject.DestroyableObjectState.Destroyed)
			{
				_rigidbody.Sleep();
				return;
			}

			MoveCommando();
		}
	}


	void CastRay(Ray ray)
	{
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			_headquartersClickTime = Time.time;
			SpawnBomb(Time.time, hit.point);
		}
	}

	public void SpawnBomb(float clickTime, Vector3 bombPos)
	{
		Debug.Log("Spawn Bomb");

		_bomb = Instantiate(BombPrefab, bombPos, Quaternion.identity) as GameObject;
		NetworkServer.Spawn(_bomb);

		_bombAmmo--;
		_ammoNum.text = _bombAmmo.ToString();
	}

	void SetBombAmmo(int ammo)
	{
		_bombAmmo = ammo;
	}

	[Command]
	public void CmdResetAmmo()
	{
		if (_playerType == PlayerTypeEnum.Commando)
			return;

		_bombAmmoParent = GameObject.FindWithTag("AmmoParent");
		foreach (Transform tran in _bombAmmoParent.transform)
		{
			if (tran.GetComponent<Text>() != null)
			{
				SetBombAmmo(50);
				_ammoNum = tran.GetComponent<Text>();
				_ammoNum.text = _bombAmmo.ToString();
			}
		}
	}


	[ClientRpc]
	public void RpcOnStartedGame()
	{
		ReadyField.gameObject.SetActive(false);
		Invoke("SetupGame", 0.5f);
	}

	private void SetupGame()
	{
		TargetGoal = GameObject.FindGameObjectsWithTag("Target").Length;

		_buttonEventReceiver = FindObjectOfType<ButtonEventReceiver>();

		_hasStarted = true;

		_commandoStartPos = GameObject.FindWithTag("CommandoStartPos").transform.position;

		_buttonEventParent = GameObject.FindWithTag("Commando_ButtonEventParent");

		if (_playerType == PlayerTypeEnum.Commando)
		{
			_buttonEventParent.SetActive(true);

			_commando = Instantiate(CommandoPrefab, _commandoStartPos, Quaternion.identity) as GameObject;

            _commandoScript = _commando.GetComponent<Commando>();
           

			_rigidbody = _commando.GetComponent<Rigidbody>();
		}
		else if (_playerType == PlayerTypeEnum.Headquarters && isLocalPlayer)
		{
			_bombAmmo = _startBombAmmo;

			TankScript[] tankScripts = GameObject.FindObjectsOfType(typeof(TankScript)) as TankScript[];
			foreach (TankScript tankScript in tankScripts)
				tankScript.gameObject.SetActive(false);

			if(_buttonEventParent != null)
				_buttonEventParent.gameObject.SetActive(false);

			GameObject.FindWithTag("IntelObject").transform.GetChild(0).gameObject.SetActive(true);
			GameObject.FindWithTag("LZObject").transform.GetChild(0).gameObject.SetActive(true);

			_bombAmmoParent = GameObject.FindWithTag("AmmoParent");

			foreach (Transform tran in _bombAmmoParent.transform)
			{
				tran.gameObject.SetActive(true);

				if (tran.GetComponent<Text>() != null)
					_ammoNum = tran.GetComponent<Text>();
			}
		}
	}

	[Command]
	public void CmdEndGame()
	{
		FindObjectOfType<EndGameScript>().EndGame();
	}

	private void MoveCommando()
	{
        if (_buttonEventReceiver == null)
        {
            _commandoScript.SetWalkingAnimation(false);
            return;
        }

		float movementSpeed = 2;

        _commandoScript.SetWalkingAnimation(false);
		if (_buttonEventReceiver.CommandoForwardPressed || Input.GetKey(KeyCode.UpArrow))
        {
            _commandoScript.FaceUp();
            _commandoScript.SetWalkingAnimation(true);
			_rigidbody.velocity = Vector3.up * movementSpeed;
        }
		else if (_buttonEventReceiver.CommandoBackPressed || Input.GetKey(KeyCode.DownArrow))
        {
            _commandoScript.FaceDown();
            _commandoScript.SetWalkingAnimation(true);
			_rigidbody.velocity = Vector3.down * movementSpeed;
        }
		else if (_buttonEventReceiver.CommandoRightPressed || Input.GetKey(KeyCode.RightArrow))
        {
            _commandoScript.FaceRight();
            _commandoScript.SetWalkingAnimation(true);
			_rigidbody.velocity = Vector3.right * movementSpeed;
        }
		else if (_buttonEventReceiver.CommandoLeftPressed || Input.GetKey(KeyCode.LeftArrow))
        {
            _commandoScript.FaceLeft();
            _commandoScript.SetWalkingAnimation(true);
			_rigidbody.velocity = Vector3.left * movementSpeed;
        }
        else
            _rigidbody.Sleep();
	}

	void OnGUI()
	{
		if (isLocalPlayer)
		{
			GUILayout.BeginArea(new Rect(0, Screen.height * 0.8f, Screen.width, 100));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			GameSession gameSession = GameSession.instance;
			if (gameSession)
			{
				if (gameSession.gameState == GameStateEnum.Lobby ||
					gameSession.gameState == GameStateEnum.Countdown)
				{
					if (GUILayout.Button(IsReady() ? "Not ready" : "Ready", GUILayout.Width(Screen.width * 0.3f), GUILayout.Height(100)))
					{
						if (IsReady())
						{
							SendNotReadyToBeginMessage();
						}
						else
						{
							SendReadyToBeginMessage();
						}
					}
				}
				//else if (gameSession.gameState == GameState.GameOver)
				//{
				//	if (isServer)
				//	{
				//		if (GUILayout.Button("Play Again", GUILayout.Width(Screen.width * 0.3f), GUILayout.Height(100)))
				//		{
				//			CmdPlayAgain();
				//		}
				//	}
				//}
			}

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}
}
