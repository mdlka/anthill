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
        [SerializeField, InterfaceType(typeof(IHexMapView))] private Object _hexMapViewObject;

        private IEnumerator Start()
        {
            var hexMap = new Map(new Dictionary<AxialCoordinate, IHex>()
            {
                {new AxialCoordinate(0, 0), new NullableHex()},
                {new AxialCoordinate(1, 0), new NullableHex()},
                {new AxialCoordinate(0, 1), new NullableHex()},
                {new AxialCoordinate(1, 1), new NullableHex()},
            });
            
            hexMap.Visualize(_hexMapViewObject as IHexMapView);

            yield return new WaitForSeconds(5f);
            
            hexMap.RemoveHex(new AxialCoordinate(0, 0));
            hexMap.Visualize(_hexMapViewObject as IHexMapView);
        }
    }
}
