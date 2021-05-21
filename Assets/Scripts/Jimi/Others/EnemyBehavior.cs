using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] Transform exit;
    [SerializeField] float cooldown;
    [SerializeField] ObjectPoolComponent bulletObjectPool;
    [SerializeField] Transform target;
    [SerializeField] float aggroRange;
    [SerializeField] float barrelRecoil;
    [SerializeField] Transform barrel;

    private bool isShooting;
    private Vector3 barrelStartPosition;
    private float recoil;

    private void Start()
    {
        if (barrel) barrelStartPosition = barrel.position;
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        transform.LookAt(target);
        isShooting = TargetIsClose(target.position);

        if (barrel)
        {
            recoil = Mathf.Max(0, recoil - Time.deltaTime * (1 / cooldown));
            barrel.position = barrelStartPosition + barrel.up * -recoil;
        }
    }

    IEnumerator Spawn() 
    {
        while (true)
        {
            if (isShooting) 
            {
                try
                {
                    var recycledBullet = bulletObjectPool.GetObject();
                    ResetBullet(recycledBullet);
                    recoil = barrelRecoil;
                } catch { }
            }

            yield return new WaitForSecondsRealtime(cooldown);
        }
    }

    private bool TargetIsClose(Vector3 target) =>
        Vector3.Distance(transform.position, target) <= aggroRange;

    private void ResetBullet(GameObject recycledBullet)
    {
        recycledBullet.transform.position = exit.position;
        recycledBullet.transform.rotation = exit.rotation;
        recycledBullet.SetActive(true);
    }
}
