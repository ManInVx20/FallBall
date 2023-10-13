using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace VinhLB
{
    public class SoftBody2D : MonoBehaviour
    {
        private const float SPRITE_OFFSET = 0.5f;

        [SerializeField]
        private SpriteShapeController _shapeController;
        [SerializeField]
        private Transform[] _pointArray;

        private void Start()
        {
            UpdateVertices();
        }

        private void Update()
        {
            UpdateVertices();
        }

        private void UpdateVertices()
        {
            for (int i = 0; i < _pointArray.Length; i++)
            {
                Vector2 vertex = _pointArray[i].localPosition;
                Vector2 towardsCenter = (-vertex).normalized;
                float colliderRadius = _pointArray[i].GetComponent<CircleCollider2D>().radius;

                try
                {
                    Vector2 position = vertex - towardsCenter * colliderRadius;
                    _shapeController.spline.SetPosition(i, position);
                }
                catch
                {
                    Debug.LogWarning("Spline points are too close to each other.");
                    Vector2 position = vertex - towardsCenter * (colliderRadius + SPRITE_OFFSET);
                    _shapeController.spline.SetPosition(i, position);
                }

                Vector2 leftTangent = _shapeController.spline.GetLeftTangent(i);
                Vector2 newRightTangent = Vector2.Perpendicular(towardsCenter) * leftTangent.magnitude;
                Vector2 newLeftTangent = Vector2.zero - newRightTangent;

                _shapeController.spline.SetLeftTangent(i, newLeftTangent);
                _shapeController.spline.SetRightTangent(i, newRightTangent);
            }
        }
    }
}
