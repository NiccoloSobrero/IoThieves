using UnityEngine;
using UnityEngine.EventSystems;

namespace Minigames
{
    public class WindowDragHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform window;
        [SerializeField] private RectTransform clampWithin;

        private Canvas _canvas;
        
        private Vector2 _pointerOffset;
        private bool _isDragging;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (window)
            {
                window.SetAsLastSibling();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (window == null || _canvas == null)
            {
                return;
            }
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                window, eventData.position, eventData.pressEventCamera, out var localPoint);
            
            _pointerOffset = localPoint;
            _isDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging || window == null || _canvas == null || IsMouseOutsideScreen())
            {
                return;
            }
            
            RectTransform parent = window.parent as RectTransform;
            if (parent == null) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent, eventData.position, eventData.pressEventCamera, out var parentLocalPoint);

            Vector2 pivot = window.pivot;
            Rect winRect = window.rect;
            
            Vector2 windowLocalToAnchored = new Vector2(
                winRect.width  * (0.5f - pivot.x),
                winRect.height * (0.5f - pivot.y)
            );

            Vector2 targetAnchored = parentLocalPoint - (_pointerOffset + windowLocalToAnchored);

            if (clampWithin != null)
            {
                targetAnchored = ClampToBounds(targetAnchored, window, clampWithin);
            }

            window.anchoredPosition = targetAnchored;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
        }
        
        private Vector2 ClampToBounds(Vector2 desiredAnchoredPos, RectTransform win, RectTransform bounds)
        {
            var bRect = bounds.rect;
            var wRect = win.rect;

            var pivot = win.pivot;
            float left   = bRect.xMin + wRect.width  * (0f + pivot.x);
            float right  = bRect.xMax - wRect.width  * (1f - pivot.x);
            float bottom = bRect.yMin + wRect.height * (0f + pivot.y);
            float top    = bRect.yMax - wRect.height * (1f - pivot.y);

            float x = Mathf.Clamp(desiredAnchoredPos.x, left, right);
            float y = Mathf.Clamp(desiredAnchoredPos.y, bottom, top);
            return new Vector2(x, y);
        }

        private bool IsMouseOutsideScreen()
        {
            return Input.mousePosition.x > Screen.width || Input.mousePosition.y > Screen.height ||
                   Input.mousePosition.x < 0f || Input.mousePosition.y < 0f;
        }
    }
}