using System;
using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IHexMapView
    {
        bool Initialized { get; }

        void InitializeRender(float mapScale, IReadOnlyDictionary<AxialCoordinate, MapCell> cells, HashSet<AxialCoordinate> closedPositions);
        
        void Render(float mapScale, HashSet<AxialCoordinate> closedPositions, params MapCellChange[] changes);
        
        Matrix4x4 HexMatrixBy(float mapScale, AxialCoordinate position);
        Matrix4x4 PointOfInterestMatrixBy(float mapScale, AxialCoordinate position, PointOfInterestType type);
    }

    public class MapCellChange
    {
        public AxialCoordinate Position { get; init; }
        public IEnumerable<IReadOnlyPart> RemovedParts { get; init; } = Array.Empty<IReadOnlyPart>();
        public IEnumerable<IReadOnlyPart> AddedParts { get; init; } = Array.Empty<IReadOnlyPart>();
        public IEnumerable<IReadOnlyPart> ChangedSizeParts { get; init; } = Array.Empty<IReadOnlyPart>();

        public MapCell MapCell { get; init; }
        
        public ChangeType ChangeType { get; init; }
    }
    
    public enum ChangeType
    {
        Hex,
        PointOfInterest
    }
}