using UnityEngine;

namespace YellowSquad.Anthill.UserInput
{
    public class TouchInput : IInput
    {
        private Touch _firstZoomTouch;
        private Touch _secondZoomTouch;

        public bool PointerDown => Input.GetMouseButtonDown(0);
        public bool PointerUp => Input.GetMouseButtonUp(0);

        public Vector2 PointerPosition => Input.touchCount > 0 ? Input.touches[0].position : Input.mousePosition;

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

                return newDistance - startDistance;
            }
        }
    }
}