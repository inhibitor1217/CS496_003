using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public RectTransform[] Panels;

    private int idx = 0;

    public void setPanel(int newIdx) {
        idx = newIdx;
    }

    public void startGame() {
        Application.LoadLevel(1);
    }

    /*
    public void startMultiplayer() {
        Application.LoadLevel(2);
    }
    */

    public void startSinglePlayGame() {
        Application.LoadLevel(2);
    }

    private void Update() {
        
        for(int i = 0; i < Panels.Length; i++) {
            Vector3 destination = new Vector3(4000 * (i == idx ? 0 : (i > idx ? 1 : -1)), 0);
            Vector3 current = Panels[i].position;
            Panels[i].position = Vector3.MoveTowards(current, destination, 4000 * Time.deltaTime);
        }

    }

}
