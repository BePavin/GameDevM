using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Bullet bullet;
    public Transform spawnBullet;
    public float fireRate = 0.2f;

    private bool canShoot = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && canShoot)
        {
            StartCoroutine(SpawnBullet());
        }
    }

    IEnumerator SpawnBullet()
    {
        canShoot = false;
        Instantiate(bullet, spawnBullet.position, transform.rotation);
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
