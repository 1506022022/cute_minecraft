#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Puzzle
{

    [CustomEditor(typeof(CubePuzzleComponent))]
    public class CubePuzzleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.BeginHorizontal();
            GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>("Assets/10. Editor/Image/�ֻ���.png"));
            GUILayout.EndHorizontal();
        }
    }

}
#endif