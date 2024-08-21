using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ashen.NodeTreeSystem
{
    [ExecuteAlways]
    public class NodeSquareLineDrawerUI : MaskableGraphic
    {
        public Transform reference;

        public List<UILocation> locations;

        public float width;

        public Color enabledColor;
        public Color disabledColor;

        public I_NodeRequirements requirement;


        public void Update()
        {
            if (locations == null)
            {
                return;
            }
            for (int x = 0; x < locations.Count; x++)
            {
                if (locations[x].rectTransform.hasChanged)
                {
                    this.SetVerticesDirty();
                }
            }
        }

        public void Enable()
        {
            color = enabledColor;
        }

        public void Disable()
        {
            color = disabledColor;
        }

        private void UpdateVector3ForSide(Vector3 vector3, RectSide rectSide, Coord coord, Rect rect)
        {
            switch (rectSide)
            {
                case RectSide.CENTER:
                    break;
                case RectSide.BOTTOM:
                    if (coord != Coord.X)
                    {
                        vector3.y -= rect.height / 2f;
                    }
                    break;
                case RectSide.LEFT:
                    if (coord != Coord.Y)
                    {
                        vector3.x -= rect.width / 2f;
                    }
                    break;
                case RectSide.RIGHT:
                    if (coord != Coord.Y)
                    {
                        vector3.x += rect.width / 2f;
                    }
                    break;
                case RectSide.TOP:
                    if (coord != Coord.X)
                    {
                        vector3.y += rect.height / 2f;
                    }
                    break;
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (locations == null || locations.Count < 2)
            {
                return;
            }
            UILocation A = locations[0];
            Vector3 startLocation = reference.transform.InverseTransformPoint(A.rectTransform.position);
            Coord coord = A.coord;
            switch (A.rectSide)
            {
                case RectSide.CENTER:
                    break;
                case RectSide.BOTTOM:
                    if (coord != Coord.X)
                    {
                        startLocation.y -= A.rectTransform.rect.height / 2f;
                    }
                    break;
                case RectSide.LEFT:
                    if (coord != Coord.Y)
                    {
                        startLocation.x -= A.rectTransform.rect.width / 2f;
                    }
                    break;
                case RectSide.RIGHT:
                    if (coord != Coord.Y)
                    {
                        startLocation.x += A.rectTransform.rect.width / 2f;
                    }
                    break;
                case RectSide.TOP:
                    if (coord != Coord.X)
                    {
                        startLocation.y += A.rectTransform.rect.height / 2f;
                    }
                    break;
            }
            DrawnResult result = new DrawnResult
            {
                lastDrawn = LastDrawn.NONE,
                lastLocation = startLocation,
                vertsOffset = 0
            };
            for (int x = 1; x < locations.Count; x++)
            {
                UILocation B = locations[x];
                result = ConnectTwoPoints(B, vh, result);
            }
        }

        private DrawnResult ConnectTwoPoints(UILocation locationB, VertexHelper vh, DrawnResult result)
        {
            Vector3 B = reference.transform.InverseTransformPoint(locationB.rectTransform.position);

            float xA = result.lastLocation.x;
            float yA = result.lastLocation.y;
            float xB = B.x;
            float yB = B.y;

            float lastDrawnFix = width / 2f;

            switch (result.lastDrawn)
            {
                case LastDrawn.NONE:
                    lastDrawnFix = 0f;
                    break;
            }

            Coord coord = locationB.coord;

            switch (coord)
            {
                case Coord.BOTH:
                    break;
                case Coord.X:
                    yB = yA;
                    break;
                case Coord.Y:
                    xB = xA;
                    break;
            }

            switch (locationB.rectSide)
            {
                case RectSide.CENTER:
                    break;
                case RectSide.BOTTOM:
                    if (coord != Coord.X)
                    {
                        yB -= locationB.rectTransform.rect.height / 2f;
                    }
                    break;
                case RectSide.LEFT:
                    if (coord != Coord.Y)
                    {
                        xB -= locationB.rectTransform.rect.width / 2f;
                    }
                    break;
                case RectSide.RIGHT:
                    if (coord != Coord.Y)
                    {
                        xB += locationB.rectTransform.rect.width / 2f;
                    }
                    break;
                case RectSide.TOP:
                    if (coord != Coord.X)
                    {
                        yB += locationB.rectTransform.rect.height / 2f;
                    }
                    break;
            }

            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            if (yA != yB && xA != xB)
            {
                if (locationB.bothResolver == BothResolver.MIDYX)
                {
                    float percentage = locationB.splitPercentage;
                    if (percentage < 0)
                    {
                        percentage = 0;
                    }
                    if (percentage > 100)
                    {
                        percentage = 100;
                    }
                    percentage = percentage / 100f;
                    float midY = yA - (percentage * (yA - yB));
                    float yFix = result.lastDrawn == LastDrawn.HORIZONTAL ? (yA > yB ? width / 2f : -width / 2f) : 0f;
                    float fix = (yA > yB ? width / 2f : -width / 2f);
                    DrawLine(xA, yA + yFix, xA, midY - fix, result.vertsOffset, vh);
                    result.vertsOffset += 4;

                    DrawLine(xA, midY, xB, midY, result.vertsOffset, vh);
                    result.vertsOffset += 4;

                    DrawLine(xB, midY + fix, xB, yB, result.vertsOffset, vh);
                    result.vertsOffset += 4;
                    return new DrawnResult
                    {
                        lastDrawn = LastDrawn.VERTICAL,
                        lastLocation = new Vector3(xB, yB),
                        vertsOffset = result.vertsOffset
                    };
                }
                else if (locationB.bothResolver == BothResolver.MIDXY)
                {
                    float percentage = locationB.splitPercentage;
                    if (percentage < 0)
                    {
                        percentage = 0;
                    }
                    if (percentage > 100)
                    {
                        percentage = 100;
                    }
                    percentage = percentage / 100f;
                    float midX = xA - (percentage * (xA - xB));
                    float xFix = result.lastDrawn == LastDrawn.VERTICAL ? (xA > xB ? width / 2f : -width / 2f) : 0f;
                    float fix = (xA < xB ? width / 2f : -width / 2f);
                    DrawLine(xA + xFix, yA, midX + fix, yA, result.vertsOffset, vh);
                    result.vertsOffset += 4;

                    DrawLine(midX, yA, midX, yB, result.vertsOffset, vh);
                    result.vertsOffset += 4;

                    DrawLine(midX - fix, yB, xB, yB, result.vertsOffset, vh);
                    result.vertsOffset += 4;
                    return new DrawnResult
                    {
                        lastDrawn = LastDrawn.HORIZONTAL,
                        lastLocation = new Vector3(xB, yB),
                        vertsOffset = result.vertsOffset
                    };
                }
                else if (locationB.bothResolver == BothResolver.XY)
                {
                    float xAFix = result.lastDrawn == LastDrawn.VERTICAL ? (xA > xB ? width / 2f : -width / 2f) : 0f;
                    DrawLine(xA + xAFix, yA, xB, yA, result.vertsOffset, vh);
                    result.vertsOffset += 4;

                    float yAFix = (yA > yB ? width / 2f : -width / 2f);
                    DrawLine(xB, yA + yAFix, xB, yB, result.vertsOffset, vh);
                    result.vertsOffset += 4;

                    return new DrawnResult
                    {
                        lastDrawn = LastDrawn.VERTICAL,
                        lastLocation = new Vector3(xB, yB),
                        vertsOffset = result.vertsOffset
                    };
                }
                else if (locationB.bothResolver == BothResolver.YX)
                {
                    float yAFix = result.lastDrawn == LastDrawn.HORIZONTAL ? (yA > yB ? width / 2f : -width / 2f) : 0f;
                    DrawLine(xA, yA + yAFix, xA, yB, result.vertsOffset, vh);
                    result.vertsOffset += 4;

                    float xAFix = (xA > xB ? width / 2f : -width / 2f);
                    DrawLine(xA + xAFix, yB, xB, yB, result.vertsOffset, vh);
                    result.vertsOffset += 4;

                    return new DrawnResult
                    {
                        lastDrawn = LastDrawn.HORIZONTAL,
                        lastLocation = new Vector3(xB, yB),
                        vertsOffset = result.vertsOffset
                    };
                }
            }
            else if (xA != xB)
            {
                float xAFix = result.lastDrawn == LastDrawn.VERTICAL ? (xA > xB ? width / 2f : -width / 2f) : 0f;
                DrawLine(xA + xAFix, yA, xB, yA, result.vertsOffset, vh);
                result.vertsOffset += 4;
                return new DrawnResult
                {
                    lastDrawn = LastDrawn.HORIZONTAL,
                    lastLocation = new Vector3(xB, yA),
                    vertsOffset = result.vertsOffset
                };
            }
            else if (yA != yB)
            {
                float yAFix = result.lastDrawn == LastDrawn.HORIZONTAL ? (yA > yB ? width / 2f : -width / 2f) : 0f;
                DrawLine(xA, yA + yAFix, xA, yB, result.vertsOffset, vh);
                result.vertsOffset += 4;
                return new DrawnResult
                {
                    lastDrawn = LastDrawn.VERTICAL,
                    lastLocation = new Vector3(xA, yB),
                    vertsOffset = result.vertsOffset
                };
            }
            return result;
        }

        private void DrawLine(float xA, float yA, float xB, float yB, int offset, VertexHelper vh)
        {
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            float width = this.width / 2f;

            if (yA != yB)
            {
                if (yB < yA)
                {
                    // UP
                    float temp = yA;
                    yA = yB;
                    yB = temp;
                }
                // DOWN
                vertex.position = new Vector3(xA - width, yA);
                vh.AddVert(vertex);
                vertex.position = new Vector3(xA - width, yB);
                vh.AddVert(vertex);
                vertex.position = new Vector3(xA + width, yB);
                vh.AddVert(vertex);
                //vertex.position = new Vector3(xA + width, yB);
                vertex.position = new Vector3(xA + width, yA);
                //vertex.position = new Vector3(xA - width, yA);
                vh.AddVert(vertex);

                vh.AddTriangle(offset + 0, offset + 1, offset + 2);
                vh.AddTriangle(offset + 2, offset + 3, offset + 0);
            }
            else if (xA != xB)
            {
                if (xB < xA)
                {
                    // LEFT
                    float temp = xA;
                    xA = xB;
                    xB = temp;
                }
                // RIGHT
                // DOWN
                vertex.position = new Vector3(xA, yA - width);
                vh.AddVert(vertex);
                vertex.position = new Vector3(xB, yA - width);
                vh.AddVert(vertex);
                vertex.position = new Vector3(xB, yA + width);
                vh.AddVert(vertex);
                //vertex.position = new Vector3(xB, yA + width);
                vertex.position = new Vector3(xA, yA + width);
                //vertex.position = new Vector3(xA, yA - width);
                vh.AddVert(vertex);

                vh.AddTriangle(offset + 0, offset + 1, offset + 2);
                vh.AddTriangle(offset + 2, offset + 3, offset + 0);
            }
        }
    }

    public enum LastDrawn
    {
        HORIZONTAL, VERTICAL, NONE
    }

    public struct DrawnResult
    {
        public Vector3 lastLocation;
        public LastDrawn lastDrawn;
        public int vertsOffset;
    }
}