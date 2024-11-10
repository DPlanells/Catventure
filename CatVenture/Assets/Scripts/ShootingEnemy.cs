using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{

    public float timer = 5f;
    private float BulletTime;

    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float enemySpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShootAtPlayer();
    }

    private void ShootAtPlayer()
    {
        BulletTime -= Time.deltaTime;

        if (BulletTime > 0) return;

        GameObject bulletObject = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        Rigidbody bulletrb = bulletObject.GetComponent<Rigidbody>();
        bulletrb.AddForce(bulletrb.transform.forward * enemySpeed);


        BulletTime = 5;
        Destroy(bulletObject, 5f );
    }
}
