using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;


public class PlayerController : MonoBehaviour {

    public float speed = 3f;
    public float rotateSpeed = 10f;

    public int player_index;

    private float sign;

    public bool isSinglePlay = false;

    Rigidbody rb;
    Animator animator;

    // 이동 관련
    Vector3 movement;
    float horizontalMove;
    float verticalMove;

    // 버튼을 누르고 있는 상태
    bool isPressingInteractive;
    bool isPressingRotateButton;

    // interactingButton을 누르고 있는 시간
    float interactiveButtonPressedTime = 0f;


    // 근처에 있는 물체가 어떤 종류인지를 저장
    // 순수 ANIMATION 재생용
    // 외부 물체 trigger에서 setNearStructure("STRING")을 통해서 설정해줌.
    /*
    EMPTY
    MIRROR
    TURRET
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

    // 전체 좌표 정보를 들고있는 컨트롤러 참조용
    public StructuresLocationController structureLocationController;

    // 플레이어가 지금 물체를 옮기고 있는 상태인지 설정
    bool isInteractingStructure = false;
    

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

        horizontalMove = CnInputManager.GetAxisRaw("Horizontal" + player_index);
        verticalMove = CnInputManager.GetAxisRaw("Vertical" + player_index);

        AnimationUpdate();


        // check interactiveButtonPressedTiem
        calculateInteractiveButtonPressedTime();

    }

    private void FixedUpdate()
    {
        Run();
        Turn();
        MoveThings();
        RotateThings();

    }

    private void OnDestroy() {
        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent(typeof(GameManager)) as GameManager;
        gm.GameOver(player_index);
    }


    // Animation Function
    void AnimationUpdate()
    {
        // moving
        if (!isInteractingStructure)
        {
            bool move = (horizontalMove != 0.0f || verticalMove != 0.0f);
            animator.speed = move ? 2.0f : 1.0f;
            animator.SetFloat("Speed", move ? 1.0f : 0.0f);
        }
        


        // activating button
        if (isPressingInteractive)
        {

            switch (nearStructureKind)
            {
                case "EMPTY":
                    // 주위에 아무 물체가 없을때 헛손질
                    animator.Play(hashHit);
                    break;
                case "TURRET":
                    // 주위에 TURRET
                    animator.Play(hashPunch);
                    break;

                case "MIRROR":
                    // 주위에 거울
                    animator.Play(hashJump);
                    break;
                default:
                    break;
            }

        }

        if (isPressingRotateButton)
        {
            switch (nearStructureKind)
            {
                case "EMPTY":
                    animator.Play(hashHit);
                    break;
                default:
                    animator.Play(hashPick);
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
        // 무엇인가와 상호작용하고 있는 동안에는 움직일수 없게 설정
        if (!isInteractingStructure)
        {
            movement.Set(horizontalMove, 0, verticalMove);
            movement = movement.normalized * speed * Time.deltaTime;

            rb.MovePosition(transform.position + movement);
        }
    }



    public void Turn()
    {
        if (isInteractingStructure)
        {
            return;
        }
        if (isStop())
        {
            return;
        }
        Quaternion newRotation = Quaternion.LookRotation(movement);

        rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, rotateSpeed * Time.deltaTime);
    }


    public void MoveThings()
    {
        // 버튼 누르고 있는지 확인
        // 주변에 물체가 있을때만 실행
        // 할당된 미러컨트롤러가 있을때만 실행
        if ( !isPressingInteractive || nearStructure == null || mc== null)
        {
            return;
        }

        // 움직이고 있는 상태인지 확인
        if (mc.checkIsThisMolving())
        {
            return;
        }

        // 버튼을 특정시간 이상 눌렀는지 확인
        if ( interactiveButtonPressedTime < 1)
        {
            return;
        }

        /*
         vertical 범위 위쪽을 볼때 0<~ <=1
         아래쪽은 -1 <= ~ < 0

        horizontal 범위 오른쪽을 볼때 0<~ <=1
        왼쪽은 -1 <= ~ < 0

        */

        // 그 후 입력받은 방향에 따라 디렉션 설정
        string desiredDirection = "";
        string playerDirection = playerFacingDirection();


