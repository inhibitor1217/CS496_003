using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureController : MonoBehaviour {
    public float X_POSITION_MAX_VALUE = 10.5f;
    public float Y_POSITION_MAX_VALUE = 10.5f;
    public int LENGTH_BETWEEN_BOX = 3;


    GameObject structureControllerObject;
    StructuresLocationController structureLocationController;

    public bool isAlive = true;

    private MasterController master;

    // structuresLocationController에서 관리할 x,y index 좌표
    public int indexX;
    public int indexY;

    // Use this for initialization
    public virtual void Start () {
        // structure 컨트롤러 등록
        structureControllerObject = GameObject.FindGameObjectWithTag("StructureLocationController");
        structureLocationController = structureControllerObject.GetComponent<StructuresLocationController>();

        // 물체의 현재 좌표를 받아와서 indexX, indexY를 계산
        //indexX = Mathf.RoundToInt(transform.position.x + X_POSITION_MAX_VALUE) / LENGTH_BETWEEN_BOX;
        //indexY = Mathf.RoundToInt(-(transform.position.z - Y_POSITION_MAX_VALUE)) / LENGTH_BETWEEN_BOX;

        // 좌표를 넣어줌
        structureLocationController.initializeStructure(indexX, indexY);

        master = GameObject.FindGameObjectWithTag("MasterController").GetComponent(typeof(MasterController)) as MasterController;
        master.list.Add(this);
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

}
