using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
	[SerializeField]
	private Text text;

	public RoomInfo info;

	public void Setup(RoomInfo _info)
	{
		info = _info;
		text.text = _info.Name;
	}

	public void Onclick()
	{
		Launcher.Instance.JoinRoom(info);
	}
}
