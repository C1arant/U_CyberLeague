using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Scoreboard : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private Transform container;

	[SerializeField]
	private GameObject scoreboardItemPrefab;

	private void Start()
	{
		Player[] playerList = PhotonNetwork.PlayerList;
		foreach (Player player in playerList)
		{
			AddScoreboardItem(player);
		}
	}

	private void AddScoreboardItem(Player player)
	{
		Object.Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreboardItem>().Initialize(player);
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		AddScoreboardItem(newPlayer);
	}
}
