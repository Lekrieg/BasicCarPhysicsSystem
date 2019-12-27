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
	public GameObject wheelColliderFL;
	public GameObject wheelColliderFR;
	public GameObject wheelColliderRL;
	public GameObject wheelColliderRR;

	[Header("Wheel meshes")]
	public List<GameObject> wheelMeshes;

	[Header("SteeringWheel and pedals")]
	public GameObject driverSteeringWheel;
	public GameObject accelerationPedal;

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
		//UpdateWheelMeshsRotation();
	}

	public float GetSpeed()
	{
		return wheelColliderRR.GetComponent<WheelCollider>().radius * Mathf.PI *
			   wheelColliderRR.GetComponent<WheelCollider>().rpm * 60f/1000f;
	}

	void Accelerate()
	{
		float scaledTorque = inputManager.throttle * torque;

		if(wheelColliderRL.GetComponent<WheelCollider>().rpm < idealRPM)
		{
			scaledTorque = Mathf.Lerp(scaledTorque/10f, scaledTorque, wheelColliderRL.GetComponent<WheelCollider>().rpm / idealRPM);
		}
		else
		{
			scaledTorque = Mathf.Lerp(scaledTorque, 0, (wheelColliderRL.GetComponent<WheelCollider>().rpm - idealRPM) / (maxRPM - idealRPM));
		}

		wheelColliderFL.GetComponent<WheelCollider>().motorTorque = driveMode == DriveMode.Rear ? 0 : scaledTorque;
		wheelColliderFR.GetComponent<WheelCollider>().motorTorque = driveMode == DriveMode.Rear ? 0 : scaledTorque;
		wheelColliderRL.GetComponent<WheelCollider>().motorTorque = driveMode == DriveMode.Front ? 0 : scaledTorque;
		wheelColliderRR.GetComponent<WheelCollider>().motorTorque = driveMode == DriveMode.Front ? 0 : scaledTorque;

		if(inputManager.brake)
		{
			wheelColliderFL.GetComponent<WheelCollider>().brakeTorque = brakeTorque;
			wheelColliderFR.GetComponent<WheelCollider>().brakeTorque = brakeTorque;
			wheelColliderRL.GetComponent<WheelCollider>().brakeTorque = brakeTorque;
			wheelColliderRR.GetComponent<WheelCollider>().brakeTorque = brakeTorque;
		}
		else
		{
			wheelColliderFL.GetComponent<WheelCollider>().brakeTorque = 0;
			wheelColliderFR.GetComponent<WheelCollider>().brakeTorque = 0;
			wheelColliderRL.GetComponent<WheelCollider>().brakeTorque = 0;
			wheelColliderRR.GetComponent<WheelCollider>().brakeTorque = 0;
		}
	}
	void AnimatePedal(GameObject pedal)
	{
		
	}

	void Steer()
	{
		wheelColliderFL.GetComponent<WheelCollider>().steerAngle = maxAngle * inputManager.steer;
		wheelColliderFL.GetComponent<WheelCollider>().transform.localEulerAngles = new Vector3(0f, inputManager.steer * maxAngle, 0f);

		wheelColliderFR.GetComponent<WheelCollider>().steerAngle = maxAngle * inputManager.steer;
		wheelColliderFR.GetComponent<WheelCollider>().transform.localEulerAngles = new Vector3(0f, inputManager.steer * maxAngle, 0f);
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
		foreach (GameObject meshItem in wheelMeshes)
		{
			meshItem.transform.Rotate(carRigidbody.velocity.magnitude *
									 (transform.InverseTransformDirection(carRigidbody.velocity).z >= 0 ? 1 : -1) /
									 (2 * Mathf.PI * 0.33f), 0f, 0f);
		}
	}
}

/*
	public InputManager inputManager;
	public UIManager uiManager;

	public List<WheelCollider> throttleWheels;
	public List<GameObject> steeringWheels;
	public List<GameObject> wheelMeshes;
	public GameObject driverSteeringWheel;

	public GameObject accelerationPedal;

	public float maxAngle = 30f;
	public float motorForce = 10000f;
	public float brakeForce;

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

	void Update()
	{
		uiManager.UpdateSpeedometer(transform.InverseTransformVector(carRigidbody.velocity).z);
	}

	void FixedUpdate()
	{
		Accelerate();
		Steer();
		TurnSteeringWheel();
		UpdateWheelMeshsRotation();
	}

	void Accelerate()
	{
		AnimatePedal(accelerationPedal);

		foreach (WheelCollider wheel in throttleWheels)
		{
			if(inputManager.brake)
			{
				wheel.motorTorque = 0f;
				wheel.brakeTorque = brakeForce * Time.deltaTime;
			}
			else
			{
				wheel.motorTorque = motorForce * Time.deltaTime * inputManager.throttle;
				wheel.brakeTorque = 0f;
			}
		}
	}
	void AnimatePedal(GameObject pedal)
	{
		
	}

	void Steer()
	{
		foreach (GameObject wheel in steeringWheels)
		{
			wheel.GetComponent<WheelCollider>().steerAngle = maxAngle * inputManager.steer;
			wheel.transform.localEulerAngles = new Vector3(0f, inputManager.steer * maxAngle, 0f);
		}
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
		foreach (GameObject meshItem in wheelMeshes)
		{
			meshItem.transform.Rotate(carRigidbody.velocity.magnitude *
									 (transform.InverseTransformDirection(carRigidbody.velocity).z >= 0 ? 1 : -1) /
									 (2 * Mathf.PI * 0.33f), 0f, 0f);
		}
	}
*/
