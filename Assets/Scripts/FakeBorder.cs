using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeBorder : MonoBehaviour
{
    private Queue<string> message;

    private Coroutine messageCoroutine;

    private Snake player;

    public AudioClip eerieSound;

    private bool done;

    public Text scoreText;

    
    void Start()
    {
        message = new Queue<string>();
        TextAsset messText = Resources.Load("message") as TextAsset;
        ParseText(messText.text);
        done = false;
    }

    private void ParseText(string toParse) {
        string[] separators = {"\r\n"};
        string[] lines = toParse.Split(separators, System.StringSplitOptions.None);
        
        for (int i = 0; i < lines.Length; i++)
            message.Enqueue(lines[i]);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        player = other.GetComponent<Snake>();
        if (player != null) {
            SpawnFood.spawner.oob = true;
            StartCoroutine(Message(message, 6f));
            SpawnFood.spawner.playClip(eerieSound, 6f);
        }    
    }

    private void Update() {
        if (done) {
            SpawnFood.spawner.Crash();
            done = false;
        }
    }

    private IEnumerator Message(Queue<string> text, float delayTime) {
        yield return new WaitForSeconds(delayTime);
        scoreText.text = "";
        if (text.Count > 0) {
            SpawnFood.spawner.changeText(text.Dequeue());
            yield return null;        
            StartCoroutine(Message(text, delayTime));
        } else {
            SpawnFood.spawner.changeText("");
            yield return null;
            done = true;
        }    
    }
}
