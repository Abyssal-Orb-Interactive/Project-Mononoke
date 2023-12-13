using UnityEngine;

namespace Source.Movement
{
    [RequireComponent(typeof(Collider2D))]
    public class GroundChecker : MonoBehaviour
    {
        public Vector2 Normal { get; private set; }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Normal = collision.contacts[0].normal;
        }
    }
}