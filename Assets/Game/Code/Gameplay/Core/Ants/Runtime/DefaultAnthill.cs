using System;
using Newtonsoft.Json;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultAnthill : IAnthill
    {
        private readonly Queen _queen;
        private readonly IAntsList _diggers;
        private readonly IAntsList _loaders;
        private readonly ISave _save;

        private AnthillSave _saveData;
        
        public DefaultAnthill(Queen queen, AntView diggersView, AntView loadersView, ISave save) 
            : this(queen, new AntsList(diggersView, queen.DiggersHomes), new AntsList(loadersView, queen.LoadersHomes), save) 
        { }

        public DefaultAnthill(Queen queen, IAntsList diggers, IAntsList loaders, ISave save)
        {
            _queen = queen;
            _diggers = diggers;
            _loaders = loaders;
            _save = save;
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
            Save();
        }

        public void AddDigger()
        {
            if (CanAddDigger == false)
                throw new InvalidOperationException();
            
            _diggers.Add(_queen.CreateDigger());
            Save();
        }

        public void Load()
        {
            _saveData = new AnthillSave();
            
            if (_save.HasKey(SaveConstants.AnthillSaveKey) == false)
                return;

            _saveData = JsonConvert.DeserializeObject<AnthillSave>(_save.GetString(SaveConstants.AnthillSaveKey));

            for (int i = 0; i < _saveData.DiggersCount && _diggers.CurrentCount < _diggers.MaxCount; i++)
                _diggers.Add(_queen.CreateDigger(spawnInHome: true));

            for (int i = 0; i < _saveData.LoadersCount && _loaders.CurrentCount < _loaders.MaxCount; i++)
                _loaders.Add(_queen.CreateLoader(spawnInHome: true));
        }

        private void Save()
        {
            _saveData.DiggersCount = _diggers.CurrentCount;
            _saveData.LoadersCount = _loaders.CurrentCount;
            
            _save.SetString(SaveConstants.AnthillSaveKey, JsonConvert.SerializeObject(_saveData));
        }
    }

    [Serializable]
    internal class AnthillSave
    {
        [JsonProperty] public int LoadersCount;
        [JsonProperty] public int DiggersCount;
    }
}