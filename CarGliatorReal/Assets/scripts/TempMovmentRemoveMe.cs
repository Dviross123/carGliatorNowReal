using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TempMovmentRemoveMe : NetworkBehaviour
{
    Rigidbody rb;
    [SerializeField] float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.right * speed * Time.deltaTime,ForceMode.Acceleration);
        }
    }
}
