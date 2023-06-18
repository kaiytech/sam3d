using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Entities.Game.UI
{
    public class UIController : MonoBehaviour
    {
        private GUIStyle _pointsGuiStyle;
        private GUIStyle _regularTextGuiStyle;
        private GUIStyle _logoGuiStyle;
        private GUIStyle _scrollBarGuiStyle;
        private Texture2D _texture;

        private string X = "10";
        private string Y = "10";
        private string Bombs = "20";

        public int NumOfBombs = 0;
        public int NumOfFlags = 0;

        public bool WonGame = false;

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
            
            _regularTextGuiStyle = new GUIStyle();
            _regularTextGuiStyle.normal.textColor = Color.white;
            _regularTextGuiStyle.fontSize = 18;
            _regularTextGuiStyle.alignment = TextAnchor.MiddleCenter;

            _logoGuiStyle = new GUIStyle();
            _logoGuiStyle.normal.textColor = Color.magenta;
            _logoGuiStyle.fontSize = 42;
            _logoGuiStyle.alignment = TextAnchor.MiddleCenter;
            _logoGuiStyle.normal.background = _texture;

            _scrollBarGuiStyle = new GUIStyle();
            _scrollBarGuiStyle.normal.background = Texture2D.whiteTexture;
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
                GUI.backgroundColor = Color.black;
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none);
                GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
                GUILayout.Label("FairSweeper", _logoGuiStyle);
                GUILayout.EndArea();
                GUILayout.BeginArea(new Rect(0, 100, Screen.width, 200));
                GUILayout.Label("X:", _regularTextGuiStyle);
                X = GUILayout.TextField(X, _regularTextGuiStyle);
                GUILayout.Label("Y:", _regularTextGuiStyle);
                Y = GUILayout.TextField(Y, _regularTextGuiStyle);
                GUILayout.Label("Bombs:", _regularTextGuiStyle);
                Bombs = GUILayout.TextField(Bombs, _regularTextGuiStyle);
                GUILayout.Label("", _regularTextGuiStyle);

                if (IsOk())
                {
                    if (GUILayout.Button("Start", _logoGuiStyle))
                    {
                        Globals.GameManager.StartGame(int.Parse(X), int.Parse(Y), int.Parse(Bombs));
                        DisplayMode = EDisplayMode.Game;
                    }
                }

                GUILayout.EndArea();
            }
            else
            {
                GUILayout.BeginArea(new Rect(0, 0, Screen.width, 300));
                var txt = WonGame ? "You win! Play again." : "You lose! Restart.";
                if (GUILayout.Button(txt, _logoGuiStyle))
                {
                    SceneManager.LoadScene("Scenes/Boot", LoadSceneMode.Single);
                    DisplayMode = EDisplayMode.Menu;
                    WonGame = false;
                }

                GUILayout.EndArea();
            }
        }

        private bool IsOk() =>
            int.TryParse(X, out var intx) &&
            int.TryParse(Y, out var inty) &&
            int.TryParse(Bombs, out var intbombs) &&
            intx >= 10 &&
            inty >= 10 &&
            intbombs < (intx * inty)/2;

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