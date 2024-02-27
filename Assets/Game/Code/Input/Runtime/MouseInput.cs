using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Input
{
    public class MouseInput : IInput
    {
        private const float MoveThreshold = 0.1f;
        
        private readonly IHexMap _map;
        private readonly Camera _camera;
        private readonly List<RaycastResult> _raycastResults = new();

        private MouseAction _lastAction;
        private float _lastLeftMouseButtonDownTime;
        private float _lastLeftMouseButtonUpTime;
        
        public MouseInput(IHexMap map, Camera camera)
        {
            _map = map;
            _camera = camera;
        }

        public Vector2 CursorPosition => UnityEngine.Input.mousePosition;
        
        public void Update(float deltaTime)
        {
            _lastAction = MouseAction.Idle;
            
            if (UnityEngine.Input.GetMouseButtonDown(0))
                _lastLeftMouseButtonDownTime = Time.realtimeSinceStartup;

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                _lastLeftMouseButtonUpTime = Time.realtimeSinceStartup;

                if (_lastLeftMouseButtonUpTime - _lastLeftMouseButtonDownTime < MoveThreshold)
                    _lastAction = MouseAction.Click;
            }

            if (_lastLeftMouseButtonDownTime - _lastLeftMouseButtonUpTime >= MoveThreshold)
                _lastAction = MouseAction.Move;
        }

        public bool ClickedOnOpenMapPosition(out AxialCoordinate position)
        {
            position = default;

            if (_lastAction != MouseAction.Click)
                return false;

            var mouseClickPosition = new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, _camera.transform.position.y);
            
            if (IsPointerOverUIObject(mouseClickPosition))
                return false;
            
            var targetPosition = _camera.ScreenToWorldPoint(mouseClickPosition);
            position = targetPosition.ToAxialCoordinate(_map.Scale);

            return _map.HasPosition(position) && _map.IsClosed(position) == false;
        }

        public bool Moved(out Vector2 delta)
        {
            delta = Vector2.zero;

            if (_lastAction != MouseAction.Move)
                return false;
            
            delta = -new Vector2(UnityEngine.Input.GetAxisRaw("Mouse X"), UnityEngine.Input.GetAxisRaw("Mouse Y"));
            return true;
        }

        public bool Zoomed(out float delta)
        {
            delta = 0f;
            
            if (UnityEngine.Input.mouseScrollDelta.y == 0)
                return false;

            delta = UnityEngine.Input.mouseScrollDelta.y;
            return true;
        }

        private bool IsPointerOverUIObject(Vector2 inputPosition)
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = inputPosition };
            EventSystem.current.RaycastAll(eventDataCurrentPosition, _raycastResults);

            return _raycastResults.Any(result => result.gameObject.layer == LayerMask.NameToLayer("UI"));
        }

        private enum MouseAction
        {
            Idle,
            Click,
            Move
        }
    }
}