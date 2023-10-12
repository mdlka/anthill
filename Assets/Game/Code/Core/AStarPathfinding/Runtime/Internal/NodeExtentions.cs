using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.AStarPathfinding
{
    internal static class NodeExtensions
    {
        private static readonly (AxialCoordinate position, double cost)[] NeighboursTemplate =
        {
            (new AxialCoordinate(1, 0), cost: 1),
            (new AxialCoordinate(0, 1), cost: 1),
            (new AxialCoordinate(-1, 0), cost: 1),
            (new AxialCoordinate(0, -1), cost: 1),
            (new AxialCoordinate(-1, 1), cost: 1),
            (new AxialCoordinate(1, -1), cost: 1),
        };

        public static void Fill(this PathNode[] buffer, PathNode parent, AxialCoordinate target)
        {
            int i = 0;
            
            foreach ((var position, double cost) in NeighboursTemplate)
            {
                double traverseDistance = parent.TraverseDistance + cost;
                var nodePosition = position + parent.Position;
                
                buffer[i++] = new PathNode(nodePosition, target, traverseDistance);
            }
        }
    }
}
