using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHealth : MonoBehaviour
{
    public int maxHealth = 300;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // SwordHitbox에서 Damage를 주면 호출됨
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log("Dragon Hit! HP : " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Dragon Dead");
        // TODO: 죽는 애니메이션, NavMesh 중지, AI 멈춤 등
    }
}

