using PlatformGame.Util;
using System;
using UnityEngine;
using static PlatformGame.Character.Status.MovementInfo;

namespace PlatformGame.Character.Controller
{
    [RequireComponent(typeof(Character))]
    public class DefaultStateController : MonoBehaviour
    {
        const uint STATE_IDLE = 4290000000;
        const uint STATE_WALK = 4290000001;
        const uint STATE_RUNNING = 4290000002;
        const uint STATE_JUMPING = 4290000003;
        const uint STATE_FALLING = 4290000004;
        const uint STATE_LAND = 4290000005;
        Character mCharacter;

        void ReturnBasicState()
        {
            var velY = Math.Round(mCharacter.Rigid.velocity.y, 1);
            uint actionID;

            if (mCharacter.State is CharacterState.Attack)
            {
                mCharacter.SetAttackDelayState();
            }

            if (mCharacter.IsAction)
            {
                return;
            }

            else if (IsLandState())
            {
                actionID = STATE_LAND;
            }

            else if (IsJumpState())
            {
                actionID = (velY > 0) ? STATE_JUMPING : STATE_FALLING;
            }

            else
            {
                actionID = IsStopped() ? STATE_IDLE :
                    IsWalked() ? STATE_WALK :
                    STATE_RUNNING;
            }

            mCharacter.DoAction(actionID);
        }

        bool IsWalked()
        {
            return (mCharacter.Rigid.velocity.magnitude < MAX_RUN_VELOCITY);
        }

        bool IsStopped()
        {
            return (Mathf.Abs(mCharacter.Rigid.velocity.magnitude) < MIN_WALK_VELOCITY);
        }

        bool IsJumpState()
        {
            return !RigidbodyUtil.IsGrounded(mCharacter.Rigid) && !IsStopped();
        }

        bool IsLandState()
        {
            return RigidbodyUtil.IsGrounded(mCharacter.Rigid)
                   && mCharacter.State is CharacterState.Falling;
        }

        void Awake()
        {
            mCharacter = GetComponent<Character>();
        }

        void Update()
        {
            ReturnBasicState();
        }
    }
}