using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;


public class AnimationEvents : NetworkBehaviour
{
    [SerializeField] GameObject dust;
    void SetBoolFalse(string boolNaME)
    {
        Animator anim= GetComponent<Animator>();
        anim.SetBool(boolNaME, false);
        GetComponent<melee>().canDamage = true;
    }

    [ServerRpc(RequireOwnership = false)]
    void MakeParticlesServerRpc()
    {
        GameObject spawnDustPs =  Instantiate(dust, transform.position + new Vector3(0,0,11), quaternion.identity);
        spawnDustPs.GetComponent<NetworkObject>().Spawn(true);
        
    }
}
