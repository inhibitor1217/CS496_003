using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;


public class PlayerController : MonoBehaviour {

    public float speed = 3f;
    public float rotateSpeed = 10f;


    Rigidbody rb;
    Animator animator;

    // 이동 관련
    Vector3 movement;
    float horizontalMove;
    float verticalMove;

    // 버튼을 누르고 있는 상태
    bool isPressingInteractive;
    bool isPressingRotateButton;


    // 근처에 있는 물체가 어떤 종류인지를 저장
    // 순수 ANIMATION 재생용
    // 외부 물체 trigger에서 setNearStructure("STRING")을 통해서 설정해줌.
    /*
    EMPTY
    MIRROR
    CANNON
    */
    string nearStructureKind = "EMPTY";


    // animation 관련
    private int hashHit = Animator.StringToHash("Base Layer.Hit");
    private int hashDead = Animator.StringToHash("Base Layer.Dead");
    private int hashWalk = Animator.StringToHash("Base Layer.Walk");
    private int hashJump = Animator.StringToHash("Base Layer.Jump");
    private int hashPick = Animator.StringToHash("Base Layer.Pick");
    private int hashPunch = Animator.StringToHash("Base Layer.Punch");


    // 근처에 있는 물체를 GameObject로 저장
    // 다량 물체 중첩 충돌 검사용 및 움직이고 있는 물체 겸용.
    GameObject nearStructure;
    
    // 주변에 있는 거울의 미러컨트롤러 참조용
    MirrorController mc;

    // 플레이어가 지금 물체를 옮기고 있는 상태인지 설정
    bool isMovingStructure = false;
    




    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
	}

	
	// Update is called once per frame
	void Update () {

        /*
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        */
        horizontalMove = CnInputManager.GetAxisRaw("Horizontal");
        verticalMove = CnInputManager.GetAxisRaw("Vertical");

        AnimationUpdate();
        
    }

    private void FixedUpdate()
    {
        Run();
        Turn();
        //MoveThings();
        RotateThings();
    }


    // Animation Function
    void AnimationUpdate()
    {
        // moving
        bool move = (horizontalMove != 0.0f || verticalMove != 0.0f);
        animator.speed = move ? 2.0f : 1.0f;
        animator.SetFloat("Speed", move ? 1.0f : 0.0f);


        // activating button
        if (isPressingInteractive)
        {

            switch (nearStructureKind)
            {
                case "EMPTY":
                    // 주위에 아무 물체가 없을때 헛손질
                    animator.Play(hashHit);
                    break;
                case "CANNON":
                    // 주위에 CANNON
                    animator.Play(hashPunch);
                    break;

                case "MIRROR":
                    // 주위에 거울
                    animator.Play(hashJump);
                    break;
            }

        }

    }

    private bool isStop()
    {
        return (horizontalMove == 0 && verticalMove == 0);
    }


    // Active Function

    public void Run()
    {
        movement.Set(horizontalMove, 0, verticalMove);
        movement = movement.normalized * speed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);
    }



    public void Turn()
    {

        if (isStop())
        {
            return;
        }
        Quaternion newRotation = Quaternion.LookRotation(movement);

        rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, rotateSpeed * Time.deltaTime);
    }


    /*
    public void MoveThings()
    {
        // 무엇인가를 움직이고 있는 상태인것을 구조물로부터 받고, 그 구조물에 대한 reference가 존재할때만 실행
        if ( !isMovingStructure || nearStructure == null)
        {
            return;
        }

        if (!isPressingInteractive)
        {
            nearStructure.transform.parent = null;
        }


        nearStructure.transform.parent = transform;

        // 잡은 물체도 같이 이동
        Rigidbody PullObjectRigidBody = nearStructure.GetComponent<Rigidbody>();
        PullObjectRigidBody.MovePosition(transform.position + movement);


    }
*/

    public void RotateThings()
    {
        // 버튼 누르고 있는지 확인
        // 주변에 물체가 있을때만 실행
        // 할당된 미러컨트롤러가 있을때만 실행
        if (isPressingRotateButton == false || nearStructure == null || mc== null)
        {
            return;
        }

        if (!mc.checkIsThisMolving())
        {
            mc.Rotate(Time.deltaTime);
        }

    }

    public void RotateThings90()
    {
        // 주변에 물체가 있을때만 실행
        // 할당된 미러컨트롤러가 있을때만 실행
        if ( nearStructure == null || mc == null)
        {
            return;
        }


        // 받아온 해당 구조물의 Rotate90을 발동시킴


        if (!mc.checkIsThisMolving())
        {
            mc.Rotate90();
        }
        
    }


    // 외부에서 버튼을 누를시 현재 캐릭터의 buttonPress 상태를 true로 설정
    public void pressInteractiveButton()
    {
        isPressingInteractive = true;
    }

    // 외부에서 버튼을 땔 때 현재 캐릭터의 buttonPress 상태를 false로 설정
    public void unpressInteractiveButton()
    {
        isPressingInteractive = false;
    }

    // 다른 object에서 player가 button을 누르고있는지 확인할때 호출
    public bool isInteractiveButtonPressed()
    {
        return isPressingInteractive;
    }

    // 외부에서 버튼을 누를시 현재 캐릭터의 buttonPress 상태를 true로 설정
    public void pressRotateButton()
    {
        isPressingRotateButton = true;
    }

    // 외부에서 버튼을 땔 때 현재 캐릭터의 buttonPress 상태를 false로 설정
    public void unpressRotateButton()
    {
        isPressingRotateButton = false;
    }

    // 다른 object에서 player가 button을 누르고있는지 확인할때 호출
    public bool isRotateButtonPressed()
    {
        return isPressingRotateButton;
    }



    // 다른 object에서 player 주변의 structure의 종류를 설정
    public void setNearStructureKind(string inputStructure)
    {
        nearStructureKind = inputStructure;
    }

    // 다른 object에서 player 주변에 있는 structure의 종류를 반환
    public string getNearStructureKind()
    {
        return nearStructureKind;
    }

    // 다른 object에서 player 주변의 structure를 설정
    public void setNearStructure(GameObject gameObject)
    {
        nearStructure = gameObject;
    }

    // 다른 object에서 player 주변에 있는 structure를 반환
    public GameObject getNearStructure()
    {
        return nearStructure;
    }


    // 플레이어가 무언가를 옮기고 있는 상태인지 반환값 리턴
    public bool isPlayerMovingStructure()
    {
        return isMovingStructure;
    }

    // 플레이어가 무언가를 옮기고 있는 상태로 설정
    public void setPlayerMovingStructure()
    {
        isMovingStructure = true;
    }

    // 플레이어가 무언가를 옮기고 있지 않는 상태로 설정
    public void setPlayerNotMovingStructure()
    {
        isMovingStructure = false;
    }

    // 미러컨트롤러 설정
    public void setMirrorController(MirrorController mirrorController)
    {
        mc = mirrorController;
    }



}
