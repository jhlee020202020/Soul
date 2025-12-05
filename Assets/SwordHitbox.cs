using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public int damage = 20;  // 공격 데미지
    private bool active = false;

    public void EnableHitbox() => active = true;
    public void DisableHitbox() => active = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!active) return;

        // 다리 콜라이더만 피격되도록 "DragonLeg" 태그 체크
        if (other.CompareTag("DragonLeg"))
        {
            DragonHealth health = other.GetComponentInParent<DragonHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}



