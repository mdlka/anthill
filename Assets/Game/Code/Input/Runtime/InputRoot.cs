using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using YellowSquad.Anthill.Core.CameraControl;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.GameLoop;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.UserInput
{
    public class InputRoot : IGameLoop
    {
        private const float MoveThreshold = 0.1f;
        private const float PointerClickMaxDelta = 0.1f;
        private const float HexHeight = 0.8f;

        private readonly IHexMap _map;
        private readonly IInput _input;
        private readonly ICamera _camera;
        private readonly IClickCommand[] _clickCommands;
        private readonly List<RaycastResult> _raycastResults = new();

        private readonly Dictionary<int, PointerDownInfo> _pointersDown = new();
        private readonly List<int> _pointers = new();

        private int _lastPointerId;
        private float _lastPointerUpTime;
        private bool _cameraMoving;
        private bool _hasFocus = true;
        
        private Plane _lowerPlane;
        private Plane _upperPlane;

        public InputRoot(IHexMap map, IInput input, ICamera camera, IClickCommand[] commands)
        {
            _map = map;
            _input = input;
            _camera = camera;
            _clickCommands = commands;
            _pointersDown[0] = default;

            _lowerPlane = new Plane(Vector3.up, 0f);
            _upperPlane = new Plane(Vector3.up, -HexHeight);
        }

        private int CurrentPointerId => _pointers.Count > 0 ? _pointers[0] : _lastPointerId;

        public void Update(float deltaTime)
        {
            if (_hasFocus == false)
                return;
            
            _input.Update();
            
            float zoomDelta = _input.ZoomDelta;
            
            if (zoomDelta != 0)
                _camera.Zoom(-zoomDelta, () => _input.PointerPosition(CurrentPointerId));
            
            if (_input.HasPointerDown)
                OnPointerDown(_input.ReadPointerDown());
            
            OnPointerMove();
            
            if (_input.HasPointerUp)
                OnPointerUp(_input.ReadPointerUp());
        }

        public void LostFocus()
        {
            _hasFocus = false;

            _pointers.Clear();
            _cameraMoving = false;
            _lastPointerUpTime = Time.realtimeSinceStartup;
        }

        public void ReturnFocus()
        {
            _hasFocus = true;
        }

        private void OnPointerDown(int pointerId)
        {
            _pointersDown[pointerId] = new PointerDownInfo
            {
                Time = Time.realtimeSinceStartup,
                Position = _input.PointerPosition(pointerId),
            };
            
            if (_pointers.Contains(pointerId) == false)
                _pointers.Add(pointerId);
        }

        private void OnPointerMove()
        {
            if (_pointersDown[CurrentPointerId].Time - _lastPointerUpTime < MoveThreshold)
                return;
            
            if (_cameraMoving == false)
                _camera.StartMove(_input.PointerPosition(CurrentPointerId));

            _camera.Move(_input.PointerPosition(CurrentPointerId));
            _cameraMoving = true;
        }

        private void OnPointerUp(int pointerId)
        {
            _lastPointerId = pointerId;
            _pointers.Remove(pointerId);

            if (_pointers.Count == 0)
            {
                _lastPointerUpTime = Time.realtimeSinceStartup;
                
                if (_lastPointerUpTime - _pointersDown[pointerId].Time < MoveThreshold 
                    || Vector2.Distance(_pointersDown[pointerId].Position, _input.PointerPosition(pointerId)) <= PointerClickMaxDelta)
                    OnPointerClick(_input.PointerPosition(pointerId));
                
                _cameraMoving = false;
            }
            else
            {
                _camera.StartMove(_input.PointerPosition(CurrentPointerId));
            }
        }

        private void OnPointerClick(Vector2 pointerPosition)
        {
            var clickPosition = new Vector3(pointerPosition.x, pointerPosition.y, _camera.Position.y);
            
            if (IsPointerOverUIObject(clickPosition))
                return;
            
            var clickRay = _camera.ScreenPointToRay(pointerPosition);
            var targetPosition = Vector3.zero;
            
            if (_upperPlane.Raycast(clickRay, out float distance))
                targetPosition = clickRay.GetPoint(distance);

            var mapClickPosition = targetPosition.ToAxialCoordinate(_map.Scale);

            if (_map.HasPosition(mapClickPosition) == false 
                || (_map.HexFrom(mapClickPosition).HasParts == false 
                    && _map.MapCell(mapClickPosition).PointOfInterestType != PointOfInterestType.Leaf))
            {
                if (_lowerPlane.Raycast(clickRay, out distance))
                {
                    targetPosition = clickRay.GetPoint(distance);
                    mapClickPosition = targetPosition.ToAxialCoordinate(_map.Scale);
                }
            }
            
            if (_map.HasPosition(mapClickPosition) == false || _map.IsClosed(mapClickPosition))
                return;

            var clickInfo = new ClickInfo
            {
                ScreenPosition = clickPosition,
                WorldPosition = targetPosition,
                MapPosition = mapClickPosition,
                ZoomFactor = _camera.ZoomFactor
            };

            foreach (var clickCommand in _clickCommands)
                clickCommand.TryExecute(clickInfo);
        }

        private bool IsPointerOverUIObject(Vector2 pointerPosition)
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = pointerPosition };
            EventSystem.current.RaycastAll(eventDataCurrentPosition, _raycastResults);

            return _raycastResults.Count > 0;
        }

        private struct PointerDownInfo
        {
            public float Time { get; init; }
            public Vector2 Position { get; init; }
        }
    }
}