        // 물체 좌우 이동
        if (playerDirection == "Left" || playerDirection == "Right")
        {
            // 오른쪽으로 특정값 이상 조이스틱을 기울일 때
            if (horizontalMove > 0.3)
            {
                desiredDirection = "Right";
            }

            // 왼족으로 특정값 이상 조이스틱을 기울일 때
            else if (horizontalMove < -0.3)
            {
                desiredDirection = "Left";
            }
            else
            {
                // 충분한 inputAxis값이 주어지지 않았을 경우엔 함수 종료
                return;
            }

        } else if (playerDirection == "Up" || playerDirection == "Down")
        {
            // 위쪽으로 특정값 이상 조이스틱을 기울일 때
            if (verticalMove > 0.3)
            {
                desiredDirection = "Up";
            }

            // 아래쪽으로 특정값 이상 조이스틱을 기울일 때
            else if (verticalMove < -0.3)
            {
                desiredDirection = "Down";
            }
            else
            {
                // 충분한 inputAxis값이 주어지지 않았을 경우엔 함수 종료
                return;
            }
        }




        // 움직일 방향과 해당 구조물의 x,y 좌표를 넘겨줌.
        structureLocationController.moveCommand(desiredDirection, mc, mc.getIndexX(), mc.getIndexY());

    }


    public void RotateThings() {
        // 버튼 누르고 있는지 확인
        // 주변에 물체가 있을때만 실행
        // 할당된 미러컨트롤러가 있을때만 실행
        if (isPressingRotateButton == false || nearStructure == null || mc == null) {
            return;
        }

        if (!mc.checkIsThisMolving()) {
            mc.Rotate(sign * Time.deltaTime);
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

        // 애니메이션
        animator.Play(hashPunch);

        // 받아온 해당 구조물의 Rotate90을 발동시킴


        if (!mc.checkIsThisMolving())
        {
            mc.Rotate90();
        }
        
    }


    // 외부에서 버튼을 누를시 현재 캐릭터의 buttonPress 상태를 true로 설정
    public void pressInteractiveButton()
    {
        isInteractingStructure = true;
        isPressingInteractive = true;
    }

    // 외부에서 버튼을 땔 때 현재 캐릭터의 buttonPress 상태를 false로 설정
    public void unpressInteractiveButton()
    {
        isInteractingStructure = false;
        isPressingInteractive = false;

        // 버튼 누르고 있던 시간 초기화
        interactiveButtonPressedTime = 0;
    }

    // 다른 object에서 player가 button을 누르고있는지 확인할때 호출
    public bool isInteractiveButtonPressed()
    {
        return isPressingInteractive;
    }

    // 외부에서 버튼을 누를시 현재 캐릭터의 buttonPress 상태를 true로 설정
    public void pressRotateButton()
    {
        sign = 1.0f;
        isInteractingStructure = true;
        isPressingRotateButton = true;
    }

    // 외부에서 버튼을 땔 때 현재 캐릭터의 buttonPress 상태를 false로 설정
    public void unpressRotateButton()
    {
        isInteractingStructure = false;
        isPressingRotateButton = false;
    }

    public void pressNegativeRotateButton() {
        sign = -1.0f;
        isInteractingStructure = true;
        isPressingRotateButton = true;
    }

    public void unpressNegativeRotateButton() {
        isInteractingStructure = false;
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
    public bool isPlayerInteractingStructure()
    {
        return isInteractingStructure;
    }

    // 플레이어가 무언가를 옮기고 있는 상태로 설정
    public void setPlayerInteractingStructure()
    {
        isInteractingStructure = true;
    }

    // 플레이어가 무언가를 옮기고 있지 않는 상태로 설정
    public void setPlayerNotInteractingStructure()
    {
        isInteractingStructure = false;
    }

    // 미러컨트롤러 설정
    public void setMirrorController(MirrorController mirrorController)
    {
        mc = mirrorController;
    }

    // 캐릭터가 바라보고 있는 방향 return
    public string playerFacingDirection()
    {
        string retString = "";
        if (315 > transform.eulerAngles.y && transform.eulerAngles.y >= 225) retString = "Left";
        else if (315 <= transform.eulerAngles.y || transform.eulerAngles.y < 45) retString = "Up";
        else if (45 <= transform.eulerAngles.y && transform.eulerAngles.y < 135) retString = "Right";
        else retString = "Down";

        return retString;
    }

    private bool checkCharacterFacingStructure()
    {
        return true;

    }

    private void calculateInteractiveButtonPressedTime()
    {
        if (isPressingInteractive)
        {
            interactiveButtonPressedTime += Time.deltaTime;
        }
    }


}
