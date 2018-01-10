using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurretController_NW : StructureController_NW {

    public float fireRate;
    public bool ShowCooldownBar = true;

    public GameObject Lazer;
    public Transform LazerShotSpawn;

    [SyncVar]
    public float nextFire = 0.0f;

    [SyncVar]
    private float t = 0.0f;

    private CountdownHandler_NW CH;

    public override void Start() {
        base.Start();
        nextFire = fireRate;
        t = 0.0f;
        CH = GetComponentInChildren(typeof(CountdownHandler_NW)) as CountdownHandler_NW;
    }

    public float CoolDown() {
        return 1.0f - Mathf.Max( (nextFire - t) / fireRate, 0.0f );
    }
    
    public void Fire() {
        
        // if(Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Began) {
            if (t > nextFire) {

                nextFire = t + fireRate;

                var spawn = Instantiate(Lazer, new Vector3(transform.position.x, LazerShotSpawn.position.y, transform.position.z), 
                                               LazerShotSpawn.rotation);
                
                spawn.transform.Rotate(new Vector3(transform.rotation.eulerAngles.y + 180.0f, 0.0f, 0.0f));

                spawn.GetComponent<LazerController_NW>().setSource(gameObject);

                NetworkServer.Spawn(spawn);

                GetComponent<AudioSource>().Play();

            }           
        // }

    }

    private void Update() {
        t += Time.deltaTime;
        if(t > nextFire + 5.0f * fireRate) {
            Fire();
        }
        CH.cooldown = CoolDown();
    }


}
