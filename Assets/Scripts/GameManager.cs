using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private bool gameOver = false;

    private Color gameOverShaderColor;
    private Color restartButtonColor;
    private Color returnButtonColor;

    private Image restartButtonImage;
    private Image returnButtonImage;

    public Text gameOverText, restartText;
    public Image shader;
    public Button restartButton, returnButton;

	void Start () {
    
        gameOverText.text = "";
        restartText.text = "";

        Color color = shader.color;
        color.a = 0.0f;
        shader.color = color;

        shader.enabled = false;
        restartButton.enabled = false;
        returnButton.enabled = false;

        restartButtonImage = restartButton.gameObject.GetComponent<Image>();
        returnButtonImage = returnButton.gameObject.GetComponent<Image>();

        color = restartButtonImage.color;
        color.a = 0.0f;
        restartButtonImage.color = color;

        color = returnButtonImage.color;
        color.a = 0.0f;
        returnButtonImage.color = color;

    }

    private void Update() {
        
        if(gameOver) {
            shader.color = Color.Lerp(shader.color, gameOverShaderColor, Time.deltaTime);
            restartButtonImage.color = Color.Lerp(restartButtonImage.color, restartButtonColor, Time.deltaTime);
            returnButtonImage.color = Color.Lerp(returnButtonImage.color, returnButtonColor, Time.deltaTime);
        }

    }

    public void GameOver(int LostPlayerIndex) {

        if (!gameOver) {
            gameOver = true;
            gameOverText.text = "Player " + (3 - LostPlayerIndex) + " Wins!";

            restartText.text = "Re?";

            shader.enabled = true;
            restartButton.enabled = true;

            Color color = shader.color;
            color.a = 0.7f;
            gameOverShaderColor = color;

            color = restartButtonImage.color;
            color.a = 1.0f;
            restartButtonColor = color;

            color = returnButtonImage.color;
            color.a = 1.0f;
            returnButtonColor = color;

        }

    }

    public void RestartGame() {

        Application.LoadLevel(Application.loadedLevel);

    }

    public void ReturnToHome() {
        Application.LoadLevel(0);
    }

}
