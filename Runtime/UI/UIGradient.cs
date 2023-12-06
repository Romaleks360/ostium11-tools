using UnityEngine;
using UnityEngine.UI;

namespace Ostium11.UI
{
    [AddComponentMenu("UI/Effects/UIGradient")]
    public class UIGradient : BaseMeshEffect
    {
        public enum AxisType { Horizontal, Vertical }

        public Color Color1 = Color.white;
        public Color Color2 = Color.black;
        public AxisType Axis = AxisType.Vertical;
        public bool Reverse;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            Rect rect = graphic.rectTransform.rect;
            UIVertex vertex = default;
            Vector2 normPos;
            var axis = (int)Axis;
            var color1 = Color1;
            var color2 = Color2;
            if (Reverse)
                (color1, color2) = (color2, color1);

            for (int i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref vertex, i);
                normPos = ((Vector2)vertex.position - rect.position) / rect.size;
                vertex.color *= Color.Lerp(color1, color2, normPos[axis]);
                vh.SetUIVertex(vertex, i);
            }
        }
    }
}