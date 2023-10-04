using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Core.HexMap
{
    [CreateAssetMenu(menuName = "Anthill/Maps/Create HexMapFactory", fileName = "HexMapFactory", order = 56)]
    public class CustomMapFactory : BaseMapFactory
    {
        [SerializeField, Range(0, 10)] private int _mapRange = 2;
        [SerializeField, Min(0.01f)] private float _mapScale = 0.57f;

        internal float MapScale => _mapScale;
        
        public override IHexMap Create()
        {
            var hexes = new Dictionary<AxialCoordinate, IHex>();
            
            for (int q = -_mapRange; q <= _mapRange; q++) 
            {
                int r1 = Mathf.Max(-_mapRange, -q - _mapRange);
                int r2 = Mathf.Min(_mapRange, -q + _mapRange);
                
                for (int r = r1; r <= r2; r++)
                    hexes.TryAdd(new AxialCoordinate(q, r), new NullHex());
            }

            return new Map(_mapScale, hexes);
        }
    }
}