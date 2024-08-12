using UnityEngine;

namespace YellowSquad.Utils
{
    internal class FaceToCamera : MonoBehaviour
    {
        [SerializeField] private bool _yRotate = true;
        
        private Transform _camera;

        private void Awake()
        {
            _camera = Camera.main.transform;
        }

        private void Update()
        {
            transform.forward = _camera.forward;

            if (_yRotate)
                return;
            
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, 0, 
                    transform.localRotation.eulerAngles.z);
        }
    }
}