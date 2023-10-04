using UnityEditor;
using UnityEngine;

namespace YellowSquad.Core.HexMap.Editor
{
    [CustomEditor(typeof(CustomMapFactory))]
    public class CustomMapFactoryEditor : UnityEditor.Editor
    {
        private CustomMapFactory _customMapFactory;
        
        private void OnEnable()
        {
            _customMapFactory ??= target as CustomMapFactory;
            SceneView.duringSceneGui += OnDuringSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnDuringSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        private void OnDuringSceneGUI(SceneView sceneView)
        {
            Handles.DrawWireCube(Vector3.zero, Vector3.one * _customMapFactory.MapScale);
        }
    }
}
