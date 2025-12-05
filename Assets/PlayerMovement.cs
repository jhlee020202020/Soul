using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 1.5f;
    public float runSpeed = 4f;

    public Transform cam;  // ⭐ 카메라 기준 이동 필수
    public SwordHitbox swordHitbox;

    private Animator anim;
    private bool isAttacking = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
{
    // 공격 중이면 이동 애니 잠금 + 실제 이동도 잠금
    if (isAttacking)
    {
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", 0);
        anim.SetFloat("Blend", 0);
        return; // 이동 자체를 멈춤
    }

    HandleMovement();

    if (Input.GetMouseButtonDown(0))
        Attack();
}



    // ---------------------------------------
    // ⭐ 소울류 카메라 기준 이동
    // ---------------------------------------
    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        anim.SetBool("IsRunning", isRunning);

        float speed = isRunning ? runSpeed : walkSpeed;


        // 🔥 카메라 방향 기준 이동 벡터 만들기
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        // 이동 방향 = 카메라 기준으로 변환
        Vector3 dir = (camForward * v + camRight * h).normalized;

        // 이동 + 부드러운 회전
        if (dir.magnitude > 0)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);

            transform.position += dir * speed * Time.deltaTime;
        }

        anim.SetFloat("Blend", Mathf.Clamp01(new Vector2(h, v).magnitude));
    }


    // ---------------------------------------
    // 🔥 공격 입력 처리
    // ---------------------------------------
    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        anim.SetBool("Attack", true);

        // 히트박스 ON
        if (swordHitbox != null)
            swordHitbox.EnableHitbox();
    }


    // ---------------------------------------
    // 🔥 애니메이션 이벤트 — 히트박스 제어
    // ---------------------------------------
    public void EnableHitbox()
    {
        if (swordHitbox != null)
            swordHitbox.EnableHitbox();
    }

    public void DisableHitbox()
    {
        if (swordHitbox != null)
            swordHitbox.DisableHitbox();
    }


    // ---------------------------------------
    // 🔥 공격 종료 이벤트 — Idle로 자연스럽게 복귀
    // ---------------------------------------
   public void OnAttackEnd()
{
    isAttacking = false;
    anim.SetBool("Attack", false);
}

}















