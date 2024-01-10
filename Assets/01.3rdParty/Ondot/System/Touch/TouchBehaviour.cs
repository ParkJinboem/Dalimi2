using UnityEngine;

namespace OnDot.System.Touch
{
    public class TouchBehaviour : MonoBehaviour
    {
        public delegate void TouchDownHandler();
        public static event TouchDownHandler OnTouchDown;

        public delegate void TouchUpHandler();
        public static event TouchUpHandler OnTouchUp;

        public TouchController touchController;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DragStart();
            }

            if (Input.GetMouseButtonUp(0))
            {
                DragEnd();
            }
        }

        private void DragStart()
        {
            if (!touchController.CanTouch)
            {
                return;
            }

            OnTouchDown?.Invoke();
        }

        private void DragEnd()
        {
            OnTouchUp?.Invoke();
        }
    }
}