using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Game.UI;
using Entities.Helpers.Enums;
using Entities.Players;
using UnityEngine;

namespace Entities.Game
{
    public class Arena : MonoBehaviour
    {
        // Start is called before the first frame update

        [NonSerialized] public int GridSizeX = 0;
        [NonSerialized] public int GridSizeY = 0;

        [NonSerialized] public Camera Camera;
        [NonSerialized] public Vector3? CameraTarget = null;

        [NonSerialized] public List<Tuple<GameObject, Square>> Squares = new();
        [NonSerialized] public List<Tuple<GameObject, BaseCharacter>> Characters = new();

        [NonSerialized] public RoundScheduler RoundScheduler;

        private bool _gameRunning = false;

        public void StartGame()
        {
            _gameRunning = true;
        }

        public void Setup(int gridSizeX, int gridSizeY, GameObject squarePrefab, List<CharacterReference> characters)
        {
            Squares.Clear();
            RoundScheduler = null;
            Globals.UI.NumOfBombs = 0;
            Globals.UI.NumOfFlags = 0;
            
            foreach (var character in characters)
            {
                var _char = character.Setup(transform);
                Characters.Add(_char);
                _char.Item2.MoveTo(_char.Item2.startingPosition, MoveType.Teleport);
            }

            //Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            Camera = Globals.Camera;
            transform.position -= new Vector3(gridSizeX-1, 0, gridSizeY-1);
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    var g =  Instantiate(squarePrefab, transform, true);
                    var square = g.GetComponent<Square>();
                    square.Setup(x, y);
                    square.Clicked += SquareOnClicked;
                    square.ClickedRight += SquareOnClickedRight;
                    Squares.Add(new(g, square));
                }
            }
        }

        private void SquareOnClicked(object sender, Square.ClickedArgs e)
        {
            if (!_gameRunning)
                return;
            if (RoundScheduler == null)
            {
                RoundScheduler = new RoundScheduler(this);
                RoundScheduler.SetIgnore(e.Square);
                RoundScheduler.Begin();
                Globals.UI.NumOfBombs = Squares.Count(s => s.Item2.Underground == Square.EUndergroundType.Mined);
                Globals.UI.NumOfFlags = 0;
            }

            if (RoundScheduler.GameLost)
                return;

            if (e.Square.Dig().HitMine)
            {
                RoundScheduler.LoseGame();
                Globals.UI.DisplayMode = UIController.EDisplayMode.Leaderboard;
            }
        }

        private void SquareOnClickedRight(object sender, Square.ClickedArgs e)
        {
            if (!_gameRunning)
                return;
            
            var markAction = e.Square.Mark();
            if (markAction.Marked)
                Globals.UI.NumOfFlags++;
            else
                Globals.UI.NumOfFlags--;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_gameRunning)
                return;
            if (CameraTarget is null)
                CameraTarget = Vector3.Lerp(Squares.First().Item1.transform.position, Squares.Last().Item1.transform.position, 0.5f);
            Vector3 targetPosition = (Vector3) CameraTarget + new Vector3(2, 0, 0);
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - Camera.transform.position, Vector3.up);
            var posDif = Vector3.Distance(targetPosition, Camera.transform.position);
            var rotDif = Camera.transform.rotation * Quaternion.Inverse(targetRotation);
        
            // is it done?
            if (posDif < 0.01f && rotDif is { x: < 0.01f, y: < 0.01f, z: < 0.01f, w: < 0.01f })
            {
                Camera.transform.position = targetPosition;
                Camera.transform.rotation = targetRotation;
                CameraTarget = null;
            }

            float duration = 0.2f;
            if (posDif < 0.5f
                && rotDif is { x: < 0.5f, y: < 0.5f, z: < 0.5f, w: < 0.5f })
                duration = 0.05f;
            float t = Time.deltaTime / duration;
            var cachedPos = Camera.transform.position;
            var lerpPos = Vector3.Lerp(Camera.transform.position, targetPosition, t);
            Camera.transform.position = new Vector3(cachedPos.x, cachedPos.y, lerpPos.z);

            //var cachedRot = Camera.transform.rotation; // for future use
            var lerpRot = Quaternion.Slerp(Camera.transform.rotation, targetRotation, t);
            Camera.transform.rotation = lerpRot.normalized;
            
            
            

            //foreach (var s in Squares.Select(square => square.Item2))
            //{
            //    s.CanMoveTo = Characters[0].Item2.CanMoveTo(new Vector2(s.PosX, s.PosY));
            //}
                
                
                
            
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            float newFOV = Camera.main.fieldOfView - (scrollInput * 20);
            newFOV = Mathf.Clamp(newFOV, 50, 130);
            Camera.main.fieldOfView = newFOV;
            
            var mouse = Input.mousePosition;
            if (mouse.x > 0 && mouse.x < Screen.width && mouse.y > 0 && mouse.y < Screen.height)
            {

                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || mouse.y < 10)
                {
                    CameraTarget -= new Vector3(0.1f, 0, 0);
                    Camera.main.transform.position -= new Vector3(0.1f, 0, 0);
                }

                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Screen.height - mouse.y < 10)
                {
                    CameraTarget += new Vector3(0.1f, 0, 0);
                    Camera.main.transform.position += new Vector3(0.1f, 0, 0);
                }

                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Screen.width - mouse.x < 10)
                {
                    CameraTarget -= new Vector3(0, 0, 0.1f);
                    Camera.main.transform.position -= new Vector3(0, 0, 0.1f);
                }

                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || mouse.x < 10)
                {
                    CameraTarget += new Vector3(0, 0, 0.1f);
                    Camera.main.transform.position += new Vector3(0, 0, 0.1f);
                }
            }

        }
    }
}
