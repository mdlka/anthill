using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap.Editor
{
    [CustomEditor(typeof(CustomMapFactory))]
    public class CustomMapFactoryEditor : UnityEditor.Editor
    {
        private EditorInput _editorInput;
        private CustomMapFactory _customMapFactory;
        private EditState _currentEditState;

        private void OnEnable()
        {
            _editorInput ??= new EditorInput();
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
            
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add hex", ButtonStyle(EditState.Add)))
                _currentEditState = EditState.Add;

            if (GUILayout.Button("Remove hex", ButtonStyle(EditState.Remove)))
                _currentEditState = EditState.Remove;
            
            GUILayout.EndHorizontal();
            
            if (_currentEditState != EditState.Disable)
                if (GUILayout.Button("Disable edit mode"))
                    _currentEditState = EditState.Disable;
        }

        private void OnDuringSceneGUI(SceneView sceneView)
        {
            foreach (var hex in _customMapFactory.Hexes)
            {
                DrawHex(hex.Position, Color.green);
                DrawText($"{hex.Position.ToString()}\nEmpty: {hex.Empty}\n{hex.Hardness}\n{hex.PointOfInterestType.ToString()}", hex.Position.ToVector3(_customMapFactory.MapScale));
            }

            Tools.hidden = true;
            Event currentEvent = Event.current;
            
            if (currentEvent.isMouse && currentEvent.button != 0)
                return;

            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            if (_currentEditState != EditState.Disable)
                DrawText(_editorInput.MousePosition.ToAxialCoordinate(_customMapFactory.MapScale).ToString(), _editorInput.MousePosition + new Vector3(0.25f, 0));
            
            if (currentEvent.GetTypeForControl(controlID) == EventType.MouseDown)
            {
                GUIUtility.hotControl = controlID;

                var targetPosition = _editorInput.MousePosition.ToAxialCoordinate(_customMapFactory.MapScale);

                try
                {
                    if (_currentEditState == EditState.Add)
                    {
                        if (_customMapFactory.HasHexIn(targetPosition))
                            _customMapFactory.RemoveHex(targetPosition);
                        
                        _customMapFactory.AddHex(targetPosition);
                    }
                    else if (_currentEditState == EditState.Remove)
                    {
                        _customMapFactory.RemoveHex(targetPosition);
                    }
                        
                }
                catch (Exception) { /* ignored */ }

                currentEvent.Use();
            }

            HandleUtility.Repaint();
        }

        private void DrawHex(AxialCoordinate position, Color color)
        {
            var oldColor = Handles.color;
            Handles.color = color;
            
            for (int i = 0; i < 6; i++)
            {
                var startPoint =  position.HexCornerPosition(i, _customMapFactory.MapScale);
                var endPoint =  position.HexCornerPosition(i + 1, _customMapFactory.MapScale);
                
                Handles.DrawLine(startPoint, endPoint);
            }
            
            Handles.DrawSolidDisc(position.ToVector3(_customMapFactory.MapScale), Vector3.up, _customMapFactory.MapScale * 0.25f);

            Handles.color = oldColor;
        }

        private void DrawText(string text, Vector3 position, FontStyle fontStyle = FontStyle.Bold)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = Mathf.Clamp((int)(100 / HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.y 
                                                    * _customMapFactory.MapScale), 5, 12);
            labelStyle.fontStyle = fontStyle;
                
            Handles.Label(position, text, labelStyle);
        }

        private GUIStyle ButtonStyle(EditState state)
        {
            GUIStyle normalButtonStyle = new GUIStyle(GUI.skin.button);
            GUIStyle selectedButtonStyle = new GUIStyle(GUI.skin.button) 
                { normal = { background = Texture2D.grayTexture } };

            return _currentEditState == state ? selectedButtonStyle : normalButtonStyle;
        }

        private enum EditState
        {
            Disable,
            Add,
            Remove,
        }
    }
}
