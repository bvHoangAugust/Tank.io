using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
	#pragma warning disable 649
	public class LauncherPhoton : MonoBehaviourPunCallbacks
    {
		#region Private Serializable Fields
		[SerializeField]
		private byte maxPlayersPerRoom = 4;
		#endregion

		#region Private Fields
		bool isConnecting;
		string gameVersion = "1";

        #endregion

        public Button playGameButton;
        public TextMeshProUGUI textWarningInput;
        public TMP_InputField playerNameInput;

        #region MonoBehaviour CallBacks
        void Awake()
		{
			PhotonNetwork.AutomaticallySyncScene = true;
            if (PlayerPrefs.HasKey("NamePlayer"))
            {
                playerNameInput.text = PlayerPrefs.GetString("NamePlayer");
            }

            playGameButton.onClick.AddListener(() => PlayGameButton());
        }
        #endregion

        #region Public Methods
        public void PlayGameButton()
        {
            if (playerNameInput.text == null || playerNameInput.text == "")
            {
                textWarningInput.gameObject.SetActive(true);
            }
            else
            {
                playGameButton.onClick.RemoveAllListeners();
                textWarningInput.gameObject.SetActive(false);
                PlayerPrefs.SetString("NamePlayer", playerNameInput.text);
                PhotonNetwork.NickName = PlayerPrefs.GetString("NamePlayer");
                Connect();
            }
        }
        public void Connect()
		{
            isConnecting = true;
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("Joining Room...");
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                Debug.Log("Connecting...");
                PhotonNetwork.GameVersion = this.gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion


        #region MonoBehaviourPunCallbacks CallBacks
        public override void OnConnectedToMaster()
		{
			if (isConnecting)
			{
				Debug.Log("OnConnectedToMaster: Next -> try to Join Random Room");
				Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");
				PhotonNetwork.JoinRandomRoom();
			}
		}
		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			Debug.Log("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
			Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            RoomOptions roomOpt = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = maxPlayersPerRoom };
            PhotonNetwork.CreateRoom("Room", roomOpt);
		}

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Tried to create a new room but failed, there must already be a room with a same name");
            RoomOptions roomOpt = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = maxPlayersPerRoom };
            PhotonNetwork.CreateRoom("Room", roomOpt);
        }

        public override void OnDisconnected(DisconnectCause cause)
		{
			Debug.Log("<Color=Red>OnDisconnected</Color> "+cause);
			Debug.LogError("PUN Basics Tutorial/Launcher:Disconnected");
			isConnecting = false;
		}


		public override void OnJoinedRoom()
		{
			Debug.Log("<Color=Green>OnJoinedRoom</Color> with "+PhotonNetwork.CurrentRoom.PlayerCount+" Player(s)");
			Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");
		
			if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
			{
				Debug.Log("We load the room ");
				PhotonNetwork.LoadLevel("GamePlayScene");

			}
		}

		#endregion
		
	}
}