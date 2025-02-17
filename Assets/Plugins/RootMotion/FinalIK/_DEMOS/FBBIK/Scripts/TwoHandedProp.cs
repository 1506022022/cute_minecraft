﻿using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{

    // Targeting hands to 2-handed props with the FullBodyBipedIK component. It is not good to parent IK targets directly to the bones that use IK (circular dependency).
    // The bones are moved in the solving process, but it will not update the IK targets parented to the bones. All IK target positions/rotations need to be set before the solver updates.
    public class TwoHandedProp : MonoBehaviour
    {

        [Range(0f, 1f)] public float weight = 1f;
        [Tooltip("The left hand target parented to the right hand.")] public Transform leftHandTarget;
        [Tooltip("Left hand poser (poses fingers to match the left hand target).")] public Poser leftHandPoser;
        [Tooltip("The weight of pinning the left hand to the prop.")][Range(0f, 1f)] public float leftHandWeight = 1f;

        private FullBodyBipedIK ik;
        private Vector3 targetPosRelativeToRight;
        private Quaternion targetRotRelativeToRight;

        void Start()
        {
            ik = GetComponent<FullBodyBipedIK>();

            // Get a call from FBBIK each time it has finished updating
            ik.solver.OnPostUpdate += AfterFBBIK;

            if (ik.solver.rightHandEffector.target == null) Debug.LogError("Right Hand Effector needs a Target in this demo.");
        }

        void LateUpdate()
        {
            // Get the position/rotation of the left hand target relative to the right hand.
            targetPosRelativeToRight = ik.references.rightHand.InverseTransformPoint(leftHandTarget.position);
            targetRotRelativeToRight = Quaternion.Inverse(ik.references.rightHand.rotation) * leftHandTarget.rotation;

            // Set the position/rotation of the left hand target relative to the right hand effector target.
            ik.solver.leftHandEffector.position = ik.solver.rightHandEffector.target.position + ik.solver.rightHandEffector.target.rotation * targetPosRelativeToRight;
            ik.solver.leftHandEffector.rotation = ik.solver.rightHandEffector.target.rotation * targetRotRelativeToRight;

            // Weights
            ik.solver.rightHandEffector.positionWeight = weight;

            float wL = leftHandWeight * weight;
            ik.solver.leftHandEffector.positionWeight = wL;
            leftHandPoser.weight = wL;
        }

        // Called by FBBIK after it updates
        void AfterFBBIK()
        {
            // Rotate the hand bones to effector.rotation directly instead of using effector.rotationWeight that might fail to get the limb bending right under some circumstances
            ik.solver.leftHandEffector.bone.rotation = Quaternion.Slerp(ik.solver.leftHandEffector.bone.rotation, ik.solver.leftHandEffector.rotation, leftHandWeight * weight);

            ik.solver.rightHandEffector.bone.rotation = Quaternion.Slerp(ik.solver.rightHandEffector.bone.rotation, ik.solver.rightHandEffector.rotation, weight);
        }

        // Clean up the delegate
        void OnDestroy()
        {
            if (ik != null) ik.solver.OnPostUpdate -= AfterFBBIK;
        }
    }
}
