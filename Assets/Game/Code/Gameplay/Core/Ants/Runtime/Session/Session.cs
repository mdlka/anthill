using System;
using System.Collections.Generic;
using UnityEngine;
using YellowSquad.GameLoop;

namespace YellowSquad.Anthill.Core.Ants
{
    public class Session : ISession, IGameLoop
    {
        private readonly Queen _queen;
        private readonly AntView _diggersView;
        private readonly AntView _loadersView;
        private readonly MovementSettings _movementSettings;
        private readonly List<IAnt> _ants = new();

        public Session(Queen queen, MovementSettings movementSettings, AntView diggersView, AntView loadersView)
        {
            _queen = queen;
            _diggersView = diggersView;
            _loadersView = loadersView;
            _movementSettings = movementSettings;
        }

        public float MaxAntMoveDuration => _movementSettings.MaxMoveDuration;
        public bool CanAddDigger => _queen.CanCreateDigger;
        public bool CanAddLoader => _queen.CanCreateLoader;

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

        public void ChangeAntsMoveDuration(float value)
        {
            _movementSettings.ChangeMoveDuration(value);
        }

        public void Visualize(ISessionView view)
        {
            view.RenderAntMoveDuration(_movementSettings.CurrentMoveDuration, _movementSettings.MaxMoveDuration);
            view.RenderDiggersCount(_queen.DiggersHomes.BusyPlaces, _queen.DiggersHomes.OpenPlaces);
            view.RenderLoadersCount(_queen.LoadersHomes.BusyPlaces, _queen.LoadersHomes.OpenPlaces);
        }
    }
}