using System;
using System.Collections;
using UnityEngine;
using static Source.InventoryModule.Inventory;

namespace Source.PickUpModule
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Collider2D))]
    [Serializable]
    public class DroppedItem : MonoBehaviour
    {
        [field: SerializeField] public InventoryItem ItemData {get; private set;}
        [SerializeField] private float _pickUpAnimationDuration = 0f;

        private Sprite _sprite = null;
        private AudioSource _audioPlayer = null;

        private void OnValidate() 
        {
            _sprite ??= GetComponent<SpriteRenderer>().sprite;
            _audioPlayer ??= GetComponent<AudioSource>(); 
        }

        public void BeginPickUp()
        {
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(PickUpAnimation());
        }

        private IEnumerator PickUpAnimation()
        {
            _audioPlayer.Play();
            var startScale = transform.localScale;
            var endScale = Vector3.zero;
            var elapsedTime = 0f;
            while(elapsedTime < _pickUpAnimationDuration)
            {
                elapsedTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / _pickUpAnimationDuration);
                yield return null;
            }
            transform.localScale = endScale;
            Destroy(gameObject);
        }

    }
}
