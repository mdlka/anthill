using System;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    [Serializable]
    public class LocalizedText
    {
        [field: SerializeField] public Language Language { get; private set; }
        [field: SerializeField] public string Text { get; private set; }
    }
}