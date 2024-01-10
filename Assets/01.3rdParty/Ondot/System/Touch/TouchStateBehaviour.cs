using UnityEngine;

namespace OnDot.System.Touch
{
    public abstract class TouchStateBehaviour : MonoBehaviour
    {
        public abstract void TouchDown();
        public abstract void TouchUp();

        private void HandlerTouchDown()
        {
            TouchDown();
        }

        private void HandlerTouchUp()
        {
            TouchUp();
        }

        protected virtual void Awake()
        {
            
        }

        protected virtual void OnEnable()
        {
            TouchBehaviour.OnTouchDown += HandlerTouchDown;
            TouchBehaviour.OnTouchUp += HandlerTouchUp;
        }

        protected virtual void OnDisable()
        {
            TouchBehaviour.OnTouchDown -= HandlerTouchDown;
            TouchBehaviour.OnTouchUp -= HandlerTouchUp;
        }

        protected virtual void OnDestroy()
        {
            
        }
    }
}