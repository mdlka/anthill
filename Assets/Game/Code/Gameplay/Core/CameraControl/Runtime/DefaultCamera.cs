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
            var newCameraPosition = _camera.transform.position + new Vector3(delta.x, 0, delta.y);
            _camera.transform.position = ClampCameraPosition(newCameraPosition);
        }

        public void Zoom(float delta, Func<Vector2> cursorPosition)
        {
            var cursorWorldPositionBeforeZoom = _camera.ScreenToWorldPoint(cursorPosition.Invoke());
            
            _currentZoom = Mathf.Clamp01(_currentZoom + delta * _zoomSpeed);
            _camera.orthographicSize = Mathf.Lerp(_zoomLimits.Min, _zoomLimits.Max, _currentZoom);
            
            var cursorWorldPositionAfterZoom = _camera.ScreenToWorldPoint(cursorPosition.Invoke());
            
            _camera.transform.position = ClampCameraPosition(_camera.transform.position + cursorWorldPositionBeforeZoom - cursorWorldPositionAfterZoom);
        }

        private Vector3 ClampCameraPosition(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, _cameraBounds.min.x, _cameraBounds.max.x);
            position.z = Mathf.Clamp(position.z, _cameraBounds.min.z, _cameraBounds.max.z);

            return position;
        }
    }
}
