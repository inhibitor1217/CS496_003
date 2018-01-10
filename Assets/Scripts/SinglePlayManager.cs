using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayManager : MonoBehaviour {

    public int Health;
    public int MaxHealth = 20;

    // change to private later
    public List<EnemyAttack> enemyList;

    public GameObject enemy;

    public GameObject HomeAttackedEffect;

    private WaveGenerator waveGen;

    private bool gameOver = false;

    private Color gameOverShaderColor;
    private Color restartButtonColor;
    private Color returnButtonColor;

    private Image restartButtonImage;
    private Image returnButtonImage;

    public Text gameOverText;
    public Image shader;
    public Button restartButton, returnButton;
    public Text healthText, waveText;

    private int WaveCount = 0;
    private bool generateNextWave = false;

    private void Start() {

        Health = MaxHealth;
        waveGen = GameObject.FindGameObjectWithTag("WaveGenerator").GetComponent(typeof(WaveGenerator)) as WaveGenerator;

        gameOverText.text = "";
        healthText.text = "X" + Health;
        waveText.text = "Wave " + WaveCount;

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

        if(Health <= 0) {
            GameOver();
        }
        foreach(var enemy in enemyList) {
            if (enemy != null) {
                if (enemy.getCollided()) {
                    Attacked(enemy.health, enemy.gameObject.transform.position, enemy.gameObject.transform.rotation);
                    enemy.setRemove(true);
                }
            }
        }
        
        if (gameOver) {
            shader.color = Color.Lerp(shader.color, gameOverShaderColor, Time.deltaTime);
            restartButtonImage.color = Color.Lerp(restartButtonImage.color, restartButtonColor, Time.deltaTime);
            returnButtonImage.color = Color.Lerp(returnButtonImage.color, returnButtonColor, Time.deltaTime);
        }

        healthText.text = "X" + Mathf.Max(Health, 0);

        if (!gameOver && generateNextWave && enemyList.Count == 0) {
            generateNextWave = false;
            NextWave();
        }

    }

    public void WaveGenFinished() {
        generateNextWave = true;
    }
    
    private void NextWave() {

        print("Next Wave");
        waveText.text = "Wave " + (WaveCount + 1);
        switch (WaveCount) {
            case 0:
                waveGen.Generate(enemy, 20, 2.5f, 0);
                break;
            case 1:
                waveGen.Generate(enemy, 30, 2.0f, 0);
                break;
            case 2:
                waveGen.Generate(enemy, 10, 2.7f, 1);
                waveGen.Generate(enemy, 25, 2.0f, 0);
                break;
            case 3:
                waveGen.Generate(enemy,  5, 16.0f, 2);
                waveGen.Generate(enemy, 40,  2.0f, 0);
                break;
            case 4:
                waveGen.Generate(enemy, 5, 8.0f, 3);
                break;
            case 5:
                waveGen.Generate(enemy, 30, 1.5f, 1);
                break;
            case 6:
                waveGen.Generate(enemy, 10, 4.6f, 2);
                waveGen.Generate(enemy, 30, 1.5f, 1);
                break;
            case 7:
                waveGen.Generate(enemy, 12, 4.0f, 3);
                break;
            case 8:
                waveGen.Generate(enemy, 60, 1.0f, 0);
                break;
            case 9:
                waveGen.Generate(enemy, 10, 6.0f, 4);
                waveGen.Generate(enemy, 30, 2.0f, 1);
                break;
            case 10:
                waveGen.Generate(enemy,  1, 8.0f, 5);
                waveGen.Generate(enemy, 10, 6.0f, 4);
                waveGen.Generate(enemy, 20, 3.0f, 2);
                break;
            default:
                waveGen.Generate(enemy, 10, 6.0f, 5);
                waveGen.Generate(enemy, 10, 6.0f, 4);
                waveGen.Generate(enemy, 20, 3.0f, 2);
                waveGen.Generate(enemy, 60, 1.0f, 0);
                break;
        }

        WaveCount++;

    }

    public void Attacked(int damage, Vector3 p, Quaternion r) {
        Health -= damage;
        Instantiate(HomeAttackedEffect, p, r);
    }

    private void GameOver() {

        if (!gameOver) {
            GameObject Home = GameObject.FindGameObjectWithTag("Home");

            Instantiate(HomeAttackedEffect, Home.transform.position, Home.transform.rotation);

            Destroy(Home);

            foreach (var enemy in enemyList) {
                if (enemy != null) {
                    enemy.setRemove(true);
                }
            }

            gameOverText.text = "Game Over!";

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

            gameOver = true;
        }

    }

    public void RestartGame() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ReturnToHome() {
        Application.LoadLevel(0);
    }

}
