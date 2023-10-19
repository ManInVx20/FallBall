using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    [System.Serializable]
    public class Path
    {
        [SerializeField, HideInInspector]
        private List<Vector2> _pointList;
        [SerializeField, HideInInspector]
        private bool _isClosed;
        [SerializeField, HideInInspector]
        private bool _autoSetControlPoints;

        public Path(Vector2 centerPos)
        {
            _pointList = new List<Vector2>
            {
                centerPos + Vector2.left,
                centerPos + (Vector2.left + Vector2.up) * 0.5f,
                centerPos + (Vector2.right + Vector2.down) * 0.5f,
                centerPos + Vector2.right
            };
        }

        public Vector2 this[int i]
        {
            get
            {
                return _pointList[i];
            }
        }

        public bool IsClosed
        {
            get
            {
                return _isClosed;
            }
            set
            {
                if (_isClosed != value)
                {
                    _isClosed = !_isClosed;

                    if (_isClosed)
                    {
                        _pointList.Add(_pointList[_pointList.Count - 1] * 2.0f - _pointList[_pointList.Count - 2]);
                        _pointList.Add(_pointList[0] * 2.0f - _pointList[1]);

                        if (_autoSetControlPoints)
                        {
                            AutoSetAnchorControlPoints(0);
                            AutoSetAnchorControlPoints(_pointList.Count - 3);
                        }
                    }
                    else
                    {
                        _pointList.RemoveRange(_pointList.Count - 2, 2);

                        if (_autoSetControlPoints)
                        {
                            AutoSetStartAndEndControlPoints();
                        }
                    }
                }
            }
        }

        public bool AutoSetControlPoints
        {
            get
            {
                return _autoSetControlPoints;
            }
            set
            {
                if (_autoSetControlPoints != value)
                {
                    _autoSetControlPoints = value;
                    if (_autoSetControlPoints)
                    {
                        AutoSetAllControlPoints();
                    }
                }
            }
        }

        public int PointCount
        {
            get
            {
                return _pointList.Count;
            }
        }

        public int SegmentCount
        {
            get
            {
                return _pointList.Count / 3;
            }
        }

        public void AddSegment(Vector2 anchorPos)
        {
            _pointList.Add(_pointList[_pointList.Count - 1] * 2.0f - _pointList[_pointList.Count - 2]);
            _pointList.Add((_pointList[_pointList.Count - 1] + anchorPos) * 0.5f);
            _pointList.Add(anchorPos);

            if (_autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(_pointList.Count - 1);
            }
        }

        public void SplitSegment(Vector2 anchorPos, int segmentIndex)
        {
            _pointList.InsertRange(segmentIndex * 3 + 2, new Vector2[]
            {
                Vector2.zero,
                anchorPos,
                Vector2.zero
            });

            if (_autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(segmentIndex * 3 + 3);
            }
            else
            {
                AutoSetAnchorControlPoints(segmentIndex * 3 + 3);
            }
        }

        public void RemoveSegment(int anchorIndex)
        {
            if (SegmentCount > 2 || !_isClosed && SegmentCount > 1)
            {
                if (anchorIndex == 0)
                {
                    if (_isClosed)
                    {
                        _pointList[_pointList.Count - 1] = _pointList[2];
                    }
                    _pointList.RemoveRange(0, 3);
                }
                else if (anchorIndex == _pointList.Count - 1 && !_isClosed)
                {
                    _pointList.RemoveRange(anchorIndex - 2, 3);
                }
                else
                {
                    _pointList.RemoveRange(anchorIndex - 1, 3);
                }
            }
        }

        public Vector2[] GetPointsInSegment(int i)
        {
            return new Vector2[]
            {
                _pointList[i * 3],
                _pointList[i * 3 + 1],
                _pointList[i * 3 + 2],
                _pointList[LoopIndex(i * 3 + 3)]
            };
        }

        public void MovePoint(int i, Vector2 pos)
        {
            Vector2 deltaMove = pos - _pointList[i];

            if (i % 3 == 0 || !_autoSetControlPoints)
            {
                _pointList[i] = pos;

                if (_autoSetControlPoints)
                {
                    AutoSetAllAffectedControlPoints(i);
                }
                else
                {
                    if (i % 3 == 0)
                    {
                        if (i + 1 < _pointList.Count || _isClosed)
                        {
                            _pointList[LoopIndex(i + 1)] += deltaMove;
                        }
                        if (i - 1 >= 0 || _isClosed)
                        {
                            _pointList[LoopIndex(i - 1)] += deltaMove;
                        }
                    }
                    else
                    {
                        bool nextPointIsAnchor = (i + 1) % 3 == 0;
                        int correspondingControlIndex = nextPointIsAnchor ? i + 2 : i - 2;
                        int anchorIndex = nextPointIsAnchor ? i + 1 : i - 1;

                        if (correspondingControlIndex >= 0 && correspondingControlIndex < _pointList.Count || _isClosed)
                        {
                            float distance = (_pointList[LoopIndex(anchorIndex)] - _pointList[LoopIndex(correspondingControlIndex)]).magnitude;
                            Vector2 direction = (_pointList[LoopIndex(anchorIndex)] - pos).normalized;
                            _pointList[LoopIndex(correspondingControlIndex)] = _pointList[LoopIndex(anchorIndex)] + direction * distance;
                        }
                    }
                }
            }
        }

        public Vector2[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1.0f)
        {
            List<Vector2> evenlySpacePointList = new List<Vector2>();
            evenlySpacePointList.Add(_pointList[0]);
            Vector2 previousPoint = _pointList[0];
            float distanceSinceLastEvenPoint = 0.0f;

            for (int i = 0; i < SegmentCount; i++)
            {
                Vector2[] p = GetPointsInSegment(i);
                float controlNetLength = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
                float estimatedCurveLength = Vector2.Distance(p[0], p[3]) + controlNetLength * 0.5f;
                int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10.0f);
                float t = 0.0f;
                while (t <= 1.0f)
                {
                    t += 1.0f / divisions;
                    Vector2 pointOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                    distanceSinceLastEvenPoint += Vector2.Distance(previousPoint, pointOnCurve);

                    if (distanceSinceLastEvenPoint >= spacing)
                    {
                        float overshootDistance = distanceSinceLastEvenPoint - spacing;
                        Vector2 newEvenySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDistance;
                        evenlySpacePointList.Add(newEvenySpacedPoint);
                        distanceSinceLastEvenPoint = overshootDistance;
                        previousPoint = newEvenySpacedPoint;
                    }

                    previousPoint = pointOnCurve;
                }
            }

            if (!_isClosed)
            {
                evenlySpacePointList.Add(_pointList[_pointList.Count - 1]);
            }
            else
            {
                evenlySpacePointList.Add(_pointList[0]);
            }

            return evenlySpacePointList.ToArray();
        }

        private void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
        {
            for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i += 3)
            {
                if (i >= 0 && i < _pointList.Count || _isClosed)
                {
                    AutoSetAnchorControlPoints(LoopIndex(i));
                }
            }

            AutoSetStartAndEndControlPoints();
        }

        private void AutoSetAllControlPoints()
        {
            for (int i = 0; i < _pointList.Count; i += 3)
            {
                AutoSetAnchorControlPoints(i);
            }

            AutoSetStartAndEndControlPoints();
        }

        private void AutoSetAnchorControlPoints(int anchorIndex)
        {
            Vector2 anchorPos = _pointList[anchorIndex];
            Vector2 direction = Vector2.zero;
            float[] neighborDistanceArray = new float[2];

            if (anchorIndex - 3 >= 0 || _isClosed)
            {
                Vector2 offset = _pointList[LoopIndex(anchorIndex - 3)] - anchorPos;
                direction += offset.normalized;
                neighborDistanceArray[0] = offset.magnitude;
            }
            if (anchorIndex + 3 >= 0 || _isClosed)
            {
                Vector2 offset = _pointList[LoopIndex(anchorIndex + 3)] - anchorPos;
                direction -= offset.normalized;
                neighborDistanceArray[1] = -offset.magnitude;
            }

            direction.Normalize();

            for (int i = 0; i < 2; i++)
            {
                int controlIndex = anchorIndex + i * 2 - 1;
                if (controlIndex >= 0 && controlIndex < _pointList.Count || _isClosed)
                {
                    _pointList[LoopIndex(controlIndex)] = anchorPos + direction * neighborDistanceArray[i] * 0.5f;
                }
            }
        }

        private void AutoSetStartAndEndControlPoints()
        {
            if (!_isClosed)
            {
                _pointList[1] = (_pointList[0] + _pointList[2]) * 0.5f;
                _pointList[_pointList.Count - 2] = (_pointList[_pointList.Count - 1] + _pointList[_pointList.Count - 3]) * 0.5f;
            }
        }

        private int LoopIndex(int i)
        {
            return (i + _pointList.Count) % _pointList.Count;
        }
    }
}
