using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    private Animator anim;

    void Start()
    {
        // Animator 컴포넌트 할당 확인
        anim = GetComponent<Animator>();
        
        // Animator가 제대로 할당되지 않은 경우 예외 처리
        if (anim == null)
        {
            Debug.LogError("Animator component not found on this object!");
        }

        currentHealth = maxHealth;  // 체력 초기화
    }

    void Update()
    {
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return; // 이미 죽었으면 데미지 받지 않음

        currentHealth -= damage; // 데미지 받으면 체력 감소
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log("Current Health: " + currentHealth);  // 체력 로그 출력
    }

    void Die()
    {
        if (anim != null)
        {
            anim.SetTrigger("Die"); // 'Die' 트리거 활성화
        }

        // 추가적으로 플레이어가 죽을 때 필요한 처리를 해줍니다.
        // 예: 이동 비활성화, 공격 비활성화 등.
        GetComponent<PlayerMovement>().enabled = false;  // 예: 이동 비활성화
    }
}
