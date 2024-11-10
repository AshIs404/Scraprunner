using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float fireRate = 1f;    // Time between shots
    protected float nextFireTime = 0f;

    // Method to be implemented by each specific weapon
    public abstract void Fire();

    // Update method to handle automatic firing
    public void UpdateWeapon()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }
}
