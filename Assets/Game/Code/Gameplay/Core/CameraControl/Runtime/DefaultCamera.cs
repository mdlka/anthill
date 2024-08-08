using System;
using UnityEngine;
using YellowSquad.Utils;

namespace YellowSquad.Anthill.Core.CameraControl
{
    public class DefaultCamera : ICamera
    {
        private readonly Camera _camera;
        private readonly CameraSettings _settings;
        private readonly float _defaultPositionY;

        private Vector3 _startMovePosition;
        private float _currentZoom;

        public DefaultCamera(Camera camera, CameraSettings settings)
        {
            _camera = camera;
            _settings = settings;

            _camera.fieldOfView = _settings.ZoomLimits.Clamp(_camera.fieldOfView);
            _currentZoom = _camera.fieldOfView / _settings.ZoomLimits.Max;
            _defaultPositionY = _camera.transform.position.y;
        }

        public Vector3 Position => _camera.transform.position;
        public float NormalizedZoom => _currentZoom / _settings.ZoomLimits.Max;

        public void StartMove(Vector2 pointerPosition)
        {
            _startMovePosition = ScreenToWorldPoint(pointerPosition);
        }

        public void Move(Vector2 pointerPosition)
        {
            var moveOffset = _startMovePosition - ScreenToWorldPoint(pointerPosition);
            _camera.transform.position = ClampCameraPosition(_camera.transform.position + moveOffset);
        }

        public void Zoom(float delta, Func<Vector2> pointerPosition)
        {
            var cursorWorldPositionBeforeZoom = ScreenToWorldPoint(pointerPosition.Invoke());
            
            _currentZoom = Mathf.Clamp01(_currentZoom + delta * _settings.ZoomSpeed);
            _camera.fieldOfView = Mathf.Lerp(_settings.ZoomLimits.Min, _settings.ZoomLimits.Max, _currentZoom);
            
            var cursorWorldPositionAfterZoom = ScreenToWorldPoint(pointerPosition.Invoke());
            
            _camera.transform.position = ClampCameraPosition(_camera.transform.position + cursorWorldPositionBeforeZoom - cursorWorldPositionAfterZoom);
        }

        public Vector3 ScreenToWorldPoint(Vector3 position)
        {
            position.z = _camera.transform.position.y;
            return _camera.ScreenToWorldPoint(position);
        }

        private Vector3 ClampCameraPosition(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, _settings.Bounds.min.x, _settings.Bounds.max.x);
            position.z = Mathf.Clamp(position.z, _settings.Bounds.min.z, _settings.Bounds.max.z);
            position.y = _defaultPositionY;

            return position;
        }
    }
}
