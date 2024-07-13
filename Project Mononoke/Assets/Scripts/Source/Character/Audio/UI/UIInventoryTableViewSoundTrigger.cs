using Source.InventoryModule;
using Source.InventoryModule.UI;
using UnityEngine;

namespace Source.Character.Audio
{
    [RequireComponent(typeof(AudioPlayer))]
    [RequireComponent(typeof(InventoryTableView))]
    public class UIInventoryTableViewSoundTrigger : MonoBehaviour
    {
        private AudioPlayer _player = null;
        private InventoryTableView _view = null;
        
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _player ??= GetComponent<AudioPlayer>();
            _view ??= GetComponent<InventoryTableView>();
            _view.ItemsSwapped += OnSwap;
            _view.ItemDropped += OnDrop;
        }

        private void OnDrop(InventoryPresenter.StackDataForUI obj)
        {
            _player.PlaySound(AudioTypes.UIItemThrow);
        }

        private void OnSwap()
        {
            _player.PlaySound(AudioTypes.UIItemsSwap);
        }
    }
}