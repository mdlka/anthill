using TMPro;
using UnityEngine;
using YellowSquad.Utils;

namespace YellowSquad.Anthill.Meta
{
    public class MapGoalView : MonoBehaviour, IMapGoalView
    {
        [SerializeField] private TMP_Text _progressText;
        [SerializeField] private SlicedFilledImage _progressForeground;
        
        public void Render(int currentValue, int targetValue)
        {
            _progressText.text = $"{currentValue}/{targetValue}";
            _progressForeground.fillAmount = Mathf.Clamp01(1f * currentValue / targetValue);
        }
    }
}