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

        public void Render(float mapScale, params MapCellChange[] changes)
        {
            foreach (var change in changes)
            {
                if (change.MapCell.PointOfInterestType != PointOfInterestType.Leaf)
                    continue;

                if (change.MapCell.DividedPointOfInterest.CanRestore)
                {
                    if (_instances.ContainsKey(change.Position)) 
                        continue;
                    
                    _instances.Add(change.Position, Instantiate(_rendererTemplate, 
                        change.Position.ToVector3(mapScale) + _offset, 
                        _rendererTemplate.transform.rotation, transform));
                }
                else
                {
                    if (_instances.ContainsKey(change.Position) == false)
                        continue;
                    
                    Destroy(_instances[change.Position].gameObject);
                    _instances.Remove(change.Position);
                }       
            }
        }
    }
}