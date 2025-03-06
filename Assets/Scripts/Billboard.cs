using UnityEngine;

public class Billboard : MonoBehaviour
{
	private Camera cam;

	private void Update()
	{
		if (cam == null)
		{
			cam = Object.FindObjectOfType<Camera>();
		}
		if (!(cam == null))
		{
			base.transform.LookAt(cam.transform);
			base.transform.Rotate(Vector3.up * 180f);
		}
	}
}
