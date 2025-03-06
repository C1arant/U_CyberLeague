using Photon.Pun;
using UnityEngine;

public class AutoShotGun : Gun
{
	[SerializeField]
	private Camera cam;

	private PhotonView PV;

	public float fireRate = 10f;

	public float nextTimetoFire = 4f;

	[SerializeField]
	private AudioSource Asourse;

	[SerializeField]
	private AudioClip Clip;

	private void Awake()
	{
		PV = GetComponent<PhotonView>();
	}

	public override void Use()
	{
		if (Input.GetButton("Fire1") && Time.time >= nextTimetoFire)
		{
			Shoot();
		}
	}

	private void Update()
	{
		if (Input.GetButton("Fire1") && Time.time >= nextTimetoFire)
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		ray.origin = cam.transform.position;
		if (Physics.Raycast(ray, out var hitInfo))
		{
			nextTimetoFire = Time.time + 1f / fireRate;
		}
		Debug.Log(hitInfo.collider.gameObject.GetComponent<IDamageable>());
		hitInfo.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)iteminfo).damage);
		Asourse.PlayOneShot(Clip);
	}
}
