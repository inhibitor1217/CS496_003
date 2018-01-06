using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {

    public float fireRate;

    public GameObject Lazer;
    public Transform LazerShotSpawn;

    private float nextFire = 0.0f;

    private void Update() {
        
        if(Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Began) {
            if(Time.time > nextFire) {

                nextFire = Time.time + fireRate;

                var lazer = Instantiate(Lazer, LazerShotSpawn.position, LazerShotSpawn.rotation);
                lazer.transform.parent = transform;

            }
        }

    }

}
