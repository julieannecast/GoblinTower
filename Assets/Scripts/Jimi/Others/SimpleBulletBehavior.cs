using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveArcComponent))]
public class SimpleBulletBehavior : MonoBehaviour, IPoolable
{
    public ObjectPoolComponent associatedPool { get; set; }

    private MoveArcComponent moveArcComponent;

    private void OnDisable()
    {
        associatedPool.PutObject(gameObject);
    }
}
