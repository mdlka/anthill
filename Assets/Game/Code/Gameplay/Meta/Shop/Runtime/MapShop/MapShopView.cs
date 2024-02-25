using System.Collections.Generic;
using UnityEngine;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Meta.Shop
{
    public class MapShopView : MonoBehaviour, IMapShopView
    {
        private readonly Dictionary<AxialCoordinate, MapCellPriceView> _views = new();

        [SerializeField] private MapCellPriceView _cellPriceViewTemplate;
        [SerializeField] private Transform _cellPriceViewContent;
        [SerializeField] private Vector3 _cellPriceViewOffset;

        private IHexMap _map;
        private ITaskStorage _taskStorage;
        
        public void Initialize(IHexMap map, ITaskStorage taskStorage)
        {
            _map = map;
            _taskStorage = taskStorage;
        }
        
        public void Render(bool canBuyCell, int currentPrice)
        {
            foreach (var position in _map.Positions)
            {
                if (_taskStorage.HasTaskGroupIn(position) == false && _map.HexFrom(position).HasParts && _map.IsClosed(position) == false)
                {
                    if (_views.ContainsKey(position) == false)
                        _views.Add(position, Instantiate(_cellPriceViewTemplate, position.ToVector3(_map.Scale) + _cellPriceViewOffset, 
                            _cellPriceViewTemplate.transform.rotation, _cellPriceViewContent));
                    
                    _views[position].Render(canBuyCell, currentPrice);
                }
                else
                {
                    if (_views.ContainsKey(position) == false) 
                        continue;
                    
                    Destroy(_views[position].gameObject);
                    _views.Remove(position);
                }
            }
        }
    }
}