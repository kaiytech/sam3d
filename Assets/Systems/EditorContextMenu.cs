#if UNITY_EDITOR
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Systems
{
    public class EditorContextMenu : MonoBehaviour
    {
        [CanBeNull] public static EditorBootUI EditorUI = null;
        
        [MenuItem("Sam3D/Open Boot UI")]
        static void DoSomething()
        {
            if (EditorUI == null)
                EditorUI = ScriptableObject.CreateInstance<EditorBootUI>();
            EditorUI.Show();
        }
    }
}
#endif