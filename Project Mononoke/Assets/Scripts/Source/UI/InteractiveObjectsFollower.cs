using Source.BuildingModule;
using Source.Character;
using UnityEngine;

namespace Source.UI
{
    public class InteractiveObjectsFollower : MonoBehaviour
    {
        [SerializeField] private CharacterLogicIsometric2DCollider _characterCollider = null;

        public void Initialize(CharacterLogicIsometric2DCollider characterCollider)
        {
            _characterCollider = characterCollider;
            _characterCollider.BuildingInCollider += OnBuildingInCollider;
        }

        private void OnBuildingInCollider(Building building)
        {
            transform.position = building.transform.position;
            ToggleWith(true);
            
        }

        private void ToggleWith(bool signal)
        {
            gameObject.SetActive(signal);
        }
    }
}