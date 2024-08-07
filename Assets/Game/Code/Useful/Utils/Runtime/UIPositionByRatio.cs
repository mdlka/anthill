using System;
using UnityEngine;

namespace YellowSquad.Utils
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class UIPositionByRatio : MonoBehaviour
    {
        [SerializeField] private Vector2 _landscapePosition;
        [SerializeField] private Vector2 _portraitPosition;

        private RectTransform _rectTransform;
        private float _oldRatio;

        private void Awake()
        {
            UpdatePosition();
        }

        private void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            _rectTransform ??= transform as RectTransform;
            
            float ratio = 1.0f * Screen.height / Screen.width;

            if (Math.Abs(ratio - _oldRatio) < float.Epsilon)
                return;

            _oldRatio = ratio;
            _rectTransform.anchoredPosition = _oldRatio > 1f ? _portraitPosition : _landscapePosition;
        }
    }
}