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

	public GameObject BombPrefab;
	private GameObject _bomb;

	public GameObject CommandoPrefab;
	private GameObject _commando;

	private Vector3 _commandoStartPos;

	private int _prevtouchCount;

	private int _headquartersClickPos;
	private float _headquartersClickTime;

	private PlayerTypeEnum _playerType;

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();

		if (playerIndex == 0)
			_playerType = PlayerTypeEnum.Headquarters;
		else
			_playerType = PlayerTypeEnum.Commando;
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
			//UpdateMover();
		}

		if (_hasStarted)
			NameField.gameObject.SetActive(false);
		
		//CmdGetMoverPosition(pos);
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
	}

	[ClientRpc]
	public void RpcOnStartedGame()
	{
		ReadyField.gameObject.SetActive(false);
		Invoke("SetupGame", 0.5f);
	}

	private void SetupGame()
	{
		_hasStarted = true;

		_commandoStartPos = GameObject.FindWithTag("CommandoStartPos").transform.position;

		if (_playerType == PlayerTypeEnum.Commando && isLocalPlayer && CommandoPrefab != null)
			_commando = Instantiate(CommandoPrefab, _commandoStartPos, Quaternion.identity) as GameObject;
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
