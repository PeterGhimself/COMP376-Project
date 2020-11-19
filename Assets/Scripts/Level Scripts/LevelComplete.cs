using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame

    private void OnCollisionEnter2D(Collision2D other)
    {
        //for now
        if (other.gameObject.CompareTag("Player"))
        {
            _gameManager.CompleteLevel();
        }
    }
}
