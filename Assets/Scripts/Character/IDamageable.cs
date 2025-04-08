using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageType damageType, int damage);
    public void TakeMPDamage(int damage);
    public void TakeAPDamage(int damage);
    void Die();


}
