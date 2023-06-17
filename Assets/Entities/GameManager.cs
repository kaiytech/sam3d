using System;
using System.Collections.Generic;
using Entities.Game;
using Entities.Game.UI;
using Entities.Helpers.EditorUI;
using Entities.Players;
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
    
        [SerializeField] public int GridSizeX = 0;

        [SerializeField] public int GridSizeY = 0;

        [SerializeField] public GameObject SquarePrefab;

        [SerializeField] public List<CharacterReference> Characters;

        [NonSerialized] public WarningLabel WarningLabel;

        // Start is called before the first frame update
        void Start()
        {
            Globals.GameManager = this;

            transform.position = new Vector3(0, 0, 0);

            Globals.UI = gameObject.AddComponent<UIController>();
            Globals.UI.DisplayMode = UIController.EDisplayMode.Menu;
        }

        public void StartGame()
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
            Globals.Arena!.Setup(GridSizeX, GridSizeY, SquarePrefab, Characters);
            Globals.Arena!.StartGame();
        }
    
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            var tileColor = new Color(255, 255, 0, 100);
            for (var x = 0; x < GridSizeX; x++)
            {
                for (var y = 0; y < GridSizeY; y++)
                {
                    var center = new Vector3(x * 2 - GridSizeX + 1, 0, y * 2 - GridSizeY + 1);
                    Gizmos.color = tileColor;
                    Gizmos.DrawCube(center, new Vector3(1.8f, .1f, 1.8f));

                    foreach (var character in Characters)
                    {
                        character.debugDrawColor.a = 1;
                        Gizmos.color = character.debugDrawColor;
                        // ReSharper disable twice CompareOfFloatsByEqualityOperator
                        if (character.startingPosition.x == x && character.startingPosition.y == y)
                            Gizmos.DrawSphere(center, .5f);
                    }
                }
            }
        }
#endif

        public List<string> GetProblems()
        {
            List<string> problems = new();
        
            // 1: starting position conflicts: the same
            foreach (var character in Characters)
            {
                foreach (var character2Iteration in Characters)
                {
                    if (character.startingPosition.x == character2Iteration.startingPosition.x &&
                        character.startingPosition.y == character2Iteration.startingPosition.y &&
                        character != character2Iteration)
                    {
                        var message = $"Position of character {character.prefab.name} conflicts with character {character2Iteration.prefab.name}";
                        if (!problems!.Contains(message))
                            problems.Add(message);
                    }
                }
            }
        
            // 2: starting position conflicts: out of bounds
            foreach (var character in Characters)
                if (character.startingPosition.x >= GridSizeX || character.startingPosition.y >= GridSizeY || character.startingPosition.x < 0 || character.startingPosition.y < 0)
                    problems.Add($"Position of character {character.prefab.name} is out of arena range.");

        
            // 3: count team members
            var teamA = 0;
            var teamB = 0;
            foreach (var character in Characters)
                switch (character.Team)
                {
                    case CharacterReference.ETeam.TeamA:
                        teamA += 1;
                        break;
                    case CharacterReference.ETeam.TeamB:
                        teamB += 1;
                        break;
                }
            if (teamA == 0)
                problems.Add("Team A has no characters.");
            if (teamB == 0)
                problems.Add("Team B has no characters.");
            return problems;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
