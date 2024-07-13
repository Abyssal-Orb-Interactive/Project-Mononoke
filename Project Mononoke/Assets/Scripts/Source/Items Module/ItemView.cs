using System;
using System.Collections;
using UnityEngine;
using VContainer;
using static Source.ItemsModule.TrashItemsDatabaseSO;

namespace Source.ItemsModule
{ 
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CircleCollider2D))]
    [Serializable]
   public class ItemView : MonoBehaviour
   {
    [field: SerializeField] public Item Item { get; private set; } = null;
    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    [SerializeField] private CircleCollider2D _collider = null;
    [SerializeField] private float _pickUpAnimationDuration = 1f;
    [field: SerializeField] public bool IsPickUpable { get; private set; } = true;

    [Inject]
    public void Initialize(Item item)
    {
        Item = item;
        UpdateSprite();
    }

    private void OnValidate() 
    {
        _spriteRenderer ??= GetComponent<SpriteRenderer>();
        _collider ??= GetComponent<CircleCollider2D>();
    }

    public void TogglePickUpable(bool signal)
    {
        IsPickUpable = signal;
    }

    private void UpdateSprite()
    {
        var data = Item.Data;
        if(data == null) return;
        _spriteRenderer.sprite = data?.UIData?.Icon;
        if( _spriteRenderer.sprite == null) return;
        AdjustColliderSize();
    }

    private void AdjustColliderSize()
    {
        var radius = _spriteRenderer.bounds.size.x * 0.5f;
        _collider.radius = radius;
    }

    public void BeginPickUp()
    {
        _collider.enabled = false;
        StartCoroutine(PickUpAnimation());
    }

    private IEnumerator PickUpAnimation()
    {
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
