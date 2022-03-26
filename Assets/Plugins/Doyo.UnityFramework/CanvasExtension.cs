using UnityEngine;
using static UnityEngine.RectTransform;

namespace Doyo.UnityFramework
{
    public static class CanvasExternal
    {
        public enum Alignment
        {
            MiddleCenter,
            UpperLeft,
            UpperRight,
            LowerLeft,
            LowerRight,
        }

        public static Vector2 GetRelativeMousePosition(this Canvas canvas, Vector2 mousePosition)
        {
            float relativeX, relativeY;
            relativeX = canvas.GetRelativePositionForAxis(Axis.Horizontal, mousePosition.x);
            relativeY = canvas.GetRelativePositionForAxis(Axis.Vertical, mousePosition.y);
            return new Vector2(relativeX, relativeY);
        }

        public static Vector2 GetRelativeMousePosition(this Canvas canvas, Vector3 mousePosition)
        {
            return canvas.GetRelativeMousePosition((Vector2)mousePosition);
        }

        public static float GetRelativePositionForAxis(this Canvas canvas, Axis axis, float axisPos)
        {
            var realResolution = canvas.GetComponent<RectTransform>();
            float relativePos = 0;
            switch (axis)
            {
                case Axis.Horizontal:
                    relativePos = axisPos / Screen.width * realResolution.rect.width;
                    break;
                case Axis.Vertical:
                    relativePos = axisPos / Screen.height * realResolution.rect.height;
                    break;
                default:
                    break;
            }
            return relativePos;
        }

        public static Vector2 GetMousePos(this Canvas canvas, Alignment alignment = Alignment.LowerLeft)
        {
            // 鼠标本身就是相对屏幕左下角的
            var canvasRect = canvas.transform as RectTransform;
            Vector3 pos;
            switch (alignment)
            {
                case Alignment.LowerRight:
                    pos = Input.mousePosition - new Vector3(canvasRect.rect.width, 0, 0);
                    break;
                case Alignment.MiddleCenter:
                    pos = Input.mousePosition
                        - new Vector3(canvasRect.rect.width * 0.5f, canvasRect.rect.height * 0.5f, 0);
                    break;
                case Alignment.UpperLeft:
                    pos = Input.mousePosition - new Vector3(0, canvasRect.rect.height, 0);
                    break;
                case Alignment.UpperRight:
                    pos = Input.mousePosition
                        - new Vector3(canvasRect.rect.width, canvasRect.rect.height, 0);
                    break;
                case Alignment.LowerLeft:
                default:
                    pos = Input.mousePosition;
                    break;
            }
            return canvas.GetRelativeMousePosition(pos);
        }

        public static Vector2 GetRelativePos(this Canvas canvas, Transform child, Alignment alignment)
        {
            Vector2 pos = GetRelativePos(canvas, child);
            RectTransform canvasRect = canvas.transform as RectTransform;
            float width = canvasRect.rect.width;
            float height = canvasRect.rect.height;
            switch (alignment)
            {
                case Alignment.UpperLeft:
                    pos = new Vector2(width * 0.5f + pos.x, pos.y - height * 0.5f);
                    break;
                case Alignment.UpperRight:
                    pos = new Vector2(pos.x - width * 0.5f, pos.y - height * 0.5f);
                    break;
                case Alignment.LowerLeft:
                    pos = new Vector2(width * 0.5f + pos.x, height * 0.5f + pos.y);
                    break;
                case Alignment.LowerRight:
                    pos = new Vector2(pos.x - width * 0.5f, height * 0.5f + pos.y);
                    break;
                case Alignment.MiddleCenter:
                    break;
            }
            return pos;
        }

        public static Vector2 GetRelativePos(this Canvas canvas, Transform child)
        {
            Vector2 pos = child.localPosition;
            RectTransform rectT = child as RectTransform;
            pos += new Vector2((0.5f - rectT.pivot.x) * rectT.rect.width, (0.5f - rectT.pivot.y) * rectT.rect.height);
            if (child.parent == canvas.transform)
            {
                return pos;
            }
            else
            {
                return pos + _GetRelativePos(canvas, child.parent);
            }
        }

        private static Vector2 _GetRelativePos(Canvas canvas, Transform child)
        {
            Vector2 pos = child.localPosition;
            if (child.parent == canvas.transform)
            {
                return pos;
            }
            else
            {
                return pos + _GetRelativePos(canvas, child.parent);
            }
        }
    }

}
