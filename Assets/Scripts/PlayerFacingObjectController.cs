using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacingObjectController : MonoBehaviour {

    public PlayerController pc;

    TurretController tc;
    MirrorController mc;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerStay(Collider other)
    {

        // 플레이어가 바라보고 있는 방향의 물체가 Structure일 경우
        if (other.gameObject.tag == "Mirror")
        {

            // 해당 거울의 컨트롤러를 얻어옴
            mc = other.gameObject.GetComponent<MirrorController>();


            // player Controller의 주변 structure를 설정
            pc.setNearStructureKind("MIRROR");
            pc.setNearStructure(other.gameObject);

            // playerController의mirrorController를 설정
            pc.setMirrorController(mc);
        }
        else if (other.gameObject.tag == "Turret")
        {
            // 해당 터렛의 컨트롤러를 얻어옴
            tc = other.gameObject.GetComponentInChildren<TurretController>();

            // player Controller의 주변 structure를 설정
            pc.setNearStructureKind("TURRET");
            pc.setNearStructure(other.gameObject);


            // 발사 작업 수행
            if (pc.isInteractiveButtonPressed())
            {
                tc.Fire();
            }

        }
    }

    // trigger를 나갔을시 
    private void OnTriggerExit(Collider other)
    {
        pc.setNearStructureKind("EMPTY");
        pc.setNearStructure(null);
        pc.setMirrorController(null);
        mc = null;
    }
}
