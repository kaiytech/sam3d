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
        [SerializeField] public int reach;
        public Vector2 startingPosition;
        public CharacterReference reference;
        
        //private Vector2 _position;
        [NonSerialized] protected int x;
        [NonSerialized] protected int y;

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
                x = (int)position.x;
                y = (int)position.y;
            }
        }

        public bool CanMoveTo(Vector2 position)
        {
            var current = Globals.Arena.Squares.FirstOrDefault(
                s => Math.Abs(s.Item2.PosX - x) < 0.1f &&
                     Math.Abs(s.Item2.PosY - y) < 0.1f)
                ?.Item2;

            var target = Globals.Arena.Squares.FirstOrDefault(
                s => Math.Abs(s.Item2.PosX - position.x) < 0.1f &&
                     Math.Abs(s.Item2.PosY - position.y) < 0.1f)
                ?.Item2;

            if (current == null || target == null)
                return false;

            // can't move to the same place where we stand
            if (Equals(current, target))
                return false;
            
            if (Globals.Arena.Characters
                .Select(arenaCharacter => arenaCharacter.Item2).Any(c => c.x == target.PosX && c.y == target.PosY))
                return false;

            // can't move anywhere if grounded (reach == 0)
            if (reach == 0)
                return false;

            var xDiff = Math.Abs(current.PosX - target.PosX);
            var yDiff = Math.Abs(current.PosY - target.PosY);
            
            if (reach % 2 == 0)
            {
                if (xDiff <= reach - 1 && yDiff <= reach - 1)
                    return true;
            }
            else
            {
                if (reach >= 3)
                    if (xDiff <= reach - 2 && yDiff <= reach - 2)
                        return true;

                if (xDiff <= reach - 1 && yDiff <= Math.Abs(reach - 2))
                    return true;

                if (yDiff <= reach - 1 && xDiff <= Math.Abs(reach - 2))
                    return true;
            }

            return false;
        }
    }
}