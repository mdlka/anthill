using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class UpdateLeafView : MonoBehaviour
    {
        private readonly Dictionary<AxialCoordinate, SpriteRenderer> _instances = new(); 
        
        [SerializeField] private SpriteRenderer _rendererTemplate;
        [SerializeField] private Vector3 _offset;

        public void Render(float mapScale, IReadOnlyDictionary<AxialCoordinate, MapCell> cells)
        {
            foreach (var cell in cells)
            {
                if (cell.Value.PointOfInterestType != PointOfInterestType.Leaf)
                    continue;

                if (cell.Value.DividedPointOfInterest.HasParts == false)
                {
                    if (_instances.ContainsKey(cell.Key)) 
                        continue;
                    
                    _instances.Add(cell.Key, Instantiate(_rendererTemplate, 
                        cell.Key.ToVector3(mapScale) + _offset, 
                        _rendererTemplate.transform.rotation, transform));
                }
                else
                {
                    if (_instances.ContainsKey(cell.Key) == false)
                        continue;
                    
                    Destroy(_instances[cell.Key].gameObject);
                    _instances.Remove(cell.Key);
                }       
            }
        }
    }
}