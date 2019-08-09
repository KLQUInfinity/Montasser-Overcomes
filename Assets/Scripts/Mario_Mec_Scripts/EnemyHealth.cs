using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{

    public float enemyMaxHealth;

    private float currentHealth;

    void Start()
    {
        currentHealth = enemyMaxHealth;
    }

    public void addDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            makeDead();
        }
    }

    void makeDead()
    {
        Destroy(gameObject);
    }
}
