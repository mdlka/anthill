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

        private IEnumerator Start()
        {
            var hexMap = _mapFactory.Create();
            hexMap.Visualize(_hexMapViewObject as IHexMapView);
            
            while (true)
            {
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

                var clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(clickRay, out RaycastHit hitInfo) == false)
                    continue;

                var position = hitInfo.point.ToAxialCoordinate(hexMap.Scale);

                if (hexMap.HasHexIn(position) == false)
                    continue;
                
                hexMap.RemoveHex(position);
                hexMap.Visualize(_hexMapViewObject as IHexMapView);
            }
        }
    }
}
