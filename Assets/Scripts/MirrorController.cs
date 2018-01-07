using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorController : MonoBehaviour {

    public float structureRotateDuration = 0.5f;
    public float structureRotateSpeed = 200;

    // 다른 오브젝트에 의해 상호작용을 받고 있는 상태인지 표시하는 변수
    private bool isThisMoving = false;
    private bool rotateFlag = false;

    Quaternion startingRotation;
    Quaternion endRotation;

    

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (rotateFlag == true)
        {
            StartCoroutine(RotateObject(startingRotation, endRotation,structureRotateDuration));
            rotateFlag = false;
        }
    }



    // 외부에서 isThisMoving을 통해 확인하고 매프레임마다 직접적으로 호출
    public void Rotate(float angle)
    {
        /*
        Quaternion targetRotation = Quaternion.
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, structureRotateSpeed * Time.deltaTime);
        */

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



    public bool checkIsThisMolving()
    {
        return isThisMoving;
    }
}
