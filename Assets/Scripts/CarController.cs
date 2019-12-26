using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class CarController : MonoBehaviour
{
	public InputManager inputManager;
	public UIManager uiManager;

	public List<WheelCollider> throttleWheels;
	public List<GameObject> steeringWheels;
	public List<GameObject> wheelMeshes;
	public GameObject driverSteeringWheel;

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

	void Steer()
	{
		foreach (GameObject wheel in steeringWheels)
		{
			wheel.GetComponent<WheelCollider>().steerAngle = maxAngle * inputManager.steer;
			wheel.transform.localEulerAngles = new Vector3(0f, inputManager.steer * maxAngle, 0f);
		}
	}
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
	private float m_horizontalInput;
	private float m_verticalInput;
	// Vou usar isso depois no IK controller, tentar pelo menos
	private float m_steeringAngle;

	// transformar em private depois
	public WheelCollider frontDriverW, frontPassengerW;
	public WheelCollider rearDriverW, rearPassengerW;
	public Transform frontDriverT, frontPassengerT;
	public Transform rearDriverT, rearPassengerT;
	[Header("Other things")]
	public GameObject steeringWheel;
	public float maxSteeringAngle = 30; // Angulo maximo da roda é 30 inicialmente
	public float motorForce = 50;

	private void FixedUpdate()
	{
		GetInput();
		Steer();
		TurnSteeringWheel();
		Accelerate();
		UpdateWheelPoses();
	}

	public void GetInput()
	{
		m_horizontalInput = Input.GetAxis("Steering");
		m_verticalInput = Input.GetAxis("Acceleration");
	}

	public void TurnSteeringWheel()
	{
		steeringWheel.transform.localEulerAngles = new Vector3(0, 0, -m_steeringAngle);
	}

	private void Steer()
	{
		m_steeringAngle = maxSteeringAngle * m_horizontalInput;
		frontDriverW.steerAngle = m_steeringAngle;
		frontPassengerW.steerAngle = m_steeringAngle;
	}

	private void Accelerate()
	{
		frontDriverW.motorTorque = m_verticalInput * motorForce;
		frontPassengerW.motorTorque = m_verticalInput * motorForce;
	}

	private void UpdateWheelPoses()
	{
		UpdateWheelPose(frontDriverW, frontDriverT);		
		UpdateWheelPose(frontPassengerW, frontPassengerT);		
		UpdateWheelPose(rearDriverW, rearDriverT);		
		UpdateWheelPose(rearPassengerW, rearPassengerT);		
	}

	private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
	{
		Vector3 _pos = _transform.position;
		Quaternion _quat = _transform.rotation;

		_collider.GetWorldPose(out _pos, out _quat);

		_transform.position = _pos;
		_transform.rotation = _quat;
	}
*/
