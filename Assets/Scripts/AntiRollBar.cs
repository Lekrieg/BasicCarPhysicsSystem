using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
	// Change stiffness in the car to 1 to use this thing
    public WheelCollider rearWheelR;
	public WheelCollider rearWheelL;
	private Rigidbody carRigidbody;

	public float antiRoll = 5000f;
	// private bool isGrounded;

	void Start()
	{
		carRigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		WheelHit hit = new WheelHit();

		float travelL = 1.0f;
		float travelR = 1.0f;
		
		//Grounded(rearWheelL, hit, ref travelL);
		bool isGroundedL = rearWheelL.GetGroundHit(out hit);
		if(isGroundedL)
		{
			travelL = (-rearWheelL.transform.InverseTransformPoint(hit.point).y - rearWheelL.radius) /
						rearWheelL.suspensionDistance;
		}

		//Grounded(rearWheelR, hit, ref travelR);
		bool isGroundedR = rearWheelR.GetGroundHit(out hit);
		if(isGroundedR)
		{
			travelR = (-rearWheelR.transform.InverseTransformPoint(hit.point).y - rearWheelR.radius) /
						rearWheelR.suspensionDistance;
		}

		var antiRollForce = (travelL - travelR) * antiRoll;
		// AntiRollMethod(-antiRollForce, rearWheelL);
		if(isGroundedL)
		{
			carRigidbody.AddForceAtPosition(rearWheelL.transform.up * -antiRollForce, rearWheelL.transform.position);
		}
		// AntiRollMethod(antiRollForce, rearWheelR);
		if(isGroundedR)
		{
			carRigidbody.AddForceAtPosition(rearWheelR.transform.up * antiRollForce, rearWheelR.transform.position);
		}
	}

	/* private void Grounded(WheelCollider wheel, WheelHit hit, ref float travel)
	{
		isGrounded = wheel.GetGroundHit(out hit);

		if(isGrounded)
		{
			travel = (-wheel.transform.InverseTransformPoint(hit.point).y - wheel.radius) / wheel.suspensionDistance;
		}
	} */

	/* private void AntiRollMethod(float antiRoll, WheelCollider wheel)
	{
		if(isGrounded)
		{
			carRigidbody.AddForceAtPosition(wheel.transform.up * antiRoll, wheel.transform.position);
		}
	} */
}
