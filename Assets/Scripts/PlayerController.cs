using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
	[SerializeField]
	private float mouseSensitivity;

	[SerializeField]
	private float sprintSpeed;

	[SerializeField]
	private float walkSpeed;

	[SerializeField]
	private float jumpForce;

	[SerializeField]
	private float smoothTime;

	[SerializeField]
	private GameObject cameraHolder;

	[SerializeField]
	private Image healthbarImage;

	[SerializeField]
	private GameObject ui;

	[SerializeField]
	private Item[] items;

	private int itemIndex;

	private int previousItemIndex = -1;

	private float verticalLookRotation;

	private bool grounded;

	private Vector3 smoothMoveVelocity;

	private Vector3 moveAmount;

	private Rigidbody rb;

	private PhotonView PV;

	private const float maxHealth = 150f;

	private float currentHealth = 150f;

	private PlayerManager playerManager;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		PV = GetComponent<PhotonView>();
		playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
	}

	private void Start()
	{
		if (PV.IsMine)
		{
			EquipItem(0);
		}
		else
		{
			Object.Destroy(GetComponentInChildren<Camera>().gameObject);
			Object.Destroy(rb);
			Object.Destroy(ui);
		}
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		if (!PV.IsMine)
		{
			return;
		}
		Look();
		Move();
		Jump();
		for (int i = 0; i < items.Length; i++)
		{
			if (Input.GetKeyDown((i + 1).ToString()))
			{
				EquipItem(i);
				break;
			}
		}
		if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
		{
			if (itemIndex >= items.Length - 1)
			{
				EquipItem(0);
			}
			else
			{
				EquipItem(itemIndex + 1);
			}
		}
		else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
		{
			if (itemIndex <= 0)
			{
				EquipItem(items.Length - 1);
			}
			else
			{
				EquipItem(itemIndex - 1);
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			items[itemIndex].Use();
		}
		if (base.transform.position.y < -10f)
		{
			Die();
		}
	}

	private void EquipItem(int _index)
	{
		if (_index != previousItemIndex)
		{
			itemIndex = _index;
			items[itemIndex].itemGameObject.SetActive(value: true);
			if (previousItemIndex != -1)
			{
				items[previousItemIndex].itemGameObject.SetActive(value: false);
			}
			previousItemIndex = itemIndex;
			if (PV.IsMine)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("itemIndex", itemIndex);
				PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
			}
		}
	}

	private void Look()
	{
		base.transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
		verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
		cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
	}

	private void Move()
	{
		Vector3 normalized = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
		moveAmount = Vector3.SmoothDamp(moveAmount, normalized * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
	}

	private void Jump()
	{
		if (Input.GetKeyDown(KeyCode.Space) && grounded)
		{
			rb.AddForce(base.transform.up * jumpForce);
		}
	}

	public void SetGroundedState(bool _grounded)
	{
		grounded = _grounded;
	}

	private void FixedUpdate()
	{
		if (PV.IsMine)
		{
			rb.MovePosition(rb.position + base.transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		if (!PV.IsMine && targetPlayer == PV.Owner)
		{
			EquipItem((int)changedProps["itemIndex"]);
		}
	}

	public void TakeDamage(float damage)
	{
		PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
	}

	[PunRPC]
	private void RPC_TakeDamage(float damage)
	{
		if (PV.IsMine)
		{
			currentHealth -= damage;
			healthbarImage.fillAmount = currentHealth / 150f;
			if (currentHealth <= 0f)
			{
				Die();
			}
		}
	}

	private void Die()
	{
		playerManager.Die();
	}
}
