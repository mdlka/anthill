using System;
using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.Anthill.UserInput
{
    public class TouchInput : IInput
    {
        private const float ZoomFactor = 0.035f;

        private readonly Dictionary<int, Vector2> _touchesPositions = new();
        private readonly Queue<int> _pointersDown = new();
        private readonly Queue<int> _pointersUp = new();
        
        private Touch _firstZoomTouch;
        private Touch _secondZoomTouch;

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

        public bool HasPointerDown => _pointersDown.Count > 0;
        public bool HasPointerUp => _pointersUp.Count > 0;
        
        public void Update()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began)
                    _pointersDown.Enqueue(touch.fingerId);
                else if (touch.phase == TouchPhase.Ended) 
                    _pointersUp.Enqueue(touch.fingerId);

                if (_touchesPositions.ContainsKey(touch.fingerId))
                    _touchesPositions.Add(touch.fingerId, touch.position);

                _touchesPositions[touch.fingerId] = touch.position;
            }
        }

        public int ReadPointerDown()
        {
            if (HasPointerDown == false)
                throw new InvalidOperationException();

            return _pointersDown.Dequeue();
        }

        public int ReadPointerUp()
        {
            if (HasPointerUp == false)
                throw new InvalidOperationException();

            return _pointersUp.Dequeue();
        }

        public Vector2 PointerPosition(int pointerId)
        {
            if (_touchesPositions.ContainsKey(pointerId) == false)
                throw new ArgumentOutOfRangeException();

            return _touchesPositions[pointerId];
        }
    }
}