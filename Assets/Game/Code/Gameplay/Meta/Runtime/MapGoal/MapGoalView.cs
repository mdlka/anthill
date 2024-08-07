using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.Anthill.Meta
{
    public class MapGoalView : MonoBehaviour, IMapGoalView
    {
        [SerializeField] private TMP_Text _progressText;
        [SerializeField] private Image _progressForeground;
        
        public void Render(int currentValue, int targetValue)
        {
            _progressText.text = $"{currentValue}/{targetValue}";
            _progressForeground.fillAmount = Mathf.Clamp01(1f * currentValue / targetValue);
        }
    }
}