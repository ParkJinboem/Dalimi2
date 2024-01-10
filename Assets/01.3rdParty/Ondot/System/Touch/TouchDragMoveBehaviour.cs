using UnityEngine;

namespace OnDot.System.Touch
{
    public abstract class TouchDragMoveBehaviour : MonoBehaviour
    {
        public abstract void DragDown();
        public abstract void DragMove(Vector3 startPosition, Vector3 currentPosition);
        public abstract void DragUp();

        private Vector3 startPosition;

        private void HandlerTouchDragDown()
        {
            startPosition = transform.position;
            DragDown();
        }

        private void HandlerTouchDragMove(Vector3 movePosition)
        {
            DragMove(startPosition, startPosition + movePosition);
        }

        private void HandlerTouchDragUp()
        {
            DragUp();
        }

        protected virtual void OnEnable()
        {
            TouchDragBehaviour.OnTouchDragDown += HandlerTouchDragDown;
            TouchDragBehaviour.OnTouchDragMove += HandlerTouchDragMove;
            TouchDragBehaviour.OnTouchDragUp += HandlerTouchDragUp;
        }

        protected virtual void OnDisable()
        {
            TouchDragBehaviour.OnTouchDragDown -= HandlerTouchDragDown;
            TouchDragBehaviour.OnTouchDragMove -= HandlerTouchDragMove;
            TouchDragBehaviour.OnTouchDragUp -= HandlerTouchDragUp;
        }
    }
}