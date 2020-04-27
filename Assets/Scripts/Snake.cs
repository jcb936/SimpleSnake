using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Snake : MonoBehaviour
{
    // Start is called before the first frame update
    
    private Vector2 dir;
    private List<Transform> tail = new List<Transform>();
    public GameObject tailPrefab;
    private bool ate = false;
    public int foodScore = 10;

    public float snakeSpeed = 10f;
    public GameObject vmcam;
    private AudioSource audioPlayer;

    public AudioClip eatSound;

    public bool stopMoving;
    
    void Start()
    {
        dir = Random.Range(-1f, 1f) > 0 ? Vector2.right : Vector2.left;
        audioPlayer = GetComponent<AudioSource>();
        InvokeRepeating("Move", 1/snakeSpeed, 1/snakeSpeed);
        stopMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKey(KeyCode.RightArrow))
            dir = Vector2.right;
        else if (Input.GetKey(KeyCode.DownArrow))
            dir = -Vector2.up;    // '-up' means 'down'
        else if (Input.GetKey(KeyCode.LeftArrow))
            dir = -Vector2.right; // '-right' means 'left'
        else if (Input.GetKey(KeyCode.UpArrow))
            dir = Vector2.up;
    }

    public void Stop() {
        stopMoving = true;
    }

    private void Move() {
        if (!stopMoving) {
            Vector2 v = transform.position;
            transform.Translate(dir);
            if (ate) {
                GameObject g = Instantiate(tailPrefab, v, Quaternion.identity);
                tail.Insert(0, g.transform);
                ate = false;
            }
            if (tail.Count > 0) {
                tail.Last().position = v;
                tail.Insert(0, tail.Last());
                tail.RemoveAt(tail.Count-1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name.StartsWith("FoodSpecial")) {
            audioPlayer.PlayOneShot(eatSound);
            ate = true;
            Destroy(other.gameObject);
            SpawnFood.spawner.IncreaseScore(foodScore);
            
            
            

        } else if (other.name.StartsWith("FakeBorder")) {
            vmcam.SetActive(true);
        } else if (other.name.StartsWith("Food")) {
            audioPlayer.PlayOneShot(eatSound);
            ate = true;
            Destroy(other.gameObject);
            SpawnFood.spawner.Spawn();
            if (!SpawnFood.spawner.oob)
                SpawnFood.spawner.IncreaseScore(foodScore);
        } else {
            SpawnFood.spawner.GameOver();
            Destroy(gameObject);
        }
    }

    //public void playClip(AudioClip clip, float delay) {
    //    audioPlayer.clip = clip;
    //    audioPlayer.PlayDelayed(delay);
    //}
}
