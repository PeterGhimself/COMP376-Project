using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentLevel : MonoBehaviour
{
    private GameManager gameManager;
    public Text levelText;

    void Start()
    {
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        int currentLevel = gameManager.getLevel();
        levelText.text = "Level: " + (currentLevel);
    }
}
