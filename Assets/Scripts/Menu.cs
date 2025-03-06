using UnityEngine;

public class Menu : MonoBehaviour
{
	public string menuName;

	public bool open;

	public void Open()
	{
		open = true;
		base.gameObject.SetActive(value: true);
	}

	public void Close()
	{
		open = false;
		base.gameObject.SetActive(value: false);
	}
}
