using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoblinComponent : MonoBehaviour, IDamageable
{
    public int VieMax;

    public int Health { get; set; }
    public RectTransform healthPanel;
    public RectTransform deathPanel;
    public RectTransform pointImage;
    public string nextLevel;

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

        for (int i = 0; i < damage && healthPanel.childCount > 0; i++)
        {
            var image = healthPanel.GetChild(healthPanel.childCount - 1);
            Destroy(image.gameObject);
        }

        if (Health <= 0)
        {
            Destroy(healthPanel.GetChild(0).gameObject);
            deathPanel.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void ChangeColorOnHit()
    {
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            TakeDamage(1);
            Debug.Log("hit");
        }
        else if (collision.gameObject.CompareTag("Chest"))
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
