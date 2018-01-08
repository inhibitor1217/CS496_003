using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructuresLocationController : MonoBehaviour {

    // 각 칸에 structure의 정보가 저장되어있는 8x8의 2차원 배열
    int[,] structureArray;

    const int EMPTY = 0;
    const int THING = 1;

	void Awake () {

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

        // instantiate


    }
	
	// Update is called once per frame
	void Update () {
    }

    // 입력받은 inputX, inputY와 direction을 바탕으로 해당칸으로 이동할 수 있는지 판별하고
    // 가능하다면 해당 MirrorController에 이동 명령을 내림.
    public void moveCommand (string direction, MirrorController mc, int inputX, int inputY)
    {
        int xChanged = 0;
        int yChanged = 0;

        switch(direction)
        {
            case "Left":

                // 맨 왼쪽일 경우
                if (inputX <= 0) return;
                // 해당 array의 왼편이 빈 공간이 아닐경우
                if (structureArray[inputX-1, inputY] != EMPTY) return;

                xChanged = -1;

                break;

            case "Right":

                // 맨 오른쪽일 경우
                if (inputX >= 7) return;
                // 해당 array의 오른쪽이 빈 공간이 아닐경우
                if (structureArray[inputX+1, inputY] != EMPTY) return;

                xChanged = 1;

                break;

            case "Up":

                // 맨 위쪽일 경우
                if (inputY <= 0) return;
                // 해당 array의 위쪽이 빈 공간이 아닐경우
                if (structureArray[inputX, inputY-1] != EMPTY) return;

                yChanged = -1;

                break;

            case "Down":

                // 맨 아래쪽일 경우
                if (inputY >= 7) return;
                // 해당 array의 위쪽이 빈 공간이 아닐경우
                if (structureArray[inputX, inputY+1] != EMPTY) return;

                yChanged = 1;
                break;

            default:
                Debug.Log("ERROR : NO INPUT DIRECTION");
                return;
        }

        // 기존 위치의 배열값을 EMPTY로 설정
        setEmpty(inputX, inputY);

        // 새로 바뀐 위치의 배열값을 THING으로 설정
        setThing(inputX + xChanged, inputY + yChanged);

        // 각 mirror controller 안의 structure의 index값을 재설정
        mc.setStructureIndex(inputX + xChanged, inputY + yChanged);

        // 이동 명령을 시작(이땐 움직이는 모션을 갓 시작한 상태)
        mc.MoveOneTowardLocation(direction);

    }

    public int setEmpty(int indexX, int indexY)
    {
        if (!isItInBoundary(indexX,indexY))
        {
            Debug.Log("ERROR : OUT OF BOUNDARY");
            return -1;
        }
        if (structureArray[indexX, indexY] == EMPTY)
        {
            Debug.Log("ERROR : THAT LOCATION IS ALREADY EMPTY");
            return -1;
        }
        structureArray[indexX, indexY] = EMPTY;
        //print("changed EMPTY TO : [" + indexX + "] [" + indexY +"]");
        return 1;
    }

    public int setThing(int indexX, int indexY)
    {
        if (!isItInBoundary(indexX, indexY))
        {
            Debug.Log("ERROR : OUT OF BOUNDARY");
            return -1;
        }
        if (structureArray[indexX, indexY] == THING)
        {
            Debug.Log("ERROR : THAT LOCATION IS ALREADY THING");
            return -1;
        }
        structureArray[indexX, indexY] = THING;
        //print("changed THING TO : [" + indexX + "] [" + indexY + "]");
        return 1;
    }

    public bool checkSomethingIsThere(int indexX, int indexY)
    {
        if (structureArray[indexX, indexY] == EMPTY)
        {
            return false;
        }
        else return true;
    }


    public void initializeStructure(int indexX, int indexY)
    {
        structureArray[indexX, indexY] = THING;
    }


    private bool isItInBoundary(int indexX, int indexY)
    {
        if (indexX >= 0 && indexX <= 7 && indexY >= 0 && indexY <= 7)
        {
            return true;
        }
        else return false;
    }
}
