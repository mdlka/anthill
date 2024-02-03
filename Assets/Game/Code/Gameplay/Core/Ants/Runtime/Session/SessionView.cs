using TMPro;
using UnityEngine;

namespace YellowSquad.Anthill.Core.Ants
{
    internal class SessionView : MonoBehaviour, ISessionView
    {
        [SerializeField] private TMP_Text _diggersCountText;
        [SerializeField] private TMP_Text _loadersCountText;
        [SerializeField] private TMP_Text _antSpeedText;
        
        public void RenderLoadersCount(int currentValue, int maxValue)
        {
            _loadersCountText.text = $"Loaders: {currentValue}/{maxValue}";
        }

        public void RenderDiggersCount(int currentValue, int maxValue)
        {
            _diggersCountText.text = $"Diggers: {currentValue}/{maxValue}";
        }

        public void RenderAntMoveDuration(float value, float maxMoveDuration)
        {
            _antSpeedText.text = $"Speed: {1f / (value / maxMoveDuration):0.00}";
        }
    }
}