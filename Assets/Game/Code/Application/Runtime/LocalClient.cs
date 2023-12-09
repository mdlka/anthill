using System.Collections;
using TNRD;
using UnityEngine;
using YellowSquad.HexMath;
using YellowSquad.Anthill.Application.Adapters;
using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.Anthill.Core.HexMap;

namespace YellowSquad.Anthill.Application
{
    public class LocalClient : MonoBehaviour
    {
        [SerializeField] private BaseMapFactory _mapFactory;
        [SerializeField] private SerializableInterface<IHexMapView> _hexMapView;

        private Camera _camera;
        private IHexMap _map;

        private IEnumerator Start()
        {
            _map = _mapFactory.Create();
            _map.Visualize(_hexMapView.Value);

            var path = new Path(new MapMovePolicy(_map));

            _camera = Camera.main;
            
            while (true)
            {
                yield return null;
                yield return new WaitUntil(() => Input.anyKey);

                Vector3 mouseClickPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.y);
                var targetPosition = _camera.ScreenToWorldPoint(mouseClickPosition);
                var targetAxialPosition = targetPosition.ToAxialCoordinate(_map.Scale);

                if (Input.GetMouseButtonDown(0))
                {
                    if (_map.HasPosition(targetAxialPosition) == false)
                        continue;
                
                    var targetHex = _map.HexFrom(targetAxialPosition);
                    
                    if (targetHex.HasParts)
                        targetHex.RemoveClosestPartFor(targetPosition);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                    Debug.Log(_map.ToString());

                _map.Visualize(_hexMapView.Value);
            }
        }
    }
}
