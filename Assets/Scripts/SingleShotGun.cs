using LootLocker.Requests;
using Photon.Pun;
using UnityEngine;

public class SingleShotGun : Gun
{
	[SerializeField]
	private Camera cam;

	[SerializeField]
	private int XpToAdd = 10;

	[SerializeField]
	private AudioSource Asourse;

	[SerializeField]
	private AudioClip Clip;

	public static SingleShotGun Instance;

	private PhotonView PV;

	private void Awake()
	{
		PV = GetComponent<PhotonView>();
		Instance = this;
	}

	public override void Use()
	{
		Shoot();
	}

	private void Shoot()
	{
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		ray.origin = cam.transform.position;
		if (Physics.Raycast(ray, out var hitInfo))
		{
			hitInfo.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)iteminfo).damage);
			OnPlayerShootGun();
			Asourse.PlayOneShot(Clip);
		}
	}

	[PunRPC]
	private void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
	{
		Collider[] array = Physics.OverlapSphere(hitPosition, 0.3f);
		if (array.Length != 0)
		{
			GameObject obj = Object.Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
			Object.Destroy(obj, 10f);
			obj.transform.SetParent(array[0].transform);
		}
	}

	public void OnPlayerShootGun()
	{
		LootLockerSDKManager.SubmitXp(XpToAdd, delegate(LootLockerXpSubmitResponse response)
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
}
