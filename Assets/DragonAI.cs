using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DragonAI : MonoBehaviour
{
    public Transform player;

    public float chaseDistance = 30f;
    public float attackDistance = 10f;
    public float waitBeforeAttack = 1.2f;
    public float rotationSpeed = 3f;

    // 🔥 Dash 관련 변수
    public float dashDistance = 22f;  
    public float dashCooldown = 5f;   
    private bool isDashing = false;
    private bool dashAvailable = true;

    // 🔥 Fire Breath 관련
    public ParticleSystem fireBreathFX;
    public float fireDamageDistance = 12f;
    public float fireDamageInterval = 0.2f;
    public int fireDamageAmount = 20;     // 🔥 데미지 수치 추가
    private bool isBreathingFire = false;

    private NavMeshAgent agent;
    private Animator anim;

    private bool isWaiting = false;
    private bool isAttacking = false;
    private bool isCoolingDown = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // ============================
        // 공격 애니메이션 도중
        // ============================
        if (isAttacking)
        {
            agent.isStopped = true;
            anim.SetBool("Walk", false);
            LookAtPlayer();
            return;
        }

        // ============================
        // 공격 준비 중
        // ============================
        if (isWaiting)
        {
            agent.isStopped = true;
            anim.SetBool("Walk", false);
            LookAtPlayer();
            return;
        }

        // ============================
        // 공격 후 쿨타임
        // ============================
        if (isCoolingDown)
        {
            agent.isStopped = true;
            anim.SetBool("Walk", false);
            LookAtPlayer();
            return;
        }

        // ============================
        // 🌀 Dash 조건
        // ============================
        if (distance > dashDistance && dashAvailable && !isDashing)
        {
            StartCoroutine(DoDash());
            return;
        }

        // ============================
        // 공격 거리
        // ============================
        if (distance <= attackDistance)
        {
            StartCoroutine(PrepareAndAttack());
            return;
        }

        // ============================
        // 추적
        // ============================
        if (distance <= chaseDistance)
        {
            ChasePlayer();
        }
        else
        {
            agent.isStopped = true;
            anim.SetBool("Walk", false);
        }
    }

    // ======================
    // 플레이어 바라보기
    // ======================
    void LookAtPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        if (dir.magnitude < 0.1f) return;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }

    // ======================
    // Chase
    // ======================
    void ChasePlayer()
    {
        if (isDashing) return;

        anim.SetBool("Walk", true);
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    // ======================
    // 공격 준비 → 공격
    // ======================
    IEnumerator PrepareAndAttack()
    {
        if (isWaiting || isAttacking || isCoolingDown)
            yield break;

        isWaiting = true;

        agent.isStopped = true;
        anim.SetBool("Walk", false);
        LookAtPlayer();

        yield return new WaitForSeconds(waitBeforeAttack);

        isWaiting = false;
        isAttacking = true;

        anim.SetTrigger("Attack");
    }

    // ======================
    // 공격 종료
    // ======================
    public void OnAttackEnd()
    {
        StopFireBreath(); 
        isAttacking = false;
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(2f);
        isCoolingDown = false;
    }

    // ======================
    // Dash 기능
    // ======================
    IEnumerator DoDash()
    {
        dashAvailable = false;
        isDashing = true;

        agent.isStopped = true;
        anim.SetTrigger("Dash");

        LookAtPlayer();

        yield return new WaitForSeconds(1.0f);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        dashAvailable = true;
    }

    // ======================
    // 🔥 불 브레스 시작 (Animation Event)
    // ======================
    public void StartFireBreath()
    {
        if (fireBreathFX == null) return;

        fireBreathFX.Play();
        isBreathingFire = true;

        StartCoroutine(FireDamageLoop());
    }

    // ======================
    // 🔥 불 브레스 종료 (Animation Event)
    // ======================
    public void StopFireBreath()
    {
        if (fireBreathFX == null) return;

        fireBreathFX.Stop();
        isBreathingFire = false;
    }

    // ======================
    // 🔥 불 데미지 반복 적용
    // ======================
    IEnumerator FireDamageLoop()
    {
        PlayerHealth hp = player.GetComponent<PlayerHealth>(); // 🔥 추가된 코드

        while (isBreathingFire)
        {
            if (hp != null)
            {
                float dist = Vector3.Distance(transform.position, player.position);

                if (dist < fireDamageDistance)
                {
                    hp.TakeDamage(fireDamageAmount);   // 🔥 데미지 적용
                }
            }

            yield return new WaitForSeconds(fireDamageInterval);
        }
    }
}
