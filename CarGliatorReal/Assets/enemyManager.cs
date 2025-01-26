using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float kbForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("fence"))
        {
            print("x: " + -rb.velocity.z * kbForce);
            rb.AddForce( collision.transform.forward * - kbForce, ForceMode.Impulse);
        }
    }
}
