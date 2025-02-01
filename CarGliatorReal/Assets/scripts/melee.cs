using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class melee : NetworkBehaviour
{
    [SerializeField] private float attackForce;
    [SerializeField] private Animator animator;


    public void Update()
    {
        if (!IsOwner) return;
            
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetBool("attack", true);
            
        }
    }
        private void OnTriggerStay(Collider other)
        {
            if (!IsOwner) return;
    
            if (other.gameObject.CompareTag("Player"))
            {
                print("hit");
                // Get the direction the object is facing (forward vector)
                Vector3 dir = transform.forward;

                // Apply the knockback force to the enemy's rigidbody
                other.attachedRigidbody.AddForce(dir.normalized * attackForce, ForceMode.Impulse);

                other.GetComponent<playerManager>().health -= gameObject.GetComponentInParent<playerManager>().damage;
            }

        }

    } 


