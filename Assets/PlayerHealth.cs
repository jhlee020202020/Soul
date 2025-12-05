using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // 최대 체력
    private int currentHealth;   // 현재 체력
    public Animator anim;        // 애니메이터
    private bool isDead = false; // 죽음 상태

    void Start()
    {
        currentHealth = maxHealth;  // 게임 시작 시 체력 초기화
    }

    void Update()
    {
        // 체력이 0 이하로 떨어지면 죽음 애니메이션 실행
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    // 데미지 받는 함수
    public void TakeDamage(int damage)
    {
        if (isDead) return;  // 이미 죽었으면 더 이상 데미지를 받지 않음

        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    // 죽을 때 호출되는 함수
    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");  // 죽음 애니메이션 트리거
        // 죽음 애니메이션 끝나면 아무 동작도 하지 않도록 처리
    }

    // 죽음 애니메이션이 끝난 후 호출되는 함수 (애니메이션 이벤트에서 호출)
    public void OnDeathAnimationEnd()
    {
        // 죽은 후 더 이상 아무것도 하지 않도록 설정
        // 예를 들어, 게임 오버 화면을 표시할 수도 있음
        Debug.Log("Player died. Game over.");
    }

    // 플레이어 초기화 함수 (게임 리로드 후 호출)
    public void InitializePlayer()
    {
        isDead = false;  // 죽지 않은 상태로 초기화
        currentHealth = maxHealth;  // 체력 초기화
        anim.SetTrigger("Respawn");  // 리스폰 애니메이션 트리거
    }
}

