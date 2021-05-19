using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour, IPoolable
{
    public ObjectPoolComponent associatedPool { get; set; }

    private void OnDisable()
    {
        associatedPool.PutObject(gameObject);
    }
}
