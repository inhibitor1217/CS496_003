using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    private bool collided = false;
    private bool remove = false;
    private bool pop = false;

    public int health = 1;

    public SinglePlayManager gm;

    public GameObject DestroyEffect;
    public Renderer colorRef;

    private Color[] colors = { Color.white, Color.yellow, Color.green, Color.blue, Color.red };

    private void Start() {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent(typeof(SinglePlayManager)) as SinglePlayManager;
        gm.enemyList.Add(this);
    }

    private void Update() {
        if (remove) {
            gm.enemyList.Remove(this);
            Destroy(gameObject);
        }
        if (pop) {
            health--;
            pop = false;
            if (health < 1) {
                Instantiate(DestroyEffect, transform.position, transform.rotation);
                gm.enemyList.Remove(this);
                Destroy(gameObject);
            } else {
                SetHealth(health);
            }
        }
    }

    public void SetHealth(int h) {
        health = h;
        colorRef.material.color = colors[health - 1];
        gameObject.GetComponent<EnemyMovement>().speed = (float) h * 1.5f;
    }

    private void OnDestroy() {
        Instantiate(DestroyEffect, transform);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Home") {
            collided = true;
        }
    }

    public bool getCollided() {
        return collided;
    }

    public void setRemove(bool r) {
        remove = r;
    }

    public void setPop(bool p) {
        pop = p;
    }

}
