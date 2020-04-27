using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpawnFood : MonoBehaviour
{
    
    public static SpawnFood spawner;
    public GameObject food;
    public GameObject foodSpecial;
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;
    private int score;
    public GameObject gameOverScreen;
    public GameObject fakeBorder;
    public Text scoreText;
    public Text gameOverText;
    [HideInInspector] public bool oob;
    private Snake player;
    private bool crashed;
    private ScreenManager screenManager;
    public AudioClip error;

    private AudioSource audioSource;

    public Text messageText;




    private void Awake() {
        if (spawner == null) {
            spawner = this;
        }
        oob = false;
        score = 0;
        player = GameObject.Find("Head").GetComponent<Snake>();
        crashed = false;
        screenManager = GameObject.Find("ScreenManager").GetComponent<ScreenManager>();
        audioSource = GetComponent<AudioSource>();
        
    }
    private void Start() {
        if (!crashed)
            Invoke("Spawn", 3);
        if (!oob)
            Invoke("SpawnSpecial", 15);
    }
    public void Spawn() {
        int x = (int) Random.Range(borderLeft.position.x, borderRight.position.y);
        int y = (int) Random.Range(borderBottom.position.y, borderTop.position.y);

        Instantiate(food, new Vector2(x,y), Quaternion.identity);
    }

    private void SpawnSpecial() {
        Vector2 pos = fakeBorder.transform.position;
        pos.x -= 3;
        pos.y = 24;
        Instantiate(foodSpecial, pos, Quaternion.identity);
    }

    public void GameOver() {
        gameOverScreen.SetActive(true);
        gameOverText.text = "Ah.Okay.Score:" + score;
    }

    public void IncreaseScore(int amount) {
        score += amount;
        scoreText.text = "Score:" + score;
    }
    public void changeText(string text) {
        messageText.text = text;
    }

    public void Crash() {
        crashed = true;
        playClip(error, 10f);
        Invoke("playerStop", 10f);
        Invoke("Deactivate", 10f);

    }

    public void playClip(AudioClip clip, float delay) {
        audioSource.clip = clip;
        audioSource.PlayDelayed(delay);
    }

    private void playerStop() {
        player.Stop();
        player.vmcam.SetActive(false);
    }

    private void Deactivate() {
        player.enabled = false;
        screenManager.enabled = false;
    }
}
