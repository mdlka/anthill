using System.Collections;
using UnityEngine;
using YellowSquad.Core.HexMap;
using YellowSquad.HexMath;
using Object = UnityEngine.Object;

namespace YellowSquad.Application
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private BaseMapFactory _mapFactory;
        [SerializeField, InterfaceType(typeof(IHexMapView))] private Object _hexMapViewObject;

        private Camera _camera;

        private IEnumerator Start()
        {
            var hexMap = _mapFactory.Create();
            hexMap.Visualize(_hexMapViewObject as IHexMapView);
            
            _camera = Camera.main;
            
            while (true)
            {
                yield return null;
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1));

                Vector3 mouseClickPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.y);
                var targetPosition = _camera.ScreenToWorldPoint(mouseClickPosition).ToAxialCoordinate(hexMap.Scale);

                if (Input.GetMouseButtonDown(0))
                {
                    if (hexMap.HasHexIn(targetPosition) == false)
                        continue;
                
                    hexMap.RemoveHex(targetPosition);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    if (hexMap.HasHexIn(targetPosition))
                        continue;
                
                    hexMap.AddHex(targetPosition, new EmptyHex());
                }
                
                hexMap.Visualize(_hexMapViewObject as IHexMapView);
            }
        }
    }
}
