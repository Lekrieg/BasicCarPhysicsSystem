using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{
    protected Animator animator;

	public bool ikActive = false;
	public Transform rightHand = null;
	public Transform leftHand = null;
	public Transform rightFoot = null;
	public Transform leftFoot = null;

	public Transform lookObj = null;

	void Start()
	{
		animator = GetComponent<Animator>();
	}

	// callback for calculating IK
	void OnAnimatorIK()
	{
		if(animator)
		{
			
			// set the look transform position, if one has been assigned
			if(lookObj != null)
			{
				LookAtAuxMethod();
			}

			// set the right hand target position and rotation, if one has been assigned
			if(rightHand != null)
			{
				IKAuxMethod(AvatarIKGoal.RightHand, rightHand);
			}
			if(leftHand != null)
			{
				IKAuxMethod(AvatarIKGoal.LeftHand, leftHand);
			}
			
			if(rightFoot != null)
			{
				IKAuxMethod(AvatarIKGoal.RightFoot, rightFoot);
			}
			if(leftFoot != null)
			{
				IKAuxMethod(AvatarIKGoal.LeftFoot, leftFoot);
			}
		}
	}

	private void LookAtAuxMethod()
	{
		// if the IK is active, set the position and rotation directly to the goal.
		if(ikActive)
		{
			animator.SetLookAtWeight(1);
			animator.SetLookAtPosition(lookObj.position);
		}
		else
		{
			animator.SetLookAtWeight(0);
		}
	}

	private void IKAuxMethod(AvatarIKGoal avatarIK, Transform objTransform)
	{
		// if the IK is active, set the position and rotation directly to the goal.
		if(ikActive)
		{
			animator.SetIKPositionWeight(avatarIK, 1);
			animator.SetIKRotationWeight(avatarIK, 1);

			animator.SetIKPosition(avatarIK, objTransform.position);
			animator.SetIKRotation(avatarIK, objTransform.rotation);
		}
		else
		{
			animator.SetIKPositionWeight(avatarIK, 0);
			animator.SetIKRotationWeight(avatarIK, 0);
		}
	}
}