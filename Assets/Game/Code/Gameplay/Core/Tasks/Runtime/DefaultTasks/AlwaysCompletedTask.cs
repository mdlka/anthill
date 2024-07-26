﻿using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class AlwaysCompletedTask : ITask
    {
        public int Price => 0;
        public FracAxialCoordinate TargetPosition => FracAxialCoordinate.Zero;
        public bool Completed => true;
        
        public bool Equals(ITask other)
        {
            return false;
        }
        
        public void UpdateProgress(float speed = 1f) { }
    }
}