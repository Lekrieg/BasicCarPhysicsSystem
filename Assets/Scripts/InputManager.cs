using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float throttle;
	public float steer;
	public bool brake;

	void Update()
	{
		GetInput();
	}

	private void GetInput()
	{
		throttle = Input.GetAxis("Acceleration");
		steer = Input.GetAxis("Steering");
		brake = Input.GetKey(KeyCode.Space);
	}
}
