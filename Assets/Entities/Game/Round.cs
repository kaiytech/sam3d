using System.Collections.Generic;
using Entities.Players;

namespace Entities.Game
{
    public class Round
    {
        public List<Turn> Turns = new List<Turn>();

        public Round(List<CharacterReference> characters)
        {
            foreach (var character in characters)
            {
                Turns.Add(new Turn(character));
            }
        }
    }
}