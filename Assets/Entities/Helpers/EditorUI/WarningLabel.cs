#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Entities.Helpers.EditorUI
{
    [CustomEditor(typeof(GameManager))]
    public class WarningLabel : Editor
    {
        public string fine = "No problems found";
        public List<string> problems = new();
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GameManager gameManager = (GameManager)target;
            problems = gameManager.GetProblems();

            var cached = GUI.color;
            if (problems!.Count == 0)
            {
                GUI.color = Color.green;
                GUILayout.Label($"✔ {fine}");
            }
            else
            { 
                GUI.color = Color.red;
                foreach (var problem in problems)
                    GUILayout.Label($"✘ {problem}");
            }

            GUI.color = cached;
        }
    }
}
#endif