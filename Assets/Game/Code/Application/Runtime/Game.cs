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
        [SerializeField, Range(0, 10)] private int _range;
        [SerializeField, Min(0.01f)] private float _maxScale;
        [SerializeField, InterfaceType(typeof(IHexMapView))] private Object _hexMapViewObject;

        private IEnumerator Start()
        {
            var hexes = new Dictionary<AxialCoordinate, IHex>();
            
            for (int q = -_range; q <= _range; q++) 
            {
                int r1 = Mathf.Max(-_range, -q - _range);
                int r2 = Mathf.Min(_range, -q + _range);
                
                for (int r = r1; r <= r2; r++)
                    hexes.TryAdd(new AxialCoordinate(q, r), new NullableHex());
            }

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
