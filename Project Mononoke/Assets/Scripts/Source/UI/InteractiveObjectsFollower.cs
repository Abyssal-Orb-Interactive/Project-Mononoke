using System;
using Source.BuildingModule;
using Source.BuildingModule.Buildings;
using Source.Character;
using Source.PickUpModule;
using UnityEngine;

namespace Source.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class InteractiveObjectsFollower : MonoBehaviour, BuildingInteractionActionRequester
    {
        [SerializeField] private CharacterLogicIsometric2DCollider _characterCollider = null;
        [SerializeField] private RectTransform _transform = null;
        [SerializeField] private PickUpper _pickUpper = null;

        private Building _currentBuilding;

        public event Action<Building, PickUpper> InteractionActionRequest;

        private void OnValidate()
        {
            _transform ??= GetComponent<RectTransform>();
        }

        public void Initialize(CharacterLogicIsometric2DCollider characterCollider)
        {
            _characterCollider = characterCollider;
            _characterCollider.BuildingInCollider += OnBuildingInCollider;
            _characterCollider.BuildingOutCollider += OnBuildingOutCollider;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                InteractionActionRequest?.Invoke(_currentBuilding, _pickUpper);
                ToggleWith(false);
            }
        }
        
        private void OnBuildingOutCollider(Building building)
        {
            if (_currentBuilding == building)
            {
                ToggleWith(false);
                _currentBuilding = null;
            }
        }

        private void OnBuildingInCollider(Building building)
        {
            _currentBuilding = building;
            if(!_currentBuilding.ReadyToInteract) return;
            var worldPosition = building.transform.position;
            var screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponentInParent<Transform>() as RectTransform, screenPosition,
                Camera.main, out var localPoint);
            var size = _transform.sizeDelta;
            var offset = new Vector3(0, size.y * 0.5f, 0);
            _transform.localPosition = localPoint;
            ToggleWith(true);
        }

        private void ToggleWith(bool signal)
        {
            gameObject.SetActive(signal);
        }
    }
}