using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
	// Change stiffness in the car to 1 to use this thing
    public WheelCollider rearWheelR;
	public WheelCollider rearWheelL;
	public WheelCollider frontWheelR;
	public WheelCollider frontWheelL;

	private Rigidbody carRigidbody;

	public float antiRoll = 5000f;
	// private bool isGrounded;

	void Start()
	{
		carRigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		DoRollBar(rearWheelL, rearWheelR);
		DoRollBar(frontWheelL, frontWheelR);
	}

	void DoRollBar(WheelCollider wheelL, WheelCollider wheelR)
	{
		WheelHit hit = new WheelHit();

		float travelL = 1.0f;
		float travelR = 1.0f;
		
		//Grounded(rearWheelL, hit, ref travelL);
		bool isGroundedL = wheelL.GetGroundHit(out hit);
		if(isGroundedL)
		{
			travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) /
						wheelL.suspensionDistance;
		}

		//Grounded(rearWheelR, hit, ref travelR);
		bool isGroundedR = wheelR.GetGroundHit(out hit);
		if(isGroundedR)
		{
			travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) /
						wheelR.suspensionDistance;
		}

		var antiRollForce = (travelL - travelR) * antiRoll;
		if(isGroundedL)
		{
			carRigidbody.AddForceAtPosition(wheelL.transform.up * -antiRollForce, wheelL.transform.position);
		}
		if(isGroundedR)
		{
			carRigidbody.AddForceAtPosition(wheelR.transform.up * antiRollForce, wheelR.transform.position);
		}
	}
}
