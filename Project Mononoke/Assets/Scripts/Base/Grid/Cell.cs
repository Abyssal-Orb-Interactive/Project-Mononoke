using System;
using Source.BattleSystem;
using Source.BuildingModule;
using UnityEngine;

namespace Base.Grid
{
    public class Cell : IReadonlyCell
    {
        public Building Building { get; private set; }
        public bool HasBuilding => Building != null;
        public CellType Type { get; private set; } = CellType.Air;

        public event Action<Cell> CellConfigurationChanged = null;

        public Cell(CellType type)
        {
            Type = type;
        }

        private void OnBuildingDestruction(IDamageable obj)
        {
            Building.GetComponent<Damageable>().Death -= OnBuildingDestruction;
            Building = null;
            CellConfigurationChanged?.Invoke(this);
        }

        public void AddBuilding(Building building)
        {
            Building = building;
            if (Building.TryGetComponent<Damageable>(out var damageable))
            {
                damageable.Death += OnBuildingDestruction;
            }
            CellConfigurationChanged?.Invoke(this);
        }
    }
}
