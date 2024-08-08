using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace YellowSquad.Anthill.Meta
{
    public class MoneyAnimation : MonoBehaviour
    {
        [SerializeField] private Image _coinTemplate;
        [SerializeField] private RectTransform _container;
        [SerializeField] private RectTransform _endPoint;
        [Header("Animation Settings")]
        [SerializeField] private int _spawnCount = 10;
        [SerializeField] private float _spawnRadius = 250;
        [SerializeField] private float _moveToOppositeDirectionDuration = 0.1f;
        [SerializeField] private float _moveToEndPositionDuration = 0.25f;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Play(Vector2 screenPosition, float zoomFactor, Action onComplete = null)
        {
            bool onCompleteInvoked = false;
            float zoom = Mathf.Lerp(0.3f, 1f, zoomFactor);
            
            Vector2 viewportPosition = _camera.ScreenToViewportPoint(screenPosition) - Vector3.one * 0.5f;
            Vector2 startLocalPosition = new Vector2(viewportPosition.x * _container.sizeDelta.x, viewportPosition.y * _container.sizeDelta.y);
            
            for (int i = 0; i < _spawnCount; i++)
            {
                var coinInstance = Instantiate(_coinTemplate, _container).rectTransform;
                coinInstance.localPosition = startLocalPosition + Vector2.right * Random.Range(-_spawnRadius, _spawnRadius) * zoom / 2;
                coinInstance.localScale *= zoom;

                var endOppositePosition = new Vector2(startLocalPosition.x, startLocalPosition.y + Random.Range(-_spawnRadius * 1.75f, 0) * zoom);
                float randomDuration = CalculateTime(_moveToEndPositionDuration);
                var sequence = DOTween.Sequence();
                
                sequence.Append(coinInstance.DOLocalMoveY(endOppositePosition.y, CalculateTime(_moveToOppositeDirectionDuration)));
                
                sequence.AppendCallback(() =>
                {
                    coinInstance.DOScale(0.3f, randomDuration);
                    coinInstance.DOMove(_endPoint.position, randomDuration);
                });
                
                sequence.AppendInterval(randomDuration);
                
                sequence.AppendCallback(() =>
                {
                    coinInstance.DOKill();
                    Destroy(coinInstance.gameObject);

                    if (onCompleteInvoked)
                        return;

                    onComplete?.Invoke();
                    onCompleteInvoked = true;
                });
            }
        }

        private static float CalculateTime(float time)
        {
            return time + Random.Range(0, time) + 0.45f;
        }
    }
}
