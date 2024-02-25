using UnityEngine;

namespace YellowSquad.Anthill.Meta.Shop
{
    public struct UpgradeButtonDTO
    {
        public string ButtonName { get; init; }
        public Sprite Icon { get; init; }
        public IUpgrade Upgrade { get; init; }
    }
}