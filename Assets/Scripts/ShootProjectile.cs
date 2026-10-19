using Fusion;
using UnityEngine;

public class ShootProjectile : NetworkBehaviour
{
    [SerializeField] float speed = 12f;
    [SerializeField] float lifeTime = 2.5f;
    [SerializeField] float hitRadious = 0.15f;

    float reminingLife;

    public override void Spawned()
    {
        reminingLife = lifeTime;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        ShootMatch match = ShootMatch.Instance;

        if (match != null && match.IsOver)
        {
            Runner.Despawn(Object);
            return;
        }

        Vector3 movement = transform.forward * speed * Runner.DeltaTime;

        bool hitSomething = Physics.SphereCast(transform.position, hitRadious, movement.normalized, out RaycastHit hit,
            movement.magnitude, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        if (hitSomething)
        {
            ShootHealth target = hit.collider.GetComponentInParent<ShootHealth>();

            if (target != null)
            {
                if (target.Object.State == Object.StateAuthority)
                {
                    transform.position += movement;
                    return;
                }

                target.RPC_ReceiveDamage();

                Runner.Despawn(Object);
                return;
            }

            Runner.Despawn(Object);
            return;
        }

        transform.position += movement;
    }
}
