using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructuresLocationController : MonoBehaviour {

    // 각 칸에 structure의 정보가 저장되어있는 8x8의 2차원 배열
    int[,] structureArray;

    const int EMPTY = 0;
    const int MIRROR = 1;
    const int CANNON = 2;

	void Start () {

        // 배열 초기화
        structureArray = new int[8, 8] {
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
        };



    }
	
	// Update is called once per frame
	void Update () {
	}

    // 입력받은 inputX, inputY와 direction을 바탕으로 해당칸으로 이동할 수 있는지 판별하고
    // 가능하다면 해당 MirrorController에 이동 명령을 내림.
    public void moveCommand (string direction, MirrorController mc, int inputX, int inputY)
    {

        switch(direction)
        {
            case "Left":

                // 맨 왼쪽일 경우
                if (inputX <= 0) return;
                // 해당 array의 왼편이 빈 공간이 아닐경우
                if (structureArray[inputX-1, inputY] != EMPTY) return;

                break;

            case "Right":

                // 맨 오른쪽일 경우
                if (inputX >= 7) return;
                // 해당 array의 오른쪽이 빈 공간이 아닐경우
                if (structureArray[inputX+1, inputY] != EMPTY) return;

                break;

            case "Up":

                // 맨 위쪽일 경우
                if (inputY <= 0) return;
                // 해당 array의 위쪽이 빈 공간이 아닐경우
                if (structureArray[inputX, inputY-1] != EMPTY) return;

                break;

            case "Down":

                // 맨 아래쪽일 경우
                if (inputY >= 7) return;
                // 해당 array의 위쪽이 빈 공간이 아닐경우
                if (structureArray[inputX, inputY+1] != EMPTY) return;

                break;

            default:
                Debug.Log("ERROR : NO INPUT DIRECTION");
                return;
        }

        mc.MoveOneTowardLocation(direction);

    }

    public bool checkSomethingIsThere(int x, int y)
    {
        if (structureArray[x, y] == EMPTY)
        {
            return false;
        }
        else return true;
    }
}
