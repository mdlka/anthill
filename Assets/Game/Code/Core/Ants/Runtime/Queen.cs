using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class Queen
    {
        private readonly AxialCoordinate _position;
        private readonly IAntFactory _antFactory;
        private readonly IHomeList _diggersHomes;
        private readonly IHomeList _loaderHomes;

        public Queen(AxialCoordinate position, IAntFactory antFactory, IHomeList diggersHomes, IHomeList loaderHomes)
        {
            _position = position;
            _antFactory = antFactory;
            _diggersHomes = diggersHomes;
            _loaderHomes = loaderHomes;
        }

        public bool CanCreateDigger => _diggersHomes.HasFreeHome;
        public bool CanCreateLoader => _loaderHomes.HasFreeHome;

        public IAnt CreateDigger()
        {
            if (CanCreateDigger == false)
                throw new InvalidOperationException();

            var freeHome = _diggersHomes.FindFreeHome();

            _diggersHomes.AddAntTo(freeHome.Position);
            return _antFactory.CreateDigger(freeHome, _position);
        }

        public IAnt CreateLoader()
        {
            if (CanCreateLoader == false)
                throw new InvalidOperationException();

            var freeHome = _loaderHomes.FindFreeHome();

            _loaderHomes.AddAntTo(freeHome.Position);
            return _antFactory.CreateLoader(freeHome, _position);
        }
    }
}