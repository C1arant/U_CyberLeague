using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
	private PlayerController playerController;

	private void Awake()
	{
		playerController = GetComponentInParent<PlayerController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!(other.gameObject == playerController.gameObject))
		{
			playerController.SetGroundedState(_grounded: true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!(other.gameObject == playerController.gameObject))
		{
			playerController.SetGroundedState(_grounded: false);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (!(other.gameObject == playerController.gameObject))
		{
			playerController.SetGroundedState(_grounded: true);
		}
	}
}
