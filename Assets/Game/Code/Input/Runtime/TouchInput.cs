using UnityEngine;

namespace YellowSquad.Anthill.UserInput
{
    public class TouchInput : IInput
    {
        private const float ZoomFactor = 0.035f;

        private int _lastTouchFingerId = -1;
        private Vector2 _lastTouchPosition;
        private Touch _firstZoomTouch;
        private Touch _secondZoomTouch;

        public bool PointerDown => Input.GetMouseButtonDown(0);
        public bool PointerUp => Input.GetMouseButtonUp(0) && Input.touchCount == 0;

        public Vector2 PointerPosition
        {
            get
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    var touch = Input.GetTouch(i);

                    if (_lastTouchFingerId == -1)
                        _lastTouchFingerId = touch.fingerId;
                    
                    if (touch.fingerId != _lastTouchFingerId)
                        continue;

                    _lastTouchPosition = touch.position;

                    if (touch.phase == TouchPhase.Ended)
                        _lastTouchFingerId = -1;
                    
                    break;
                }

                if (Input.touchCount == 0)
                    _lastTouchFingerId = -1;
                
                return _lastTouchPosition;
            }
        }

        public float ZoomDelta
        {
            get
            {
                if (Input.touchCount < 2)
                    return 0;
                
                var newFirstTouch = Input.GetTouch(0);
                var newSecondTouch = Input.GetTouch(1);

                if (newFirstTouch.phase == TouchPhase.Began || newSecondTouch.phase == TouchPhase.Began)
                {
                    _firstZoomTouch = newFirstTouch;
                    _secondZoomTouch = newSecondTouch;
                    return 0;
                }

                float startDistance = (_firstZoomTouch.position - _secondZoomTouch.position).magnitude;
                float newDistance = (newFirstTouch.position - newSecondTouch.position).magnitude;

                _firstZoomTouch = newFirstTouch;
                _secondZoomTouch = newSecondTouch;

                return (newDistance - startDistance) * ZoomFactor;
            }
        }
    }
}