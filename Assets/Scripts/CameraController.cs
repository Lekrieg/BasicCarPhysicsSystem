using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform objectToFollow;
	public Vector3 offset;
	public float followSpeed = 10;
	public float lookSpeed = 10;

	private void LateUpdate()
	{
		LookAtTarget();
		MoveToTarget();
	}

	public void LookAtTarget()
	{
		Vector3 _lookDirection = objectToFollow.position - transform.position;
		Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
	}

	public void MoveToTarget()
	{
		Vector3 _targetPos = objectToFollow.position + 
							 objectToFollow.forward * offset.z + 
							 objectToFollow.right * offset.x + 
							 objectToFollow.up * offset.y;
		transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
	}

    /* public GameObject objectToFollow;
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
	} */
}
