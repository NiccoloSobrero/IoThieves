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
            // We clamp window.anchoredPosition, which is in win.parent local space
            var parent = win.parent as RectTransform;
            if (parent == null) return desiredAnchoredPos;

            // Get bounds corners in world, then convert to parent local space
            Vector3[] corners = new Vector3[4];
            bounds.GetWorldCorners(corners); // 0=BL,1=TL,2=TR,3=BR in world space

            Vector2 bl = parent.InverseTransformPoint(corners[0]);
            Vector2 tl = parent.InverseTransformPoint(corners[1]);
            Vector2 tr = parent.InverseTransformPoint(corners[2]);
            Vector2 br = parent.InverseTransformPoint(corners[3]);

            float xMin = Mathf.Min(bl.x, tl.x, tr.x, br.x);
            float xMax = Mathf.Max(bl.x, tl.x, tr.x, br.x);
            float yMin = Mathf.Min(bl.y, tl.y, tr.y, br.y);
            float yMax = Mathf.Max(bl.y, tl.y, tr.y, br.y);

            // Window size in its own local space is fine (RectTransform rect is local size)
            Rect wRect = win.rect;
            Vector2 pivot = win.pivot;

            // Clamp desired anchored position so the window stays fully inside bounds
            float left   = xMin + wRect.width  * (0f + pivot.x);
            float right  = xMax - wRect.width  * (1f - pivot.x);
            float bottom = yMin + wRect.height * (0f + pivot.y);
            float top    = yMax - wRect.height * (1f - pivot.y);

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