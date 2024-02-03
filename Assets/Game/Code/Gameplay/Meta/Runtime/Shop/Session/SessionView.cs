using TMPro;
using UnityEngine;

namespace YellowSquad.Anthill.Meta
{
    internal class SessionView : MonoBehaviour, ISessionView
    {
        [SerializeField] private TMP_Text _diggersCountText;
        [SerializeField] private TMP_Text _loadersCountText;
        [SerializeField] private TMP_Text _antSpeedText;
        
        public void RenderLoadersCount(int currentValue, int maxValue)
        {
            _loadersCountText.text = $"{currentValue}/{maxValue}";
        }

        public void RenderDiggersCount(int currentValue, int maxValue)
        {
            _diggersCountText.text = $"{currentValue}/{maxValue}";
        }

        public void RenderAntMoveDuration(float value, float maxMoveDuration)
        {
            _antSpeedText.text = $"{1f / (value / maxMoveDuration):0.00}";
        }
    }
}