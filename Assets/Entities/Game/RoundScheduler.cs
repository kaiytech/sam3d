using System.Collections.Generic;
using UnityEngine;

namespace Entities.Game
{
    public class RoundScheduler
    {
        private Arena _arena;

        private List<Round> _rounds;
        
        public RoundScheduler(Arena arena)
        {
            _arena = arena;
        }

        public void Begin()
        {
            
        }

        public void NewRound()
        {
            //_rounds.Add(new Round(_arena.Characters));
        }
    }
}