using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class Queen
    {
        private readonly AxialCoordinate _position;
        private readonly IAntFactory _antFactory;
        private readonly IHomeList _diggersHomes;
        private readonly IHomeList _loadersHomes;

        public Queen(AxialCoordinate position, IAntFactory antFactory, IHomeList diggersHomes, IHomeList loadersHomes)
        {
            _position = position;
            _antFactory = antFactory;
            _diggersHomes = diggersHomes;
            _loadersHomes = loadersHomes;
        }

        public IReadOnlyHomeList DiggersHomes => _diggersHomes;
        public IReadOnlyHomeList LoadersHomes => _loadersHomes;
        public bool CanCreateDigger => _diggersHomes.HasFreeHome;
        public bool CanCreateLoader => _loadersHomes.HasFreeHome;

        public IAnt CreateDigger(bool spawnInHome = false)
        {
            if (CanCreateDigger == false)
                throw new InvalidOperationException();

            var freeHome = _diggersHomes.FindFreeHome();

            _diggersHomes.AddAntTo(freeHome.Position);
            return _antFactory.CreateDigger(freeHome, spawnInHome ? freeHome.Position : _position);
        }

        public IAnt CreateLoader(bool spawnInHome = false)
        {
            if (CanCreateLoader == false)
                throw new InvalidOperationException();

            var freeHome = _loadersHomes.FindFreeHome();

            _loadersHomes.AddAntTo(freeHome.Position);
            return _antFactory.CreateLoader(freeHome, spawnInHome ? freeHome.Position : _position);
        }
    }
}