using System;
using Entities.Helpers.Classes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Entities.Players
{
    [System.Serializable]
    public class CharacterReference
    {
        public enum ETeam
        {
            TeamA,
            TeamB
        }
        
        [SerializeField] public GameObject prefab;
        [SerializeField] public Vector2 startingPosition;
        [SerializeField] public ETeam Team;
        [SerializeField] public Color debugDrawColor;
        [NonSerialized] public BaseCharacter character;

        public Tuple<GameObject, BaseCharacter> Setup(Transform parent)
        {
            if (!prefab.TryGetComponent<BaseCharacter>(out var c))
                throw new Exception("Character does not have any components inheriting from BaseCharacter!");
            var go = Object.Instantiate(prefab, parent);
            character = go.GetComponent<BaseCharacter>();
            character.startingPosition = startingPosition;
            character.reference = this;
            return new Tuple<GameObject, BaseCharacter>(go, character);
        }
    }
}
