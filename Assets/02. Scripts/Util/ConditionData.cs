using UnityEngine;

namespace Util
{
    [CreateAssetMenu(menuName = "Custom/Util/Condition")]
    public class ConditionData : ScriptableObject
    {
#if DEVELOPMENT
        [Header("Editor-Only Debug Text")] [SerializeField]
        string mCondition;
#endif
    }
}