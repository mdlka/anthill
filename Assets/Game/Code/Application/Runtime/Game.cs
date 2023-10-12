using System.Collections;
using UnityEngine;
using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;
using Object = UnityEngine.Object;

namespace YellowSquad.Anthill.Application
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private BaseMapFactory _mapFactory;
        [SerializeField, InterfaceType(typeof(IHexMapView))] private Object _hexMapViewObject;

        private Camera _camera;
        private IHexMap _map;

        private IEnumerator Start()
        {
            _map = _mapFactory.Create();
            _map.Visualize(_hexMapViewObject as IHexMapView);

            var path = new Path(new MapMovePolicy(_map));

            _camera = Camera.main;
            
            while (true)
            {
                yield return null;
                yield return new WaitUntil(() => Input.anyKey);

                Vector3 mouseClickPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.y);
                var targetPosition = _camera.ScreenToWorldPoint(mouseClickPosition).ToAxialCoordinate(_map.Scale);

                if (Input.GetMouseButtonDown(0))
                {
                    if (_map.HasHexIn(targetPosition) == false)
                        continue;
                
                    _map.RemoveHex(targetPosition);
                }

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if (_map.HasHexIn(targetPosition))
                        continue;
                
                    _map.AddHex(targetPosition, new EmptyHex());
                }
                
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    if (_map.HasHexIn(targetPosition))
                        continue;
                
                    _map.AddHex(targetPosition, new ObstacleHex());
                }

                if (Input.GetMouseButtonDown(1))
                {
                    Debug.Log($"Try find path from (0, 0) to {targetPosition}");

                    if (path.Calculate(new AxialCoordinate(0, 0), targetPosition, out var result) == false)
                        continue;

                    Debug.Log(string.Join(' ', result));
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log(_map.ToString());
                }
                
                _map.Visualize(_hexMapViewObject as IHexMapView);
            }
        }
    }
}
