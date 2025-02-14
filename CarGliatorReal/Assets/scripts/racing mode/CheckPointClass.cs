using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointClass : MonoBehaviour
{
    public BoxCollider bc;
    public bool archived = false;

    public void builder()
    {
        bc = null;
        archived = false;
    }
}
