using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorController : StructureController {

    public float structureRotateDuration = 0.5f;
    public float structureMovingDuration = 1f;
    public float structureRotateSpeed = 200;

    // 다른 오브젝트에 의해 상호작용을 받고 있는 상태인지 표시하는 변수
    private bool isThisMoving = false;

    // 90도 회전 시작 활성화
    private bool rotateFlag = false;

    // 한칸 자동 이동 시작 활성화
    private bool moveFlag = false;

    // 이미 Rotate로 돌아가고 있는지 표시하는 변수
    private bool isThisRotating = false;

    Quaternion startingRotation;
    Quaternion endRotation;

    Vector3 startPosition;
    Vector3 targetPosition;



    // Use this for initialization
    public override void Start () {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        if (rotateFlag == true)
        {
            StartCoroutine(RotateObject(startingRotation, endRotation,structureRotateDuration));
            rotateFlag = false;
        }
        if (moveFlag == true)
        {
            StartCoroutine(MoveObject(startPosition, targetPosition, structureMovingDuration));
            moveFlag = false;
        }
    }



    // 외부에서 isThisMoving을 통해 확인하고 매프레임마다 직접적으로 호출
    public void Rotate(float angle)
    {

        transform.Rotate(new Vector3(0.0f, angle * structureRotateSpeed, 0.0f));

    }


    // 외부에서 이미 isThisMoving을 통해 stationary한 상태를 확인된 상태로 90도 Rotate를 1회 실행
    public void Rotate90()
    {
        // 움직이고 있는 상태로 외부 영향 받지 않게 설정
        isThisMoving = true;
        

        // 90도 회전
        startingRotation = transform.rotation;
        endRotation = Quaternion.Euler(0, 90, 0) * startingRotation;

        rotateFlag = true;
    }

    IEnumerator RotateObject(Quaternion starteRotation, Quaternion endRotation, float duration)
    {
        float endtime = Time.time + duration;

        while (Time.time <= endtime)
        {
            float t = 1f - (endtime - Time.time) / duration;
            transform.rotation = Quaternion.Lerp(starteRotation, endRotation, t);
            yield return 0;
        }

        transform.rotation = endRotation;
        isThisMoving = false;

    }




    // structuresLocationController로부터 입력받은 방향에 맞춰서 한칸 이동
    public void MoveOneTowardLocation(string desiredDirection)
    {
        //startPosition;
        //targetPosition;
        Vector3 vectorToAdd = new Vector3 (0,0,0);

        // 움직이고 있는 상태로 외부 영향 받지 않게 설정
        isThisMoving = true;

        switch (desiredDirection)
        {
            case "Left":
                vectorToAdd = new Vector3 (-3, 0, 0);
                break;
            case "Right":
                vectorToAdd = new Vector3(3, 0, 0);
                break;
            case "Up":
                vectorToAdd = new Vector3(0, 0, 3);
                break;
            case "Down":
                vectorToAdd = new Vector3(0, 0, -3);
                break;
            default:
                break;
        }

        startPosition = transform.position;

        targetPosition = startPosition + vectorToAdd;

        moveFlag = true;
    }

    IEnumerator MoveObject(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float endtime = Time.time + duration;

        while (Time.time <= endtime)
        {
            float t = 1f - (endtime - Time.time) / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return 0;
        }

        // 이동 성공
        transform.position = endPosition;
        isThisMoving = false;

    }




    public bool checkIsThisMolving()
    {
        return (isThisMoving || isThisRotating);
    }
}
