using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera_Action : MonoBehaviour {

    public GameObject player;

    public float offsetX = 0f;
    public float offsetY = 7f;
    public float offsetZ = -6f;

    public float followSpeed = 1f;
    
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = new Vector3(offsetX, offsetY, offsetZ);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private float Boundary(float x) {
        return Mathf.Max(Mathf.Min(x, 3.0f), -3.0f);
    }

    private void LateUpdate()
    {

        Vector3 designatedPosition = new Vector3(Boundary(player.transform.position.x),
                                                 player.transform.position.y,
                                                 Boundary(player.transform.position.z)) + offset;
        transform.position = Vector3.Lerp(transform.position, designatedPosition, followSpeed * Time.deltaTime);
        
    }
    

}
