using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IHexMapView
    {
        void Render(float mapScale, IReadOnlyDictionary<AxialCoordinate, MapCell> cells, HashSet<AxialCoordinate> closedPositions);
        
        Matrix4x4 HexMatrixBy(float mapScale, AxialCoordinate position);
        Matrix4x4 PointOfInterestMatrixBy(float mapScale, AxialCoordinate position, PointOfInterestType type);
    }
}