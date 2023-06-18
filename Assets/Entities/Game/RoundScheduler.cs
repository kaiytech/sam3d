using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Entities.Game
{
    public class RoundScheduler
    {
        private Arena _arena;

        private int _bombs = 10;

        private Random _random = new Random();

        private Square _ignoredSquare;

        public bool GameLost = false;
        
        public RoundScheduler(Arena arena)
        {
            _arena = arena;
        }

        public void SetIgnore(Square square)
        {
            _ignoredSquare = square;
        }

        public void SetBombs(int bombs)
        {
            _bombs = bombs;
        }

        public void Begin()
        {
            //Clear();
            SetupBombs();
        }

        private void Clear()
        {
            foreach (var square in _arena.Squares)
                square.Item2.Reset();
        }

        private void SetupBombs()
        {
            for (int i = 0; i < _bombs;)
            {
                var r = _arena.Squares[_random.Next(0, _arena.Squares.Count)].Item2;
                if (r.Underground == Square.EUndergroundType.Mined || r == _ignoredSquare)
                    continue;
                r.Underground = Square.EUndergroundType.Mined;
                i++;
            }
        }

        public void LoseGame()
        {
            foreach (var (_, item2) in _arena.Squares.Where(s => s.Item2.Underground == Square.EUndergroundType.Mined))
            {
                item2.Field = Square.EFieldType.DugUp;
            }

            GameLost = true;
        }
        
        public void WinGame()
        {
            foreach (var (_, item2) in _arena.Squares.Where(s => s.Item2.Underground == Square.EUndergroundType.Mined))
            {
                item2.Field = Square.EFieldType.DugUp;
            }

            GameLost = true;
            Globals.UI.WonGame = true;
        }
    }
}