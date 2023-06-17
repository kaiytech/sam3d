using System;
using UnityEngine;

namespace Entities.Game.UI
{
    public class UIController : MonoBehaviour
    {
        private GUIStyle _pointsGuiStyle;
        private GUIStyle _logoGuiStyle;
        private Texture2D _texture;

        public int NumOfBombs = 0;
        public int NumOfFlags = 0;

        public enum EDisplayMode
        {
            Game,
            Menu,
            Leaderboard
        }

        public EDisplayMode DisplayMode = EDisplayMode.Game;

        private void Start()
        {
            _pointsGuiStyle = new GUIStyle();
            _pointsGuiStyle.normal.textColor = Color.black;
            _pointsGuiStyle.fontSize = 26;
            _texture = MakeTexture(2, 2, Color.white);
            _pointsGuiStyle.normal.background = _texture;

            _logoGuiStyle = new GUIStyle();
            _logoGuiStyle.normal.textColor = Color.magenta;
            _logoGuiStyle.fontSize = 42;
            _logoGuiStyle.alignment = TextAnchor.MiddleCenter;
            _logoGuiStyle.normal.background = _texture;
        }

        private void OnGUI()
        {
            if (DisplayMode == EDisplayMode.Game)
            {
                GUILayout.BeginArea(new Rect(30, 30, 300, 100));
                //GUILayout.Box(_texture);
                GUILayout.Label("Bombs: " + NumOfBombs, _pointsGuiStyle);
                GUILayout.EndArea();

                GUILayout.BeginArea(new Rect(30, 60, 300, 100));
                //GUILayout.Box(_texture);
                GUILayout.Label("Flags: " + NumOfFlags, _pointsGuiStyle);
                GUILayout.EndArea();
            }
            else if (DisplayMode == EDisplayMode.Menu)
            {
                GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
                GUILayout.Label("FairSweeper", _logoGuiStyle);
                if (GUILayout.Button("Start"))
                {
                    Globals.GameManager.StartGame();
                    DisplayMode = EDisplayMode.Game;
                }
                GUILayout.EndArea();
            }
            else
            {
                GUILayout.BeginArea(new Rect(0, 0, 300, 300));
                if (GUILayout.Button("Restart"))
                {
                    Globals.GameManager.StartGame();
                    DisplayMode = EDisplayMode.Game;
                }

                GUILayout.EndArea();
            }
        }
        
        private Texture2D MakeTexture(int width, int height, Color color)
        {
            var texture = new Texture2D(width, height);
            var fillColorArray = texture.GetPixels();

            for (var i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = color;
            }

            texture.SetPixels(fillColorArray);
            texture.Apply();

            return texture;
        }
    }
}