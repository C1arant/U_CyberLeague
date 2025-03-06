using System;
using LootLocker.Requests;
using UnityEngine;
using UnityEngine.UI;

public class lootLockerManager : MonoBehaviour
{
	public InputField PlayerNameInputField;

	public GameObject createPlayerButton;

	public Slider XPSlider;

	public Text CurrentLevelText;

	public Text NextLevelText;

	public Text currentxpText;

	private Guid currentDeviceID;

	private int xpToGive = 30;

	private string XpToAdd;

	public void createPlayer()
	{
		currentDeviceID = Guid.NewGuid();
		PlayerPrefs.SetString("GUID", currentDeviceID.ToString());
		LootLockerSDKManager.StartSession(currentDeviceID.ToString(), delegate(LootLockerSessionResponse response)
		{
			if (response.success)
			{
				LootLockerSDKManager.SetPlayerName(PlayerNameInputField.text, null);
			}
			else
			{
				Debug.Log("failed" + response.Error);
			}
		});
	}

	public void Login()
	{
		LootLockerSDKManager.StartSession(PlayerPrefs.GetString("GUID", ""), delegate(LootLockerSessionResponse response)
		{
			if (response.success)
			{
				Debug.Log("Succses");
			}
			else
			{
				Debug.Log("failed" + response.Error);
			}
		});
	}

	public void GiveXP()
	{
		LootLockerSDKManager.SubmitXp(xpToGive, delegate(LootLockerXpSubmitResponse response)
		{
			if (response.success)
			{
				Debug.Log("Successful");
			}
			else
			{
				Debug.Log("Error: " + response.Error);
			}
		});
	}

	public void CheckLevel()
	{
		LootLockerSDKManager.GetPlayerInfo(delegate
		{
			SingleShotGun.Instance.OnPlayerShootGun();
		});
	}
}
