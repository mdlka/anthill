using System;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultAnthill : IAnthill
    {
        private readonly Queen _queen;
        private readonly IAntsList _diggers;
        private readonly IAntsList _loaders;
        
        public DefaultAnthill(Queen queen, AntView diggersView, AntView loadersView) 
            : this(queen, new AntsList(diggersView, queen.DiggersHomes), new AntsList(loadersView, queen.LoadersHomes)) 
        { }

        public DefaultAnthill(Queen queen, IAntsList diggers, IAntsList loaders)
        {
            _queen = queen;
            _diggers = diggers;
            _loaders = loaders;
        }

        public IReadOnlyAntsList Loaders => _loaders;
        public IReadOnlyAntsList Diggers => _diggers;
        public bool CanAddLoader => _queen.CanCreateLoader;
        public bool CanAddDigger => _queen.CanCreateDigger;
        
        public void Update(float deltaTime)
        {
            _loaders.Update(deltaTime);
            _diggers.Update(deltaTime);
        }
        
        public void AddLoader()
        {
            if (CanAddLoader == false)
                throw new InvalidOperationException();
            
            _loaders.Add(_queen.CreateLoader());
        }

        public void AddDigger()
        {
            if (CanAddDigger == false)
                throw new InvalidOperationException();
            
            _diggers.Add(_queen.CreateDigger());
        }
    }
}