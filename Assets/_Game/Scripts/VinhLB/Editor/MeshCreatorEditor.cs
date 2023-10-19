using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VinhLB
{
    [CustomEditor(typeof(MeshCreator))]
    public class MeshCreatorEditor : Editor
    {
        private MeshCreator _creator;

        private void OnEnable()
        {
            _creator = (MeshCreator)target;
        }

        private void OnSceneGUI()
        {
            if (_creator.AutoUpdate && Event.current.type == EventType.Repaint)
            {
                _creator.UpdateMesh();
            }
        }
    }
}
