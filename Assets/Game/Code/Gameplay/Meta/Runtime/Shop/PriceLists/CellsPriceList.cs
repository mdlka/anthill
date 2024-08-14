using System;
using Newtonsoft.Json;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.Anthill.Meta
{
    public class CellsPriceList : IPriceList
    {
        private readonly int _addValue;
        private readonly IHexMap _map;
        private readonly ISave _save;

        private CellsPriceListSave _saveData;

        public CellsPriceList(IHexMap map, ISave save, int startValue, int addValue)
        {
            _map = map;
            _save = save;
            _addValue = addValue;
            CurrentPrice = startValue;
        }

        public int CurrentPriceNumber { get; private set; } = 1;
        public int CurrentPrice { get; private set; }
        public bool HasNext => CurrentPriceNumber < _map.TotalCells;
        public bool HasPrevious => CurrentPriceNumber > 1;

        public void Next()
        {
            CurrentPrice += CalculateAddedValue();
            CurrentPriceNumber += 1;

            Save();
        }

        public void Previous()
        {
            CurrentPriceNumber -= 1;
            CurrentPrice -= CalculateAddedValue();

            Save();
        }

        public void Load()
        {
            _saveData = new CellsPriceListSave
            {
                CurrentPrice = CurrentPrice, 
                CurrentPriceNumber = CurrentPriceNumber
            };
            
            if (_save.HasKey(SaveConstants.MapCellPriceListSaveKey) == false)
                return;

            _saveData = JsonConvert.DeserializeObject<CellsPriceListSave>(_save.GetString(SaveConstants.MapCellPriceListSaveKey));
            CurrentPrice = _saveData.CurrentPrice;
            CurrentPriceNumber = _saveData.CurrentPriceNumber;
        }

        private int CalculateAddedValue()
        {
            float addedValue = _addValue - CurrentPriceNumber * _addValue / _map.TotalCells;
            return (int)Math.Round(addedValue);
        }
        
        private void Save()
        {
            _saveData.CurrentPrice = CurrentPrice;
            _saveData.CurrentPriceNumber = CurrentPriceNumber;
            
            _save.SetString(SaveConstants.MapCellPriceListSaveKey, JsonConvert.SerializeObject(_saveData));
        }
    }

    [Serializable]
    internal class CellsPriceListSave
    {
        [JsonProperty] public int CurrentPrice;
        [JsonProperty] public int CurrentPriceNumber;
    }
}