using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class CarController : MonoBehaviour
{
	public enum DriveMode { Front, Rear, All };
	public DriveMode driveMode = DriveMode.All;

	[Header("Managers")]
	public InputManager inputManager;
	public UIManager uiManager;

	[Header("Wheel colliders")]
	// TODO: transformar em um array depois
	public WheelCollider[] wheelColliders;

	[Header("Wheel meshes")]
	public GameObject[] wheelMeshes;

	[Header("SteeringWheel and pedals")]
	public GameObject driverSteeringWheel;
	public GameObject accelerationPedal;
	public GameObject brakePedal;

	[Header("Car physics stuffy")]
	public float idealRPM = 50f;
	public float maxRPM = 1000f;
	public float maxAngle = 30f;
	public float torque = 1000f;
	public float brakeTorque = 400.0f;

	public Transform centerOfMass;
	public Rigidbody carRigidbody;

	void Start()
	{
		inputManager = GetComponent<InputManager>();

		if(centerOfMass)
		{
			carRigidbody.centerOfMass = centerOfMass.localPosition;
		}
	}

	void FixedUpdate()
	{
		uiManager.UpdateSpeedometer(GetSpeed());
		Accelerate();
		Steer();
		TurnSteeringWheel();
		UpdateWheelMeshsRotation();
	}

	public float GetSpeed()
	{
		return wheelColliders[0].radius * Mathf.PI *
			   wheelColliders[0].rpm * 60f/1000f;
	}

	void Accelerate()
	{
		float scaledTorque = inputManager.throttle * torque;

		if(scaledTorque < 0)
		{
			AnimatePedal(brakePedal);
		}
		else
		{
			AnimatePedal(accelerationPedal);
		}

		if(wheelColliders[0].GetComponent<WheelCollider>().rpm < idealRPM)
		{
			scaledTorque = Mathf.Lerp(scaledTorque/10f, scaledTorque, wheelColliders[0].GetComponent<WheelCollider>().rpm / idealRPM);
		}
		else
		{
			scaledTorque = Mathf.Lerp(scaledTorque, 0, (wheelColliders[0].GetComponent<WheelCollider>().rpm - idealRPM) / (maxRPM - idealRPM));
		}

		for(int i = 0; i < wheelColliders.Length; i++)
		{
			wheelColliders[i].GetComponent<WheelCollider>().motorTorque = driveMode == DriveMode.Rear ? 0 : scaledTorque;
			if(inputManager.brake)
			{
				wheelColliders[i].GetComponent<WheelCollider>().brakeTorque = brakeTorque;
			}
			else
			{
				wheelColliders[i].GetComponent<WheelCollider>().brakeTorque = 0;
			}
		}
	}
	void AnimatePedal(GameObject pedal)
	{
		// pedal rot * dir• amounttomovemax•accelAxis
		if(pedal)
		{
			// Quaternion pedalRotation = pedal.transform.localRotation;
			// Vector3 pedalDiretion = pedal.transform.position.normalized;
			float angle = Mathf.Abs(-30f * inputManager.throttle);

			pedal.transform.localRotation = Quaternion.Euler(angle, 0f, 0f);

			//Debug.Log(pedalRotation * pedalDiretion * angle);
			Debug.Log(angle);
		}
	}

	void Steer()
	{
		float angleInput = Mathf.Lerp(wheelColliders[0].steerAngle, maxAngle * inputManager.steer, Time.deltaTime * 10);

		// Left
		wheelColliders[0].steerAngle = angleInput;
		// wheelColliders[0].GetComponent<WheelCollider>().transform.localEulerAngles = new Vector3(0f, angleInput, 0f);

		// Right
		wheelColliders[1].steerAngle = angleInput;
		// wheelColliders[1].GetComponent<WheelCollider>().transform.localEulerAngles = new Vector3(0f, angleInput, 0f);
	}
	// Rotacao do pedal * angulo maximo de movimentacao * axisAcceleration
	public void TurnSteeringWheel()
	{
		if(driverSteeringWheel)
		{
			driverSteeringWheel.transform.localEulerAngles = new Vector3(0f, 0f, -(inputManager.steer * maxAngle));
		}
	}

	void UpdateWheelMeshsRotation()
	{
		Vector3 pos;
		Quaternion quat;

		for(int i = 0; i < wheelColliders.Length; i++)
		{
			wheelColliders[i].GetWorldPose(out pos, out quat);
			wheelMeshes[i].transform.position = pos;
			wheelMeshes[i].transform.rotation = quat;
		}
	}
}