using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // 공격력과 관련된 변수
    public int attackDamage = 20;

    // 공격이 발동하는 메서드
    public void Attack()
    {
        // 공격 로직
        Debug.Log("Player is attacking!");

        // 공격하는 로직 추가 (예: 충돌 검사)
    }
}

