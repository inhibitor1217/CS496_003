using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

    public GameObject ParentObject;

    private void OnDestroy() {
        Destroy(ParentObject);
    }

}
