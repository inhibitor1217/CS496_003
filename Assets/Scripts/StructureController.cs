using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureController : MonoBehaviour {
    public float X_POSITION_MAX_VALUE = 10.5f;
    public float Y_POSITION_MAX_VALUE = 10.5f;
    public float LENGTH_BETWEEN_BOX = 3f;


    GameObject structureControllerObject;
    StructuresLocationController structureLocationController;


    // structuresLocationController에서 관리할 x,y index 좌표
    int indexX;
    int indexY;

    // Use this for initialization
    public virtual void Start () {
        // structure 컨트롤러 등록
        structureControllerObject = GameObject.FindGameObjectWithTag("StructureLocationController");
        structureLocationController = structureControllerObject.GetComponent<StructuresLocationController>();

        // 물체의 현재 좌표를 받아와서 indexX, indexY를 계산
        indexX = (int)(transform.position.x + X_POSITION_MAX_VALUE) / (int)LENGTH_BETWEEN_BOX;
        indexY = -(int)(transform.position.z - Y_POSITION_MAX_VALUE) / (int)LENGTH_BETWEEN_BOX;

        // 좌표를 넣어줌
        structureLocationController.initializeStructure(indexX, indexY);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    // 성공시 0, 실패시 -1 반환
    public int setStructureIndex(int inputX, int inputY)
    {
        if (inputX < 0 || inputX > 7 || inputY < 0 || inputY > 7)
        {
            Debug.Log("ERROR. INDEX RANGE OVER");
            return -1;
        }
        indexX = inputX;
        indexY = inputY;

        return 1;
    }

    public int getIndexX()
    {
        return indexX;
    }

    public int getIndexY()
    {
        return indexY;
    }

    private void OnDestroy()
    {
        // 파괴될때 컨트롤러 배열에서도 삭제
        structureLocationController.setEmpty(indexX, indexY);
    }
}
