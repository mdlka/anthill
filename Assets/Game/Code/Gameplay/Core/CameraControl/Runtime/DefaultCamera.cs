using System;
using UnityEngine;

namespace YellowSquad.Anthill.Core.CameraControl
{
    public class DefaultCamera : ICamera
    {
        private readonly Camera _camera;
        private readonly Bounds _cameraBounds;
        private readonly MinMaxFloat _zoomLimits;
        private readonly float _zoomSpeed;

        private float _currentZoom;

        public DefaultCamera(Camera camera, Bounds cameraBounds, MinMaxFloat zoomLimits, float zoomSpeed)
        {
            _camera = camera;
            _cameraBounds = cameraBounds;
            _zoomLimits = zoomLimits;
            _zoomSpeed = zoomSpeed;

            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _zoomLimits.Min, _zoomLimits.Max);
            _currentZoom = _camera.orthographicSize / _zoomLimits.Max;
        }
        
        public void Move(Vector2 delta)
        {
            Vector3 newCameraPosition = _camera.transform.position + new Vector3(delta.x, 0, delta.y);

            newCameraPosition.x = Mathf.Clamp(newCameraPosition.x, _cameraBounds.min.x, _cameraBounds.max.x);
            newCameraPosition.z = Mathf.Clamp(newCameraPosition.z, _cameraBounds.min.z, _cameraBounds.max.z);
            
            _camera.transform.position = newCameraPosition;
        }

        public void Zoom(float delta, Func<Vector2> cursorPosition)
        {
            var cursorWorldPositionBeforeZoom = _camera.ScreenToWorldPoint(cursorPosition.Invoke());
            
            _currentZoom = Mathf.Clamp01(_currentZoom + delta * _zoomSpeed);
            _camera.orthographicSize = Mathf.Lerp(_zoomLimits.Min, _zoomLimits.Max, _currentZoom);

            var cursorWorldPositionAfterZoom = _camera.ScreenToWorldPoint(cursorPosition.Invoke());
            var positionDiff = cursorWorldPositionBeforeZoom - cursorWorldPositionAfterZoom;

            var cameraPosition = _camera.transform.position;
            var targetPosition = new Vector3(cameraPosition.x + positionDiff.x, cameraPosition.y + positionDiff.y, cameraPosition.z);

            _camera.transform.position = targetPosition;
        }
    }
}
