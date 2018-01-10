using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {

    private float[] timeCount = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
    private float[] timeGap   = { 100.0f, 100.0f, 100.0f, 100.0f, 100.0f, 100.0f };

    private int[] numLeft = { 0, 0, 0, 0, 0, 0 };

    private GameObject enemy;

    public Transform Spawn;

    private SinglePlayManager gm;

    private void Start() {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SinglePlayManager>();
    }

    private void Update() {
        
        for (int i = 0; i < 5; i++) {
            if(numLeft[i] > 0) { 
                if (timeCount[i] > timeGap[i]) {
                    timeCount[i] = 0.0f;
                    var obj = Instantiate(enemy, Spawn);
                    obj.GetComponent<EnemyAttack>().SetHealth(i + 1);
                    numLeft[i]--;
                }
                timeCount[i] += Time.deltaTime;
            }
        }

        bool waveGenFin = true;
        for(int i = 0; i < 5; i++) {
            if (numLeft[i] > 0) waveGenFin = false;
        }
        if (waveGenFin) gm.WaveGenFinished();

    }

    public void Generate(GameObject obj, int num, float TimeGap, int enemyHealth) {

        timeGap[enemyHealth] = TimeGap;
        timeCount[enemyHealth] = 0.0f;
        numLeft[enemyHealth] = num;
        enemy = obj;

    }

}
