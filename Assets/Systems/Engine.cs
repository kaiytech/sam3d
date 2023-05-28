using JetBrains.Annotations;
using UnityEngine;

namespace Systems
{
    public class Engine : MonoBehaviour
    {
        // Start is called before the first frame update
        [CanBeNull] public CBootLoader bootLoader;
        [CanBeNull] public static Camera camera;
    
        void Start()
        {
            bootLoader = gameObject.AddComponent<CBootLoader>();
            bootLoader.LevelLoaded += PrepareLevel;
        }

        void PrepareLevel()
        {
            /**/
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
