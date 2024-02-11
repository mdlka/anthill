namespace YellowSquad.Anthill.Meta
{
    public struct ShopButtonDTO
    {
        public string ButtonName { get; init; }
        public IButtonCommand ButtonCommand { get; init; }
        public IPriceList PriceList { get; init; }
    }
}