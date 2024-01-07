using System;
using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.AStarPathfinding
{
    public class Path : IPath
    {
        private const int MaxNeighbours = 6;

        private readonly int _maxSteps;
        private readonly IMovePolicy _movePolicy;
        private readonly IBinaryHeap<AxialCoordinate, PathNode> _frontier;
        private readonly IDictionary<AxialCoordinate, AxialCoordinate> _links;
        private readonly HashSet<AxialCoordinate> _ignoredPositions;
        private readonly List<AxialCoordinate> _output;
        
        private readonly PathNode[] _neighbours = new PathNode[MaxNeighbours];

        public Path(IMovePolicy movePolicy, int maxSteps = int.MaxValue, int initialCapacity = 0)
        {
            if (maxSteps <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxSteps));
            
            if (initialCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));

            _movePolicy = movePolicy;
            _maxSteps = maxSteps;
            
            _frontier = new BinaryHeap<AxialCoordinate, PathNode>(node => node.Position, initialCapacity);
            _links = new Dictionary<AxialCoordinate, AxialCoordinate>(initialCapacity);
            _ignoredPositions = new HashSet<AxialCoordinate>(initialCapacity);
            _output = new List<AxialCoordinate>(initialCapacity);
        }

        public bool Calculate(AxialCoordinate start, AxialCoordinate target, out IReadOnlyList<AxialCoordinate> path)
        {
            if (GenerateNodes(start, target, _movePolicy) == false)
            {
                path = Array.Empty<AxialCoordinate>();
                return false;
            }

            _output.Clear();
            _output.Add(target);

            while (_links.TryGetValue(target, out target))
                _output.Add(target);

            path = _output;
            return true;
        }

        private bool GenerateNodes(AxialCoordinate start, AxialCoordinate target, IMovePolicy movePolicy)
        {
            _frontier.Clear();
            _ignoredPositions.Clear();
            _links.Clear();

            if (_movePolicy.CanMove(start) == false)
                return false;

            _frontier.Enqueue(new PathNode(start, target, 0));

            int step = 0;
            
            while (_frontier.Count > 0 && step++ <= _maxSteps)
            {
                PathNode current = _frontier.Dequeue();
                _ignoredPositions.Add(current.Position);

                if (current.Position.Equals(target))
                    return true;

                GenerateFrontierNodes(current, target, movePolicy);
            }

            return false;
        }

        private void GenerateFrontierNodes(PathNode parent, AxialCoordinate target, IMovePolicy movePolicy)
        {
            _neighbours.Fill(parent, target);
            
            foreach (PathNode neighbour in _neighbours)
            {
                if (_ignoredPositions.Contains(neighbour.Position))
                    continue;

                if (movePolicy.CanMove(neighbour.Position) == false)
                    continue;

                if (_frontier.TryGet(neighbour.Position, out PathNode existingNode) == false)
                {
                    _frontier.Enqueue(neighbour);
                    _links[neighbour.Position] = parent.Position;
                }
                else if (neighbour.TraverseDistance < existingNode.TraverseDistance)
                {
                    _frontier.Modify(neighbour);
                    _links[neighbour.Position] = parent.Position;
                }
            }
        }
    }
}
