using System.Collections.Generic;
using UnityEngine;

namespace Source.Formations
{
    public abstract class Formation
    {
        [SerializeField] [Range(0, 1)] protected float _formationDisorderPercentage = 0f;
        [SerializeField] protected float _positionsDistance = 1f;
        [SerializeField] protected bool _isHollow = false;

        public abstract IEnumerable<Vector3> GetFormationPositions();

        public Vector3 GetDisorderedOffsetFor(Vector3 orderedPosition)
        {
            var noise = Mathf.PerlinNoise(orderedPosition.x * _formationDisorderPercentage, orderedPosition.y * _formationDisorderPercentage);
            return new Vector3(noise, noise, 0);
        }
    }
}
