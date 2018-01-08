using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private bool gameOver = false;

    public Text gameOverText;
    public Text restartText;

    public Image shader;

	void Start () {
        gameOverText.text = "";
        restartText.text = "";

        Color color = shader.color;
        color.a = 0.0f;
        shader.color = color;
    }

    public void GameOver(int LostPlayerIndex) {

        if (!gameOver) {
            gameOver = true;
            gameOverText.text = "Player " + (3 - LostPlayerIndex) + " Wins!";

            Color color = shader.color;
            color.a = 0.4f;
            shader.color = color;
        }

    }

    public void RestartGame() {
        
        

    }

}
