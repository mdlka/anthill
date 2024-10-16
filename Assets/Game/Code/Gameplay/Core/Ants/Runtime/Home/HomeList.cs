﻿using System;
using System.Collections.Generic;
using System.Linq;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class HomeList : IHomeList
    {
        private readonly Dictionary<AxialCoordinate, int> _homeAntsCount = new();
        private readonly int _homeCapacity;
        private readonly IHexMap _map;
        private readonly IHome[] _homes;

        public HomeList(int homeCapacity, IHexMap map, params IHome[] homes)
        {
            _homeCapacity = homeCapacity;
            _map = map;
            _homes = homes;

            foreach (var home in homes)
                _homeAntsCount.Add(home.Position, 0);
        }

        public bool HasFreeHome => _homeAntsCount.Any(pair => IsAvailableHome(pair.Key));
        public int BusyPlaces => _homeAntsCount.Sum(pair => pair.Value);
        public int OpenPlaces => _homeAntsCount.Count(pair => _map.HexFrom(pair.Key).HasParts == false) * _homeCapacity;

        public IHome FindFreeHome()
        {
            foreach (var pair in _homeAntsCount)
                if (IsAvailableHome(pair.Key))
                    return _homes.First(home => home.Position == pair.Key);

            throw new InvalidOperationException();
        }

        public void AddAntTo(AxialCoordinate position)
        {
            if (IsAvailableHome(position) == false)
                throw new InvalidOperationException();

            _homeAntsCount[position] += 1;
        }
        
        private bool IsAvailableHome(AxialCoordinate position)
        {
            if (_homeAntsCount.ContainsKey(position) == false)
                throw new ArgumentException();

            return _homeAntsCount[position] < _homeCapacity && _map.HexFrom(position).HasParts == false;
        }
    }
}