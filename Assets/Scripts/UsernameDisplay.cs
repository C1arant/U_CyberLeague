using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UsernameDisplay : MonoBehaviour
{
	[SerializeField]
	private PhotonView playerPV;

	[SerializeField]
	private Text text;

	private void Start()
	{
		if (playerPV.IsMine)
		{
			base.gameObject.SetActive(value: false);
		}
		text.text = playerPV.Owner.NickName;
	}
}
