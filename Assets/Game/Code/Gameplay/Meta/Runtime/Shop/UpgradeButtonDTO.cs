using UnityEngine;

namespace YellowSquad.Anthill.Meta
{
    public struct UpgradeButtonDTO
    {
        public string ButtonName { get; init; }
        public Sprite Icon { get; init; }
        public IUpgrade Upgrade { get; init; }
    }
}