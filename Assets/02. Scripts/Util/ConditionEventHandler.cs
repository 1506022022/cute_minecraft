using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class ConditionEventHandler : MonoBehaviour
    {
        readonly Dictionary<ConditionData, bool> mConditionStatusMap = new();
        public UnityEvent CompletionEvent;
        public UnityEvent ResetAllEvetn;

        public void AddCondition(ConditionData condition)
        {
            if (mConditionStatusMap.TryAdd(condition, false))
            {
                return;
            }

            Debug.Log($"Conditions already added : {condition}");
            return;
        }

        public void RemoveCondition(ConditionData condition)
        {
            if (!mConditionStatusMap.ContainsKey(condition))
            {
                Debug.Log($"Conditions that don't exist : {condition}");
                return;
            }

            mConditionStatusMap.Remove(condition);
        }

        public void MeetCondition(ConditionData condition)
        {
            if (!mConditionStatusMap.ContainsKey(condition))
            {
                Debug.Log($"Conditions that don't exist : {condition}");
                return;
            }

            mConditionStatusMap[condition] = true;

            if (mConditionStatusMap.All(x => x.Value))
            {
                CompletionEvent.Invoke();
            }
        }

        public void CancleCondition(ConditionData condition)
        {
            if (!mConditionStatusMap.ContainsKey(condition))
            {
                Debug.Log($"Conditions that don't exist : {condition}");
                return;
            }

            mConditionStatusMap[condition] = false;

            if (mConditionStatusMap.All(x => !x.Value))
            {
                ResetAllEvetn.Invoke();
            }
        }

        public void ToggleCondition(ConditionData condition)
        {
            if (!mConditionStatusMap.ContainsKey(condition))
            {
                Debug.Log($"Conditions that don't exist : {condition}");
                return;
            }

            if (mConditionStatusMap[condition])
            {
                CancleCondition(condition);
            }
            else
            {
                MeetCondition(condition);
            }
        }

        public void ResetAllConditions()
        {
            var keys = mConditionStatusMap.Keys.ToList();
            foreach (var key in keys)
            {
                mConditionStatusMap[key] = false;
            }

            ResetAllEvetn.Invoke();
        }
    }
}