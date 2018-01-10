using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager_NW : MonoBehaviour {

    private bool gameOver = false;

    private Color gameOverShaderColor;
    private Color returnButtonColor;
    
    private Image returnButtonImage;

    public Text gameOverText;
    public Image shader;
    public Button returnButton;

	void Start () {
    
        gameOverText.text = "";

        Color color = shader.color;
        color.a = 0.0f;
        shader.color = color;

        shader.enabled = false;
        returnButton.enabled = false;
        
        returnButtonImage = returnButton.gameObject.GetComponent<Image>();

        color = returnButtonImage.color;
        color.a = 0.0f;
        returnButtonImage.color = color;

    }

    private void Update() {
        
        if(gameOver) {
            shader.color = Color.Lerp(shader.color, gameOverShaderColor, Time.deltaTime);
            returnButtonImage.color = Color.Lerp(returnButtonImage.color, returnButtonColor, Time.deltaTime);
        }

    }

    public void GameOver() {

        if (!gameOver) {
            gameOver = true;
            // gameOverText.text = "Game Ends"
            

            shader.enabled = true;

            Color color = shader.color;
            color.a = 0.7f;
            gameOverShaderColor = color;

            color = returnButtonImage.color;
            color.a = 1.0f;
            returnButtonColor = color;

        }

    }

    public void ReturnToHome() {

        NetworkManager NWManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent(typeof(NetworkManager)) as NetworkManager;

        NWManager.StopServer();
        NWManager.StopClient();

        Application.LoadLevel(0);

    }

}
