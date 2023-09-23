using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YellowSquad.Core.HexMap;
using YellowSquad.HexMath;
using Object = UnityEngine.Object;

namespace YellowSquad.Application
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Vector2Int _area;
        [SerializeField, Min(0.01f)] private float _maxScale;
        [SerializeField, InterfaceType(typeof(IHexMapView))] private Object _hexMapViewObject;

        private IEnumerator Start()
        {
            var hexes = new Dictionary<AxialCoordinate, IHex>();
            
            for (int i = _area.x; i < _area.y; i++)
                for (int j = _area.x; j < _area.y; j++)
                    hexes.TryAdd(new AxialCoordinate(i, j), new NullableHex());
            
            var hexMap = new Map(_maxScale, hexes);
            hexMap.Visualize(_hexMapViewObject as IHexMapView);

            while (true)
            {
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

                var clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(clickRay, out RaycastHit hitInfo) == false)
                    continue;
                
                hexMap.RemoveHex(hitInfo.point.ToAxialCoordinate(_maxScale));
                hexMap.Visualize(_hexMapViewObject as IHexMapView);
            }
        }
    }
}
