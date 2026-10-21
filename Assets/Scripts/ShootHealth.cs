using UnityEngine;
using Fusion;

public class ShootHealth : NetworkBehaviour
{
    [Networked]
    public int health {  get; set; }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            health = 100;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]

    public void RPC_ReceiveDamage(RpcInfo info = default)
    {
        if (HasStateAuthority) return;

        ShootMatch match = ShootMatch.Instance;

        if (match == null || !match.CanPlay) return;

        if (health <= 0) return;

        if (info.Source == Object.StateAuthority) return;

        Debug.Log("Jogador recebeu dano. Vida: " +  health);

        if(health == 0)
        {
            match.RPC_ReportDefeat(Object.StateAuthority);
        }
    }
}
