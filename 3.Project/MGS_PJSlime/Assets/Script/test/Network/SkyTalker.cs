using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SkyTalker : MonoBehaviour {
	public static SkyTalker direct;
	public static NetworkManager networkManager;

	void Start() {
		direct = this;
		networkManager = GetComponent<NetworkManager>();
		networkManager.StartHost();
		DontDestroyOnLoad(gameObject);
	}

	/*
	public Text debugText;
	public bool isAtStartup = true;

	NetworkClient myClient;

	void Update() {
		if (isAtStartup) {
			if (Input.GetKeyDown(KeyCode.S)) {
				SetupServer();
			}

			if (Input.GetKeyDown(KeyCode.C)) {
				SetupClient();
			}

			if (Input.GetKeyDown(KeyCode.B)) {
				SetupServer();
				SetupLocalClient();
			}
		}
	}

	void OnGUI() {
		if (isAtStartup) {
			GUI.Label(new Rect(2, 10, 150, 100), "Press S for server");
			GUI.Label(new Rect(2, 30, 150, 100), "Press B for both");
			GUI.Label(new Rect(2, 50, 150, 100), "Press C for client");
		}
	}

	// Create a server and listen on a port
	public void SetupServer() {
		debugText.text = "SetupServer";
		NetworkServer.Listen(4444);
		isAtStartup = false;
	}

	// Create a client and connect to the server port
	public void SetupClient() {
		debugText.text = "SetupClient";
		myClient = new NetworkClient();
		myClient.RegisterHandler(MsgType.Connect, OnConnected);
		myClient.Connect("127.0.0.1", 4444);
		isAtStartup = false;
	}

	// Create a local client and connect to the local server
	public void SetupLocalClient() {
		debugText.text = "SetupLocalClient";
		myClient = ClientScene.ConnectLocalServer();
		myClient.RegisterHandler(MsgType.Connect, OnConnected);
		isAtStartup = false;
	}

	public void OnConnected(NetworkMessage netMsg) {
		debugText.text = "OnConnected";
		Debug.Log("Connected to server");
	}*/
	
		
	public void ResetScene() {
		//networkManager.StopHost();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void StopHost() {
		networkManager.StopHost();
	}
}