using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : MonoBehaviour {

    public List<StructureController> list;

    private StructuresLocationController SLC;

    private void Start() {
        SLC = GameObject.FindGameObjectWithTag("StructureLocationController").GetComponent(typeof(StructuresLocationController)) as StructuresLocationController;
    }

    private void Update() {
        for(int i = 0; i < list.Count; i++) {
            if (!list[i].isAlive) {

                SLC.setEmpty(list[i].indexX, list[i].indexY);

                GameObject asdf = list[i].gameObject;
                list.RemoveAt(i);
                Destroy(asdf);
            }
        }
    }

}
