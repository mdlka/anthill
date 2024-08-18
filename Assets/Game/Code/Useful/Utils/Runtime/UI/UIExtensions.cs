using DG.Tweening;
using UnityEngine;

namespace YellowSquad.Utils
{
    public static class UIExtensions
    {
        public static void Enable(this CanvasGroup canvasGroup, float duration = 0)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOFade(1, duration).SetUpdate(true);
        }
        
        public static void Disable(this CanvasGroup canvasGroup, float duration = 0)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(0, duration).SetUpdate(true);
        }
    }
}