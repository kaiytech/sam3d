using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems
{
    public class CBootLoader : MonoBehaviour
    {
        public Scene? CurrentLevel = null;
    
        // Start is called before the first frame update
        void Start()
        {
            LoadLevel("Level1");
        }

        public delegate void LevelLoadedHandler();

        public event LevelLoadedHandler LevelLoaded;

        void LoadLevel(string levelName)
        {
            SceneManager.LoadScene("Scenes/" + levelName, LoadSceneMode.Single);
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                CurrentLevel = scene;
                LevelLoaded?.Invoke();
            };
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
