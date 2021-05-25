using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPointsComponent : MonoBehaviour, IDamageable
{
    public int VieMax;

    public int Health { get; set; }
    public RectTransform healthPanel;
    public RectTransform deathPanel;
    public RectTransform pointImage;
    private void Awake()
    {
        Health = VieMax;
        for(int i = 0; i < Health; ++i)
        {
            var image = Instantiate(pointImage);
            image.SetParent(healthPanel);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health > 0)
        {
            for (int i = 0; i < damage; i++)
            {
                var image = healthPanel.GetChild(healthPanel.childCount - 1);
                Destroy(image.gameObject);
            }
        }
        else
        {
            deathPanel.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void ChangeColorOnHit()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Ouille"))
        {
            TakeDamage(1);
        }
    }
}
