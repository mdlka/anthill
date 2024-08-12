using DG.Tweening;
using UnityEngine;

namespace Agava.MonstersMerge.Tutorial
{
    internal class UpDownAnimation : MonoBehaviour
    {
        [SerializeField, Min(0.001f)] private float _duration;
        [SerializeField] private float _offsetY;
        
        private void Start()
        {
            float basePositionY = transform.localPosition.y;

            var sequence = DOTween.Sequence()
                .SetAutoKill(false)
                .SetLoops(-1)
                .SetEase(Ease.Linear);
            
            sequence.Append(transform.DOLocalMoveY(basePositionY + _offsetY, _duration / 2));
            sequence.Append(transform.DOLocalMoveY(basePositionY, _duration / 2));
        }
    }
}