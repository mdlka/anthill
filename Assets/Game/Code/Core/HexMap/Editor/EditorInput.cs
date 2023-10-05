using UnityEditor;
using UnityEngine;

namespace YellowSquad.Core.HexMap.Editor
{
    internal class EditorInput
    {
        public Vector3 MousePosition
        {
            get
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                Vector3 normal = Vector3.up;
                float distance = -Vector3.Dot(normal, ray.origin) / Vector3.Dot(normal, ray.direction);
            
                return ray.origin + ray.direction * distance;
            }
        }
    }
}