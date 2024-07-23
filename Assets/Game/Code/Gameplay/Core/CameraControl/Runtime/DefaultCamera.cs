using System;
using UnityEngine;

namespace YellowSquad.Anthill.Core.CameraControl
{
    public class DefaultCamera : ICamera
    {
        private const float SizeFactor = 0.1f * 0.2f;
        
        private readonly Camera _camera;
        private readonly CameraSettings _settings;

        private float _currentZoom;

        public DefaultCamera(Camera camera, CameraSettings settings)
        {
            _camera = camera;
            _settings = settings;

            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _settings.ZoomLimits.Min, _settings.ZoomLimits.Max);
            _currentZoom = _camera.orthographicSize / _settings.ZoomLimits.Max;
        }

        public Vector3 Position => _camera.transform.position;

        public void Move(Vector2 delta)
        {
            var modifiedDelta = new Vector3(delta.x, 0, delta.y) * (_camera.aspect * _camera.orthographicSize * SizeFactor);
            _camera.transform.position = ClampCameraPosition(_camera.transform.position + modifiedDelta);
        }

        public void Zoom(float delta, Func<Vector2> cursorPosition)
        {
            var cursorWorldPositionBeforeZoom = _camera.ScreenToWorldPoint(cursorPosition.Invoke());
            
            _currentZoom = Mathf.Clamp01(_currentZoom + delta * _settings.ZoomSpeed);
            _camera.orthographicSize = Mathf.Lerp(_settings.ZoomLimits.Min, _settings.ZoomLimits.Max, _currentZoom);
            
            var cursorWorldPositionAfterZoom = _camera.ScreenToWorldPoint(cursorPosition.Invoke());
            
            _camera.transform.position = ClampCameraPosition(_camera.transform.position + cursorWorldPositionBeforeZoom - cursorWorldPositionAfterZoom);
        }

        public Vector3 ScreenToWorldPoint(Vector3 position)
        {
            return _camera.ScreenToWorldPoint(position);
        }

        private Vector3 ClampCameraPosition(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, _settings.Bounds.min.x, _settings.Bounds.max.x);
            position.z = Mathf.Clamp(position.z, _settings.Bounds.min.z, _settings.Bounds.max.z);

            return position;
        }
    }
}
