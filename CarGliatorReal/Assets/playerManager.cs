using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float kbForce;
    [SerializeField] int maxHealth;
    public int health;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("fence"))
        {
            rb.AddForce( collision.transform.forward * - kbForce, ForceMode.Impulse);
            health -= 2;
        }
    }
}
