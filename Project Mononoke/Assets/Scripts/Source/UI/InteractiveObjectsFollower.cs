using System;
using Source.BuildingModule;
using Source.Character;
using UnityEngine;

namespace Source.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class InteractiveObjectsFollower : MonoBehaviour
    {
        [SerializeField] private CharacterLogicIsometric2DCollider _characterCollider = null;
        [SerializeField] private RectTransform _transform = null;

        private void OnValidate()
        {
            _transform ??= GetComponent<RectTransform>();
        }

        public void Initialize(CharacterLogicIsometric2DCollider characterCollider)
        {
            _characterCollider = characterCollider;
            _characterCollider.BuildingInCollider += OnBuildingInCollider;
        }

        private void OnBuildingInCollider(Building building)
        {
            var screenPos = Camera.main.WorldToScreenPoint(building.transform.position);
            var size = _transform.sizeDelta;
            var offset = new Vector3(0, size.y * 0.5f, 0);
            _transform.position = screenPos + offset;
            ToggleWith(true);
            
        }

        private void ToggleWith(bool signal)
        {
            gameObject.SetActive(signal);
        }
    }
}