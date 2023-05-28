using System;
using System.Linq;
using Entities.Game;
using Entities.Helpers.Classes;
using Entities.Helpers.Enums;
using UnityEngine;
using UnityEngine.Animations;
using Object = UnityEngine.Object;

namespace Entities.Players
{
    public class BaseCharacter : MonoBehaviour
    {
        [SerializeField] public string displayName;
        [SerializeField] public int healthMax;
        public Vector2 startingPosition;
        public CharacterReference reference;



        //private Vector2 _position;
        [NonSerialized] protected float x;
        [NonSerialized] protected float y;

        public static Vector3 CalculateWorldPosition(Vector2 v2)
        {
            var sq = Globals.Arena.Squares.FirstOrDefault(s => s.Item2.PosX == v2.x && s.Item2.PosY == v2.y);
            if (sq != null)
                return sq.Item1.transform.position;
          //      foreach (var square in Globals.Arena.Squares.Where(square => Math.Abs(square.Item2.PosX - v2.x) < .1f && Math.Abs(square.Item2.PosY - v2.y) < .1f))
          //      return square.Item2.transform.localPosition;
            throw new Exception("what");
        }
        
        public virtual void Start()
        {
            //MoveTo(startingPosition, MoveType.Teleport);
            //gameObject.transform.position = CalculateWorldPosition(new Vector2(x, y));
            healthMax = 60;
            //MoveTo(startingPosition, MoveType.Teleport);
            ;
        }
        
        public virtual void Update()
        {
            gameObject.transform.position = CalculateWorldPosition(new Vector2(x, y));
        }

        public void MoveTo(Vector2 position, MoveType moveType)
        {
            if (moveType is MoveType.Teleport)
            {
                healthMax = 30;
                x = position.x;
                y = position.y;
            }
        }
    }
}