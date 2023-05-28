using Entities.Players;
using UnityEngine.TextCore.Text;

namespace Entities.Game
{
    public class Turn
    {
        public CharacterReference Character;

        public Turn(CharacterReference character)
        {
            Character = character;
        }
    }
}