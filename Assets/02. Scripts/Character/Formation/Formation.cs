using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Character
{
    [Serializable]
    public class Formation : MonoBehaviour
    {
        public Transform objectA; // A ������Ʈ
        public Transform ToB; // B ������Ʈ
        Vector3 OriginPos;
        // �̵� �и�
        public float time = 0f;
        public float Duration = 10f;
        private Vector3 controlPoint; // ������
        public AnimationCurve curve;
        public UnityEvent OnFormationChanged;

        bool mbFormationChange;
        bool mbEnter;

        void FixedUpdate()
        {
            if (Vector3.Distance(objectA.position, ToB.position) < 0.1f)
            {
                if(mbEnter)
                {
                    return;
                }
                mbEnter = true;

                if (mbFormationChange)
                {
                    OnFormationChanged.Invoke();
                    mbFormationChange = false;
                }

                time = Duration;
                return;
            }

            if (mbEnter)
            {
                mbEnter = false;
            }

            if (time == Duration)
            {
                time = 0;
                controlPoint = (objectA.position + ToB.position) / 2f;
            }

            time = Mathf.Clamp(time + Time.deltaTime, 0, Duration);
            Vector3 bezierPosition = CalculateBezierPoint(objectA.position, controlPoint, ToB.position, (time * curve.Evaluate(time / Duration)) / Duration);
            objectA.position = bezierPosition;
        }

        public void PomationByState(CharacterState state)
        {
            if (state is CharacterState.Jumping or CharacterState.Falling)
            {
                SwapPomation(new Vector3(0, -2, 0), 0.5f);
            }
            else
            {
                PomationOrigin();
            }
        }

        public void SwapPomation(Vector3 pos, float duration)
        {
            ToB.localPosition = pos;
            time = 0;
            controlPoint = (objectA.position + ToB.position) / 2f;
            Duration = duration;
            mbFormationChange = true;
        }
        public void PomationOrigin()
        {
            SwapPomation(OriginPos, 3f);
        }

        // ������ � ��� �Լ�
        private Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 point = uu * p0; // ������
            point += 2 * u * t * p1; // ������
            point += tt * p2; // ����

            return point;
        }

        void Awake()
        {
            OriginPos = transform.localPosition;
        }
    }

}

