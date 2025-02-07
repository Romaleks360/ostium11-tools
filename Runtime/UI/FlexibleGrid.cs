using Ostium11.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Ostium11.UI
{
    [ExecuteAlways]
    public class FlexibleGrid : MonoBehaviour, ILayoutGroup, ILayoutElement
    {
        [SerializeField] Vector2 _cellSize = new Vector2(100, 100);
        [SerializeField] Vector2 _spacing;

        int _calculatedCellCountX = 1;
        Vector2 _calculatedCellSize;
        bool _isDirty = false;

        public float minWidth => 0;
        public float preferredWidth => this.rectTransform().sizeDelta.x;
        public float flexibleWidth => 0;
        public float minHeight => 0;
        public float preferredHeight => this.rectTransform().sizeDelta.y;
        public float flexibleHeight => 0;
        public int layoutPriority => 0;
        public Vector2 CellSize { get => _cellSize; set => _cellSize = value; }
        public Vector2 Spacing { get => _spacing; set => _spacing = value; }

        void Start() => UpdateLayout();

        void OnRectTransformDimensionsChange() => UpdateLayout();

        void OnTransformChildrenChanged() => _isDirty = true;

        void Update()
        {
            if (!_isDirty) return;
            _isDirty = false;
            UpdateLayout();
        }

#if UNITY_EDITOR
        void OnValidate() => LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform());
#endif
        public void SetLayoutHorizontal() => UpdateLayout();

        public void SetLayoutVertical() { }

        public void CalculateLayoutInputHorizontal() { }

        public void CalculateLayoutInputVertical() { }

        public Rect GetItemPositionAt(int index)
        {
            var col = index % _calculatedCellCountX;
            var row = index / _calculatedCellCountX;
            var size = _calculatedCellSize;
            var pos = size + _spacing;
            pos.Scale(new(col, row));
            return new Rect(pos, size);
        }

        void UpdateLayout()
        {
            float width = this.rectTransform().rect.width;
            int cellCountX = Mathf.Max(1, (int)((width + _spacing.x + 0.001f) / (_cellSize.x + _spacing.x)));
            float spaceLeft = width - cellCountX * (_cellSize.x + _spacing.x) + _spacing.x;
            float factor = (spaceLeft / cellCountX + _cellSize.x) / _cellSize.x;
            var newCellSize = _cellSize * factor;
            _calculatedCellCountX = cellCountX;
            _calculatedCellSize = newCellSize;

            var pivot = new Vector2(0, 1);
            var pos = Vector2.zero;
            int childCount = 0;
            foreach (RectTransform tr in transform)
            {
                if (!tr.gameObject.activeSelf)
                    continue;

                tr.anchorMin = pivot;
                tr.anchorMax = pivot;
                tr.pivot = pivot;

                tr.anchoredPosition = pos;
                tr.sizeDelta = newCellSize;

                pos.x += newCellSize.x + _spacing.x;
                childCount++;
                if (childCount % cellCountX == 0)
                {
                    pos.x = 0;
                    pos.y -= newCellSize.y + _spacing.y;
                }
            }

            int rows = Mathf.Max(1, childCount / cellCountX + (childCount % cellCountX > 0 ? 1 : 0));
            float height = rows * (newCellSize.y + _spacing.y) - _spacing.y;
            this.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}