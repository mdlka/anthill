using System.Collections.Generic;

namespace YellowSquad.Anthill.Core.Ants
{
    internal class AntsList : IAntsList
    {
        private readonly AntView _antsView;
        private readonly IReadOnlyHomeList _homeList;
        private readonly List<IAnt> _ants = new();

        public AntsList(AntView antsView, IReadOnlyHomeList homeList)
        {
            _antsView = antsView;
            _homeList = homeList;
        }

        public int CurrentCount => _ants.Count;
        public int MaxCount => _homeList.OpenPlaces;
        
        public void Update(float deltaTime)
        {
            _antsView.UpdateRender();
            
            foreach (var ant in _ants)
                ant.Update(deltaTime);
        }

        public void Add(IAnt ant)
        {
            _ants.Add(ant);
            _antsView.Add(ant);
        }
    }
}