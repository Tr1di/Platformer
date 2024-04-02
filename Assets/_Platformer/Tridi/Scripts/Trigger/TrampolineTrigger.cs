using UnityEngine;

namespace Tridi
{
    public class TrampolineTrigger : Trigger
    {
        [SerializeField] private Vector2 direction = Vector2.up;
        [SerializeField] private float force = 20f;

        public override void Enter(Collider2D other)
        {
            var vel = other.attachedRigidbody.velocity;
            vel.y = Mathf.Abs(vel.y);
            vel += direction.normalized * force;
            other.attachedRigidbody.velocity = vel;
        }
    }
}

