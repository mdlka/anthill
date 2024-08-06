using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class UpdateLeafView : MonoBehaviour
    {
        private readonly Dictionary<AxialCoordinate, GameObject> _instances = new(); 
        
        [SerializeField] private GameObject _leafTemplate;
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
                    
                    _instances.Add(change.Position, Instantiate(_leafTemplate, 
                        change.Position.ToVector3(mapScale) + _offset, 
                        _leafTemplate.transform.rotation, transform));
                }
                else
                {
                    if (_instances.ContainsKey(change.Position) == false)
                        continue;
                    
                    Destroy(_instances[change.Position]);
                    _instances.Remove(change.Position);
                }       
            }
        }
    }
}