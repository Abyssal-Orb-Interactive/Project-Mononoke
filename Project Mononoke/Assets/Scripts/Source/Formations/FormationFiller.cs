using System.Collections.Generic;
using Source.Character.Movement;
using UnityEngine;

namespace Source.Formations
{
    public class FormationFiller : MonoBehaviour
    {
        [SerializeField] private FormationPositionsHolder _formationPositions = null;
        private List<GameObject> _spawnedMinions = new();


    }
}