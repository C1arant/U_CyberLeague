using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardItem : MonoBehaviour
{
	public Text usernameText;

	public Text KillsText;

	public Text DeathsText;

	public void Initialize(Player player)
	{
		usernameText.text = player.NickName;
	}
}
