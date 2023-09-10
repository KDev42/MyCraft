using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class PartickesBroken : MonoBehaviour
{
    private PoolObject poolObject => GetComponent<PoolObject>();

    public void OnParticleSystemStopped()
    {
        poolObject.ReturnToPool();
    }
}
