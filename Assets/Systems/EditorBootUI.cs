#if UNITY_EDITOR
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems
{
    public class EditorBootUI : EditorWindow
    {
        [CanBeNull] public Button button = null;
        [CanBeNull] public string SavedScene;
    
        [MenuItem("Window/UI Toolkit/EditorBootUI")]
        public static void ShowExample()
        {
            EditorBootUI wnd = GetWindow<EditorBootUI>();
            wnd.titleContent = new GUIContent("Sam3D Boot UI");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
        
            VisualElement label = new Label("Sam3D");
            label.style.alignSelf = new StyleEnum<Align>(Align.Center);
            button = new Button();
            button.style.color = new StyleColor(Color.black);
            button.style.backgroundColor = new StyleColor(new Color(0.4f, 0.8f, 0.5f));
            button.style.height = 40;
            button.clicked += Toggle;
            root.Add(label);
            root.Add(button);
        }

        public void Toggle()
        {
            if (button != null)
            {
                switch (button.text)
                {
                    case "Stop!":
                        EditorApplication.isPlaying = false;
                        EditorApplication.update += LoadSavedScene;
                        break;
                    case "Launch!":
                        SavedScene = EditorSceneManager.GetActiveScene().path;
                        EditorSceneManager.OpenScene("Assets/Scenes/Boot.unity");
                        EditorApplication.isPlaying = true;
                        break;
                }
            }
        }

        private void LoadSavedScene()
        {
            if (EditorApplication.isPlaying) return;
            EditorSceneManager.OpenScene(SavedScene);
            EditorApplication.update -= LoadSavedScene;
        }

        public void Update()
        {
            if (button != null)
            {
                var playing = EditorApplication.isPlaying;
                button.text = playing ? "Stop!" : "Launch!";
                button.style.backgroundColor = playing ? new StyleColor(new Color(0.9f, 0.3f, 0.3f)) : new StyleColor(new Color(0.4f, 0.8f, 0.5f));
            }
        }
    }
}
#endif