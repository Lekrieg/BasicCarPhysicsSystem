using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class CarController : MonoBehaviour
{
	// TODO: Move the  object that the IK is looking for, varies between -0.4f, 0f, 0.4f

	public enum DriveMode { Front, Rear, All };
	public DriveMode driveMode = DriveMode.All;

	[Header("Some other things")]
	public GameObject headDirection;
	private GameObject carSeat;
	private GameObject playerPrefabModel;

	[Header("Managers")]
	public InputManager inputManager;
	public UIManager uiManager;

	[Header("Wheel colliders")]
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
	float angleInput;
	public float torque = 1000f;
	public float brakeTorque = 400.0f;
	public float friction = 25f;

	public Transform centerOfMass;
	public Rigidbody carRigidbody;

	private void Awake()
	{
		carSeat = GameObject.Find("Seat");

		GameObject playerModel = Resources.Load<GameObject>("Prefabs/Haruko");
		playerPrefabModel = Instantiate(playerModel, carSeat.transform);

		playerPrefabModel.transform.parent = carSeat.transform;
	}

	void Start()
	{
		inputManager = GetComponent<InputManager>();

		if (centerOfMass)
		{
			carRigidbody.centerOfMass = centerOfMass.localPosition;
		}
	}

	void Update()
	{
		AnimateBrakePedal();
		TurnSteeringWheel();
		TurnHead();
		UpdateWheelMeshsRotation();
	}

	void FixedUpdate()
	{
		uiManager.UpdateSpeedometer(GetSpeed());
		Accelerate();
		Steer();
	}

	public float GetSpeed()
	{
		return wheelColliders[0].radius * Mathf.PI *
			   wheelColliders[0].rpm * 60f / 1000f;
	}

	void Accelerate()
	{
		float scaledTorque = inputManager.throttle * torque;

		if (Mathf.Abs(scaledTorque) > 0)
		{
			AnimateAccelerationPedal();
		}

		if (wheelColliders[0].rpm < idealRPM)
		{
			scaledTorque = Mathf.Lerp(scaledTorque / 5f, scaledTorque, wheelColliders[0].rpm / idealRPM);
		}
		else
		{
			scaledTorque = Mathf.Lerp(scaledTorque, 0, (wheelColliders[0].rpm - idealRPM) / (maxRPM - idealRPM));
		}

		for (int i = 0; i < wheelColliders.Length; i++)
		{
			wheelColliders[i].motorTorque = driveMode == DriveMode.Rear ? 0 : scaledTorque;

			wheelColliders[i].brakeTorque = inputManager.brake ? brakeTorque : friction - Mathf.Abs(inputManager.throttle * friction);
		}
	}
	void AnimateAccelerationPedal()
	{
		// pedal rot * dir• amounttomovemax•accelAxis
		if (accelerationPedal)
		{
			float angle = Mathf.Abs(-30f * inputManager.throttle);

			accelerationPedal.transform.localRotation = Quaternion.Euler(angle, 0f, 0f);
		}
	}

	float brakePedalAngle;
	void AnimateBrakePedal()
	{
		if (brakePedal)
		{
			if (inputManager.brake)
			{
				brakePedalAngle = Mathf.Lerp(brakePedalAngle, 1, Time.deltaTime * 4);
			}
			else
			{
				brakePedalAngle = Mathf.Lerp(brakePedalAngle, 0, Time.deltaTime * 2);
			}

			brakePedal.transform.localRotation = Quaternion.Euler(brakePedalAngle * maxAngle, 0f, 0f);
		}
	}

	void Steer()
	{
		if (inputManager.steer > 0.7f || inputManager.steer < -0.7f)
		{
			angleInput = Mathf.Lerp(angleInput, inputManager.steer, Time.deltaTime * 4);
		}
		else
		{
			angleInput = Mathf.Lerp(angleInput, inputManager.steer, Time.deltaTime * 2);
		}

		// Left
		wheelColliders[0].steerAngle = angleInput * maxAngle;

		// Right
		wheelColliders[1].steerAngle = angleInput * maxAngle;
	}
	// Rotacao do pedal * angulo maximo de movimentacao * axisAcceleration
	public void TurnSteeringWheel()
	{
		if (driverSteeringWheel)
		{
			driverSteeringWheel.transform.localEulerAngles = new Vector3(0f, 0f, -(angleInput * maxAngle));
		}
	}
	void TurnHead()
	{
		if (headDirection)
		{
			float viewDirection = inputManager.steer * 0.5f;
			// usar um lerp da posicao atual ate a direcao especifica
			headDirection.transform.localPosition = new Vector3(viewDirection, 0f, 0f);
		}
	}

	void UpdateWheelMeshsRotation()
	{
		Vector3 pos;
		Quaternion quat;

		for (int i = 0; i < wheelColliders.Length; i++)
		{
			wheelColliders[i].GetWorldPose(out pos, out quat);
			wheelMeshes[i].transform.position = pos;
			wheelMeshes[i].transform.rotation = quat;
		}
	}
}