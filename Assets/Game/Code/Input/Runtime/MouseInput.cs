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
        private readonly IHexMap _map;
        private readonly Camera _camera;
        private readonly List<RaycastResult> _raycastResults = new();
        
        public MouseInput(IHexMap map)
        {
            _map = map;
            _camera = Camera.main;
        }
        
        public bool ClickedOnOpenMapPosition(out AxialCoordinate position)
        {
            position = default;
            
            if (UnityEngine.Input.GetMouseButtonDown(0) == false)
                return false;
            
            var mouseClickPosition = new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, _camera.transform.position.y);
            
            if (IsPointerOverUIObject(mouseClickPosition))
                return false;
            
            var targetPosition = _camera.ScreenToWorldPoint(mouseClickPosition);
            position = targetPosition.ToAxialCoordinate(_map.Scale);

            return _map.HasPosition(position) && _map.IsClosed(position) == false;
        }
        
        private bool IsPointerOverUIObject(Vector2 inputPosition)
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = inputPosition };
            EventSystem.current.RaycastAll(eventDataCurrentPosition, _raycastResults);

            return _raycastResults.Any(result => result.gameObject.layer == LayerMask.NameToLayer("UI"));
        }
    }
}