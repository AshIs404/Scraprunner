using UnityEngine;

public class MissileLauncher : Weapon
{
    public GameObject missilePrefab; // Prefab for the missile
    public Transform firePoint;      // Point from which missiles are launched

    public override void Fire()
    {
        // Instantiate and fire the missile
        Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
        Debug.Log("MissileLauncher fired!");
    }
}
