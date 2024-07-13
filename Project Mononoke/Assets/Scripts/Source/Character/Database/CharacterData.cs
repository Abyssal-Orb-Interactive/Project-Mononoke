using System;
using Base.Databases;
using Source.BattleSystem;
using Source.Character.AI.BattleAI.Behaviours.EnemyInDamageAreBehaviours;
using Source.Character.AI.BattleAI.Behaviours.EnemyInSearchAreaBehaviours;
using UnityEngine;

namespace Source.Character.Database
{
    [Serializable]
    public class CharacterData : IDatabaseItem
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public float HP { get; private set; }
        [field: SerializeField] public float UnarmedDamage { get; private set; }
        [field: SerializeField] public float ManipulatorStrength { get; private set; }
        [field: SerializeField] public float ManipulatorCapacity { get; private set; }
        [field: SerializeField] public float InventoryWeightCapacity { get; private set; }
        [field: SerializeField] public float InventoryVolumeCapacity { get; private set; }
        [field: SerializeField] public Fractions Fraction { get; private set; }
        [field: SerializeField] public EnemyInDamageAreaBehaviours EnemyInDamageAreaBehaviour { get; private set; }
        [field: SerializeField] public EnemyInSearchAreaBehaviours EnemyInSearchAreaBehaviour { get; private set; }
    }
}