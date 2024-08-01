using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.Anthill.Core.Tasks
{
    internal class TaskProgressView : MonoBehaviour
    {
        [SerializeField] private Image _progressImage;

        public void Render(float progress)
        {
            _progressImage.fillAmount = progress;
        }
    }
}