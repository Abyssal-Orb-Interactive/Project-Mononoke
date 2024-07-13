using Base.Databases;
using UnityEngine;

namespace Source.Character.Database
{
    [CreateAssetMenu(fileName = "CharactersDatabase", menuName = "Databases/Create characters database")]
    public class CharactersDatabase : DatabaseSO<CharacterData>
    {
    }
}