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

        public InputRoot(IHexMap map, IInput input, ICamera camera, IClickCommand[] commands)
        {
            _map = map;
            _input = input;
            _camera = camera;
            _clickCommands = commands;
            _pointersDown[0] = default;
        }

        private int CurrentPointerId => _pointers.Count > 0 ? _pointers[0] : _lastPointerId;

        public void Update(float deltaTime)
        {
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
            
            var targetPosition = _camera.ScreenToWorldPoint(clickPosition);
            var mapClickPosition = targetPosition.ToAxialCoordinate(_map.Scale);
            
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