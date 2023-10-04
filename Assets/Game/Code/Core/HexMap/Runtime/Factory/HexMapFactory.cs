using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Core.HexMap
{
    [CreateAssetMenu(menuName = "Anthill/Maps/Create HexMapFactory", fileName = "HexMapFactory", order = 56)]
    public class HexMapFactory : ScriptableObject, IHexMapFactory
    {
        [SerializeField, Range(0, 10)] private int _range = 2;
        [SerializeField, Min(0.01f)] private float _maxScale = 0.57f;   
        
        public IHexMap Create()
        {
            var hexes = new Dictionary<AxialCoordinate, IHex>();
            
            for (int q = -_range; q <= _range; q++) 
            {
                int r1 = Mathf.Max(-_range, -q - _range);
                int r2 = Mathf.Min(_range, -q + _range);
                
                for (int r = r1; r <= r2; r++)
                    hexes.TryAdd(new AxialCoordinate(q, r), new NullHex());
            }

            return new Map(_maxScale, hexes);
        }
    }
}