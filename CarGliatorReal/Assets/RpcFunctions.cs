using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RpcFunctions : NetworkBehaviour
{
    ParticleSystem fireRight;
    ParticleSystem fireLeft;


    [ServerRpc(RequireOwnership = false)]
    public void TurnOnPsServerRpc(/*ParticleSystem ps,*/ bool turnOn)
    {
        if (turnOn)
        {
            fireRight.Play();
            fireLeft.Play();
        }
        else
        {
            fireRight.Stop();
            fireLeft.Stop();
        }        
    }

    public void SetFire(ParticleSystem r, ParticleSystem l)
    {
        this.fireRight = r;
        this.fireLeft = l;
    }
}
