using UnityEngine;

namespace YellowSquad.Anthill.Core.CameraControl
{
    [CreateAssetMenu(menuName = "Anthill/Camera/Create CameraSettings", fileName = "CameraSettings", order = 56)]
    public class CameraSettings : ScriptableObject
    {
        [SerializeField] private Bounds _bounds;
        [SerializeField] private MinMaxFloat _zoomLimits;
        [SerializeField] private float _zoomSpeed;
        
        public Bounds Bounds => _bounds;
        public MinMaxFloat ZoomLimits => _zoomLimits;
        public float ZoomSpeed => _zoomSpeed;

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_bounds.center, _bounds.extents);
        }
#endif
    }
}