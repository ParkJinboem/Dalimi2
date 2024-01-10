using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace OnDot.Util
{
    public class UnSafeArea : MonoBehaviour
    {
        [SerializeField] private RectOffset initOffset;

        [Space]
        [SerializeField] private LayoutGroup layoutGroup;        
        [SerializeField] private RectOffset initLayoutPadding;

        [Space]
        [SerializeField] private bool useLeft = true;
        [SerializeField] private bool useBottom = true;
        [SerializeField] private bool useRight = true;
        [SerializeField] private bool useTop = true;

        private bool isApply = false;
        private RectTransform Panel;
        private Rect LastSafeArea = new Rect(0, 0, 0, 0);

        private void Awake()
        {
            Panel = GetComponent<RectTransform>();

            Refresh();
        }

        private void Update()
        {
            if (isApply)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            Rect safeArea = GetSafeArea();
            if (safeArea != LastSafeArea)
            {
                LastSafeArea = safeArea;
                ApplySafeArea();
            }
        }

        private Rect GetSafeArea()
        {
            return Screen.safeArea;
        }

        private void ApplySafeArea()
        {
            StopAllCoroutines();
            StartCoroutine(IApplySafeArea());
        }

        IEnumerator IApplySafeArea()
        {
            if (isApply)
            {
                yield return new WaitForEndOfFrame();
            }

            Rect r = LastSafeArea;

            Vector2 anchorMin = r.position;
            Vector2 anchorMax = r.position + r.size;

            Vector2 offsetMin = anchorMin;
            Vector2 offsetMax = new Vector2(Screen.width - anchorMax.x, Screen.height - anchorMax.y);
            if (!useLeft) offsetMin.x = initOffset.left;
            if (!useBottom) offsetMin.y = initOffset.bottom;
            if (!useRight) offsetMax.x = -initOffset.right;
            if (!useTop) offsetMax.y = -initOffset.top;
            if (layoutGroup)
            {
                layoutGroup.padding.left = (int)(initLayoutPadding.left + offsetMin.x);
                layoutGroup.padding.bottom = (int)(initLayoutPadding.bottom + offsetMin.y);
                layoutGroup.padding.right = (int)(initLayoutPadding.right + offsetMax.x);
                layoutGroup.padding.top = (int)(initLayoutPadding.top + offsetMax.y);
            }
            Panel.offsetMin = -offsetMin;
            Panel.offsetMax = offsetMax;

            isApply = true;
        }

        public void ChangeLayoutPadding(RectOffset layoutPadding)
        {
            initLayoutPadding = layoutPadding;

            ApplySafeArea();
        }
    }
}