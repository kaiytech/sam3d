using System;
using System.Collections.Generic;
using Entities.Game;
using Entities.Game.UI;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Entities
{
    public class GameManager : MonoBehaviour
    {
        [Serializable]
        public class ModeInfo
        {
            public string Id;
            public GameObject Value;
        }

        [SerializeField] public GameObject SquarePrefab;
        
        // Start is called before the first frame update
        void Start()
        {
            Globals.GameManager = this;

            transform.position = new Vector3(0, 0, 0);

            Globals.UI = gameObject.AddComponent<UIController>();
            Globals.UI.DisplayMode = UIController.EDisplayMode.Menu;
        }

        public void StartGame(int x, int y, int bombs)
        {
            if (Globals.Camera is not null)
                Destroy(Globals.Camera);
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            Globals.Camera = cameraObject.AddComponent<Camera>();
            Globals.Camera.transform.position = new Vector3(0, 10, 0);
            Globals.Camera.fieldOfView = 80.0f;
            Globals.Camera.clearFlags = CameraClearFlags.SolidColor;
            Globals.Camera.backgroundColor = Color.black;
            if (Globals.Arena is not null)
                Destroy(Globals.Arena);
            Globals.Arena = gameObject.AddComponent<Arena>();
            Globals.Arena!.name = "Arena";
            Globals.Arena!.Setup(x, y, bombs, SquarePrefab);
            Globals.Arena!.StartGame();
        }
        
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
