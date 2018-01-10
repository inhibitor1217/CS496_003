using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownHandler_NW : MonoBehaviour {

    public GameObject turret;

    private TurretController_NW controller;
    private float max_size;

    private Vector3 init_scale;

    private float edgeConstant = 0.9f;

    public float cooldown = 1.0f;

	// Use this for initialization
	void Start () {
   
        init_scale = transform.localScale;
        max_size = transform.localScale.y;

    }
	
	// Update is called once per frame
	void Update () {
    
        transform.localScale    = new Vector3(init_scale.x, max_size * cooldown, init_scale.z);
        transform.localPosition = new Vector3(0.0f, edgeConstant * (1.0f - cooldown), 0.0f);

        GetComponent<MeshRenderer>().enabled = turret.GetComponent<TurretController_NW>().ShowCooldownBar;

    }
}
