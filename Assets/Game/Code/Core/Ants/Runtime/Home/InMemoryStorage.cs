﻿using System;
using System.Collections.Generic;

namespace YellowSquad.Anthill.Core.Ants
{
    public class InMemoryStorage : ITaskStorage
    {
        private readonly Queue<ITask> _tasks = new();

        public bool HasTask => _tasks.Count > 0;
        
        public void AddTask(ITask task)
        {
            _tasks.Enqueue(task);
        }
        
        public ITask FindTask()
        {
            if (HasTask == false)
                throw new InvalidOperationException();

            return _tasks.Dequeue();
        }
    }
}