using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.AStarPathfinding
{
    internal readonly struct PathNode : IComparable<PathNode>
    {
        private readonly double _estimatedTotalCost;
        private readonly double _heuristicDistance;

        public PathNode(AxialCoordinate position, AxialCoordinate target, double traverseDistance)
        {
            Position = position;
            TraverseDistance = traverseDistance;
            _heuristicDistance = (position - target).DistanceEstimate();
            _estimatedTotalCost = traverseDistance + _heuristicDistance;
        }

        public AxialCoordinate Position { get; }
        public double TraverseDistance { get; }

        public int CompareTo(PathNode other)
            => _estimatedTotalCost.CompareTo(other._estimatedTotalCost);
    }
}
