using System.Collections.Generic;
using System.Linq;
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

        private Vector2 _lastPointerDownPosition;
        private float _lastPointerDownTime;
        private float _lastPointerUpTime;
        private bool _cameraMoving;

        public InputRoot(IHexMap map, IInput input, ICamera camera, IClickCommand[] commands)
        {
            _map = map;
            _input = input;
            _camera = camera;
            _clickCommands = commands;
        }

        public void Update(float deltaTime)
        {
            if (_input.ZoomDelta != 0)
                _camera.Zoom(-_input.ZoomDelta, () => _input.PointerPosition);
            
            if (_input.PointerDown)
                OnPointerDown();
            
            OnPointerMove();
            
            if (_input.PointerUp)
                OnPointerUp();
        }

        private void OnPointerDown()
        {
            _lastPointerDownTime = Time.realtimeSinceStartup;
            _lastPointerDownPosition = _input.PointerPosition;
        }

        private void OnPointerMove()
        {
            if (_lastPointerDownTime - _lastPointerUpTime < MoveThreshold)
                return;
            
            if (_cameraMoving == false)
                _camera.StartMove(_input.PointerPosition);

            _camera.Move(_input.PointerPosition);
            _cameraMoving = true;
        }

        private void OnPointerUp()
        {
            _lastPointerUpTime = Time.realtimeSinceStartup;
            
            if (_lastPointerUpTime - _lastPointerDownTime < MoveThreshold
                || Vector2.Distance(_lastPointerDownPosition, _input.PointerPosition) <= PointerClickMaxDelta) 
                OnPointerClick();

            _cameraMoving = false;
        }

        private void OnPointerClick()
        {
            var clickPosition = new Vector3(_input.PointerPosition.x, _input.PointerPosition.y, _camera.Position.y);
            
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

        private bool IsPointerOverUIObject(Vector2 inputPosition)
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = inputPosition };
            EventSystem.current.RaycastAll(eventDataCurrentPosition, _raycastResults);

            return _raycastResults.Any(result => result.gameObject.layer == LayerMask.NameToLayer("UI"));
        }
    }
}