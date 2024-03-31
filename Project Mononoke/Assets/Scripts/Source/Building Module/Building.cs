using System;
using UnityEngine;

namespace Source.BuildingModule
{
    public abstract class Building : MonoBehaviour
    {
        public event Action CharacterComesUp;
    }
}