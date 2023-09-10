using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public Transform Container { get; set; }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
        transform.SetParent(Container);
    }
}
