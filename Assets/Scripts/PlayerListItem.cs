using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private Text text;

	private Player player;

	public void SetUp(Player _player)
	{
		player = _player;
		text.text = _player.NickName;
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		if (player == otherPlayer)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public override void OnLeftRoom()
	{
		Object.Destroy(base.gameObject);
	}
}
