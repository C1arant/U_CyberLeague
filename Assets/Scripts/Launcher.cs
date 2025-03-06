using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
	public static Launcher Instance;

	[SerializeField]
	private InputField RoomNameInputField;

	[SerializeField]
	private Text ErrorText;

	[SerializeField]
	private Text roomNameText;

	[SerializeField]
	private Transform roomListContent;

	[SerializeField]
	private GameObject RoomListItemPrefab;

	[SerializeField]
	private Transform playerListContent;

	[SerializeField]
	private GameObject playerListItemPrefab;

	[SerializeField]
	private GameObject StartGameButton;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public override void OnJoinedLobby()
	{
		MenuManager.Instance.OpenMenu("title");
		Debug.Log("Joined lobby");
	}

	public void CreateRoom()
	{
		if (!string.IsNullOrEmpty(RoomNameInputField.text))
		{
			PhotonNetwork.CreateRoom(RoomNameInputField.text);
			MenuManager.Instance.OpenMenu("loading");
		}
	}

	public override void OnJoinedRoom()
	{
		MenuManager.Instance.OpenMenu("room");
		roomNameText.text = PhotonNetwork.CurrentRoom.Name;
		Player[] playerList = PhotonNetwork.PlayerList;
		foreach (Transform item in playerListContent)
		{
			Object.Destroy(item.gameObject);
		}
		for (int i = 0; i < playerList.Count(); i++)
		{
			Object.Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(playerList[i]);
		}
		StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}

	public void JoinRoom(RoomInfo info)
	{
		PhotonNetwork.JoinRoom(info.Name);
		MenuManager.Instance.OpenMenu("loading");
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
		MenuManager.Instance.OpenMenu("loading");
	}

	public override void OnLeftRoom()
	{
		MenuManager.Instance.OpenMenu("title");
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		ErrorText.text = "Room creation failed" + message;
		MenuManager.Instance.OpenMenu("error");
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		foreach (Transform item in roomListContent)
		{
			Object.Destroy(item.gameObject);
		}
		for (int i = 0; i < roomList.Count; i++)
		{
			if (!roomList[i].RemovedFromList)
			{
				Object.Instantiate(RoomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);
			}
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Object.Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
	}

	public void StartGame()
	{
		PhotonNetwork.LoadLevel(1);
	}
}
