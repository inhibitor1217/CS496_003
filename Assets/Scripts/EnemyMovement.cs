using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    private List<Transform> WayPoints;

    private int curPosition = 0;
    private Transform target;

    public float speed = 1.0f;

	void Start () {
        EnemyPath path = GameObject.FindGameObjectWithTag("EnemyPath").GetComponent(typeof(EnemyPath)) as EnemyPath;
        WayPoints = path.WayPoints;
	}
	
	void Update () {
		if(curPosition < WayPoints.Count) {
            if(target == null) {
                target = WayPoints[curPosition];
            }
            transform.forward = Vector3.RotateTowards(transform.forward, target.position - transform.position, speed * Time.deltaTime, 0.0f);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            if(transform.position == target.position) {
                curPosition++;
                if(curPosition < WayPoints.Count) target = WayPoints[curPosition];
            }
        }
	}

}
