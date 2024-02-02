using System;
using System.Collections.Generic;
using UnityEngine;
using YellowSquad.Anthill.Core.Ants;
using YellowSquad.Anthill.Meta;
using YellowSquad.GameLoop;

namespace YellowSquad.Anthill.Application
{
    public class Session : ISession, IGameLoop
    {
        private readonly Queen _queen;
        private readonly AntView _diggersView;
        private readonly AntView _loadersView;
        private readonly List<IAnt> _ants = new();

        public Session(Queen queen, AntView diggersView, AntView loadersView)
        {
            _queen = queen;
            _diggersView = diggersView;
            _loadersView = loadersView;
        }

        public bool CanAddDigger => _queen.CanCreateDigger;
        public bool CanAddLoader => _queen.CanCreateLoader;
        public bool CanIncreaseSpeed => false;
        
        public void Update(float deltaTime)
        {
            _diggersView.UpdateRender();
            _loadersView.UpdateRender();
            
            foreach (var ant in _ants)
                ant.Update(Time.deltaTime);
        }
        
        public void AddDigger()
        {
            if (CanAddDigger == false)
                throw new InvalidOperationException();
            
            var ant = _queen.CreateDigger();
            _ants.Add(ant);
            _diggersView.Add(ant);
        }

        public void AddLoader()
        {
            if (CanAddLoader == false)
                throw new InvalidOperationException();
            
            var ant = _queen.CreateLoader();
            _ants.Add(ant);
            _loadersView.Add(ant);
        }

        public void IncreaseSpeed()
        {
            if (CanIncreaseSpeed == false)
                throw new InvalidOperationException();
        }
    }
}