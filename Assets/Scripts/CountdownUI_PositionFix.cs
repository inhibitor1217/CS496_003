using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownUI_PositionFix : MonoBehaviour {

    public GameObject turret;

    private Vector3 offset_position;
    private Quaternion offset_rotation;

	// Use this for initialization
	void Start () {

        offset_position = transform.localPosition;
        offset_rotation = Quaternion.Euler(0f, 0f, 90f);

	}
	
	// Update is called once per frame
	void Update () {

        transform.rotation = offset_rotation;
        transform.position = transform.parent.position + offset_position;

        GetComponent<MeshRenderer>().enabled = turret.GetComponent<TurretController>().ShowCooldownBar;

    }
}
