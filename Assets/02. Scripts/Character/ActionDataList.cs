using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "ActionDataList", menuName = "Custom/ActionDataList")]
    public class ActionDataList : ScriptableObject
    {
        [SerializeField] List<ActionData> mActions;

        public List<ActionData> Actions
        {
            get
            {
                Debug.Assert(mActions is { Count: > 0 });
                return mActions;
            }
        }

        public Dictionary<int, ActionData> Library
        {
            get
            {
                var library = new Dictionary<int, ActionData>();
                foreach (var item in mActions)
                {
                    Debug.Assert(!library.ContainsKey(item.ID), $"Duplicate values : {item.ID}");
                    library.Add(item.ID, item);
                }

                return library;
            }
        }
    }
}