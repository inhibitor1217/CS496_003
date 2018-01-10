using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera_Action : MonoBehaviour {

    public GameObject player;

    public float offsetX = 0f;
    public float offsetY = 7f;
    public float offsetZ = -6f;

    public float leftBoundX;
    public float rightBoundX;
    public float topBoundZ;
    public float bottomBoundZ;

    public float followSpeed = 1f;
    
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = new Vector3(offsetX, offsetY, offsetZ);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private float Bound(float x, float min, float max) {
        return Mathf.Max(Mathf.Min(x, max), min);
    }

    private void LateUpdate()
    {

        Vector3 designatedPosition = new Vector3(Bound(player.transform.position.x, leftBoundX, rightBoundX),
                                                 player.transform.position.y,
                                                 Bound(player.transform.position.z, bottomBoundZ, topBoundZ)) + offset;
        transform.position = Vector3.Lerp(transform.position, designatedPosition, followSpeed * Time.deltaTime);
        
    }
    
}
