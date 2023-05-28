using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<Tuple<GameObject, BaseCharacter>> Characters = new();

        [NonSerialized] public RoundScheduler RoundScheduler;
        void Start()
        {

        }

        public void Setup(int gridSizeX, int gridSizeY, GameObject squarePrefab, List<CharacterReference> characters)
        {
            foreach (var character in characters)
            {
                Characters.Add(character.Setup(transform));
            }

            //Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            Camera = Camera.main;
            transform.position -= new Vector3(gridSizeX-1, 0, gridSizeY-1);
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    var g =  Instantiate(squarePrefab, transform, true);
                    var square = g.GetComponent<Square>();
                    square.Setup(x, y);
                    square.Clicked += SquareOnClicked;
                    Squares.Add(new(g, square));
                }
            }

            CameraTarget = Squares.Last().Item1.transform.position;

            RoundScheduler = new RoundScheduler(this);
            RoundScheduler.Begin();
        }

        private void SquareOnClicked(object sender, Square.ClickedArgs e)
        {
            
            Debug.Log($"X: {e.Square.PosX}, Y: {e.Square.PosY}");
            CameraTarget = e.Square.transform.position;
            foreach (var square in Squares)
                square.Item2.State = Square.EState.None;
            e.Square.State = Square.EState.Selected;
            
            Characters[0].Item2.MoveTo(new Vector2(e.Square.PosX, e.Square.PosY), MoveType.Teleport);
            //Camera.transform.LookAt(e.Square.transform);
        }

        // Update is called once per frame
        void Update()
        {
            if (CameraTarget is not null)
            {
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
                    goto done;
                }

                float duration = 0.2f;
                if (posDif < 0.5f
                    && rotDif is { x: < 0.5f, y: < 0.5f, z: < 0.5f, w: < 0.5f })
                    duration = 0.05f;
                float t = Time.deltaTime / duration;
                var cachedPos = Camera.transform.position;
                var lerpPos = Vector3.Lerp(Camera.transform.position, targetPosition, t);
                Camera.transform.position = new Vector3(cachedPos.x, cachedPos.y, lerpPos.z);

                var cachedRot = Camera.transform.rotation; // for future use
                var lerpRot = Quaternion.Slerp(Camera.transform.rotation, targetRotation, t);
                Camera.transform.rotation = lerpRot.normalized;
            }
        
            done: ;
        }
    }
}
