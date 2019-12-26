using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject objectToFollow;
	public float distance = 4f;
	public float height = 2f;
	public float dampening = 1f;

	public float height2 = 0f;
	public float distance2 = 0f;
	public float l = 0f;

	public int camMode = 0;

	void Update()
	{
		CamModeInput();

		switch(camMode)
		{
			case 1:
				transform.position = objectToFollow.transform.position +
									 objectToFollow.transform.TransformDirection(new Vector3(l, height2, distance2));
				transform.rotation = objectToFollow.transform.rotation;
				Camera.main.fieldOfView = 90f;
				break;
			default:
				transform.position = Vector3.Lerp(transform.position,
										  objectToFollow.transform.position + objectToFollow.transform.TransformDirection(new Vector3(0f, height, -distance)),
										  dampening * Time.deltaTime);
				transform.LookAt(objectToFollow.transform);
				Camera.main.fieldOfView = 60f;
				break;
		}
	}

	private void CamModeInput()
	{
		if(Input.GetKeyDown(KeyCode.C))
		{
			camMode = (camMode + 1) % 2; // 0 e 1, se for ter mais é só aumentar o valor
		}
	}
}
