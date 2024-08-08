using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using YellowSquad.Anthill.Core.GameTime;
using Random = UnityEngine.Random;

namespace YellowSquad.Anthill.Meta
{
    public class AddMoneyAnimation : MonoBehaviour
    {
        [SerializeField] private Image _coinTemplate;
        [SerializeField] private RectTransform _container;
        [SerializeField] private RectTransform _endPoint;
        [SerializeField] private TimeScale _timeScale;
        [Header("Animation Settings")]
        [SerializeField] private int _spawnCount = 10;
        [SerializeField] private float _spawnRadius = 350;
        [SerializeField] private float _moveToOppositeDirectionDuration = 0.2f;
        [SerializeField] private float _moveToEndPositionDuration = 0.45f;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Play(Vector3 screenPosition, float normalizedZoomFactor, Action onComplete = null)
        {
            bool onCompleteInvoked = false;

            float zoom = Mathf.Lerp(0.3f, 1f, normalizedZoomFactor);
            var startLocalPosition = ConvertWorldToContainerSpacePosition(screenPosition);

            for (int i = 0; i < _spawnCount; i++)
            {
                var coinInstance = Instantiate(_coinTemplate, _container).rectTransform;
                coinInstance.position = startLocalPosition + Vector2.right * Random.Range(-_spawnRadius, _spawnRadius) * zoom / 2;
                coinInstance.localScale *= zoom;

                coinInstance.DOMoveY(startLocalPosition.y + Random.Range(-_spawnRadius * 1.75f, 0) * zoom,
                    CalculateTime(_moveToOppositeDirectionDuration + Random.Range(0, _moveToOppositeDirectionDuration), zoom))
                        .OnComplete(() =>
                        {
                            float randomDuration = CalculateTime(_moveToEndPositionDuration + Random.Range(0, _moveToEndPositionDuration), zoom);
                            coinInstance.DOScale(0.3f, randomDuration);
                            coinInstance.DOMove(_endPoint.position, randomDuration)
                                .OnComplete(() =>
                                {
                                    Destroy(coinInstance.gameObject);

                                    if (onCompleteInvoked) 
                                        return;
                                    
                                    onComplete?.Invoke();
                                    onCompleteInvoked = true;
                                });
                        });
            }
        }

        private Vector2 ConvertWorldToContainerSpacePosition(Vector3 screenPosition)
        {
            Vector2 viewportPosition = _camera.ScreenToViewportPoint(screenPosition);
            Vector2 containerSpacePosition = new Vector2(viewportPosition.x * _container.sizeDelta.x, viewportPosition.y * _container.sizeDelta.y);

            return containerSpacePosition;
        }

        private float CalculateTime(float time, float zoom)
        {
            return time * _timeScale.Value + Mathf.Lerp(1f, 0f, zoom);
        }
    }
}
