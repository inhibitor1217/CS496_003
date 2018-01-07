using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPlayerNearMirror : MonoBehaviour {

    PlayerController pc;
    public MirrorController mc;

    GameObject player;
    public GameObject parentStructure;

    public string structureKind = "MIRROR";
    /*
     EMPTY
     MIRROR
     */



    // Use this for initialization
    void Start()
    {
        // 플레이어 검색후 등록
        player = GameObject.FindGameObjectWithTag("Player");

        // player를 바탕으로 playerController 등록
        pc = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerStay(Collider other)
    {

        PlayerController pc = player.GetComponent<PlayerController>();


        // EMPTY가 아니고, 이전에 잡고있던 물체가 아닌 다른 물체면 무시한다.

        if (pc.getNearStructure() != null && pc.getNearStructure() != parentStructure)
        {
            return;
        }

        // 플레이어가 이 구조의 충돌 범위에 들어갔을 시 
        if (other.gameObject == player)
        {
            // player Controller의 주변 structure를 설정
            pc.setNearStructureKind(structureKind);
            pc.setNearStructure(parentStructure);

            // playerController의mirrorController를 설정
            pc.setMirrorController(mc);


            // 이동 작업 수행
            pc.setPlayerNotMovingStructure();

            // player가 무엇인가를 움직이고 있는 상태로 변경
            pc.setPlayerMovingStructure();
        }
    }

    // trigger를 나갔을시 
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            // player Controller의 주변 structure를 다시 없앰
            pc.setNearStructureKind("EMPTY");
            pc.setNearStructure(null);
            pc.setPlayerNotMovingStructure();
            pc.setMirrorController(null);
        }
    }
}
