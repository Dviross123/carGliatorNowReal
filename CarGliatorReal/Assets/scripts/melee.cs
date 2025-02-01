using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class melee : MonoBehaviour
{
    [SerializeField] private float attackForce;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (other.gameObject.CompareTag("enemy"))
            {
                // Get the direction the object is facing (forward vector)
                Vector3 dir = transform.forward;

                // Apply the knockback force to the enemy's rigidbody
                other.attachedRigidbody.AddForce(dir.normalized * attackForce, ForceMode.Impulse);

                other.GetComponent<playerManager>().health -= gameObject.GetComponentInParent<playerManager>().damage;
            }
        }
    }

}
