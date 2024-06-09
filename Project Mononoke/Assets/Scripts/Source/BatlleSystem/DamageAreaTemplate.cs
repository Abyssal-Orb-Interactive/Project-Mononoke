using System.Collections.Generic;
using Base.Input;
using UnityEngine;

namespace Source.BattleSystem
{
    public abstract class DamageAreaTemplate : ScriptableObject
    {
        public abstract IEnumerable<Vector2> GetVertices(MovementDirection facing);
    }
}