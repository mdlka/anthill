﻿using System.Collections.Generic;
using UnityEngine;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Meta.Map
{
    public class MapPriceView : MonoBehaviour, IMapPriceView
    {
        private readonly Dictionary<AxialCoordinate, MapCellPriceView> _views = new();

        [SerializeField] private MapCellPriceView _cellPriceViewTemplate;
        [SerializeField] private Transform _cellPriceViewContent;
        [SerializeField] private Vector3 _cellPriceViewOffset;

        private IHexMap _map;
        
        public void Initialize(IHexMap map)
        {
            _map = map;
        }
        
        public void Render(bool canBuyCell, int currentPrice)
        {
            foreach (var position in _map.Positions)
            {
                if (_map.HexFrom(position).HasParts && _map.IsClosed(position) == false)
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