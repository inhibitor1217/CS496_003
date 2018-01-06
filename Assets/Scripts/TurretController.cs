using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {

    public float fireRate;
    public bool ShowCooldownBar = true;

    public GameObject Lazer;
    public Transform LazerShotSpawn;

    private float nextFire = 0.0f;

    public float CoolDown() {
        return 1.0f - Mathf.Max( (nextFire - Time.time) / fireRate, 0.0f );
    }

    public void Fire() {
        
        // if(Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Began) {
            if (Time.time > nextFire) {

                nextFire = Time.time + fireRate;

                var spawn = Instantiate(Lazer, new Vector3(transform.position.x, LazerShotSpawn.position.y, transform.position.z), 
                                               LazerShotSpawn.rotation);
                
                spawn.transform.Rotate(new Vector3(transform.rotation.eulerAngles.y + 180.0f, 0.0f, 0.0f));

                spawn.GetComponent<LazerController>().setSource(gameObject);

            }           
        // }

    }

    private void Update() {
        if (Time.time > nextFire) {

            nextFire = Time.time + fireRate;

            var spawn = Instantiate(Lazer, new Vector3(transform.position.x, LazerShotSpawn.position.y, transform.position.z),
                                           LazerShotSpawn.rotation);

            spawn.transform.Rotate(new Vector3(transform.rotation.eulerAngles.y + 180.0f, 0.0f, 0.0f));

            spawn.GetComponent<LazerController>().setSource(gameObject);

        }
    }

}
