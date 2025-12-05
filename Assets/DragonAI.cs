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

        if (isAttacking)
        {
            agent.isStopped = true;
            anim.SetBool("Walk", false);
            LookAtPlayer();
            return;
        }

        if (isWaiting)
        {
            agent.isStopped = true;
            anim.SetBool("Walk", false);
            LookAtPlayer();
            return;
        }

        if (isCoolingDown)
        {
            agent.isStopped = true;
            anim.SetBool("Walk", false);
            LookAtPlayer();
            return;
        }

        if (distance > dashDistance && dashAvailable && !isDashing)
        {
            StartCoroutine(DoDash());
            return;
        }

        if (distance <= attackDistance)
        {
            StartCoroutine(PrepareAndAttack());
            return;
        }

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

    void LookAtPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        if (dir.magnitude < 0.1f) return;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }

    void ChasePlayer()
    {
        if (isDashing) return;

        anim.SetBool("Walk", true);
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

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

    // 🔥 브레스 시작
    public void StartFireBreath()
    {
        if (fireBreathFX == null) return;

        fireBreathFX.Play();
        isBreathingFire = true;

        StartCoroutine(FireDamageLoop());
    }

    // 🔥 브레스 종료
    public void StopFireBreath()
    {
        if (fireBreathFX == null) return;

        fireBreathFX.Stop();
        isBreathingFire = false;
    }

    // 🔥 불 데미지 반복 적용
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







