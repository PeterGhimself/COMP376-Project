using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FloorInfo
{
    //data to use for floor generation
    public int Room = 0;
    public int Floor = 0;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] LoadingScreen m_loadingScreen = default;
    [SerializeField] FloorManager m_floorManager = default;
    [SerializeField] int m_currentLevel = 0;
    [SerializeField] FloorInfo[] m_floorInfos = default;

    private UnityEvent m_onLevelComplete;
    private UnityEvent m_onRestartLevel;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        m_onLevelComplete = new UnityEvent();
        m_onLevelComplete.AddListener(LevelComplete);
        m_onRestartLevel = new UnityEvent();
        m_onRestartLevel.AddListener(RestartLevel);

        //Todo: send events to floor manager (or something) so they can be called when dying or finishing level
    }

    void Start()
    {
        LoadFirstLevel(); //should be moved to be called by the play button in the main menu
    }

    void OnDestroy()
    {
        m_onLevelComplete.RemoveAllListeners();
        m_onRestartLevel.RemoveAllListeners();
    }

    private IEnumerator LoadLevel()
    {
        m_loadingScreen.FadeOut();
        yield return new WaitUntil(() => !m_loadingScreen.IsFading);

        yield return null;
        print("level loading started");

        //generate floor with m_levelInfos[m_currentLevel]
        m_floorManager.InitFloor(m_floorInfos[m_currentLevel]);

        yield return new WaitForSeconds(0.25f);

        m_loadingScreen.FadeIn();
        print("level loading done");
    }

    public void LoadFirstLevel()
    {
        if (m_floorInfos.Length > 0)
            StartCoroutine(LoadLevel());
    }

    private void LevelComplete()
    {
        LoadNextLevel();
    }

    private void RestartLevel()
    {
        StartCoroutine(LoadLevel());
    }

    private void LoadNextLevel()
    {
        if (m_currentLevel < m_floorInfos.Length - 1)
        {
            m_currentLevel++;
            StartCoroutine(LoadLevel());
        }
        else
        {
            //game complete
        }
    }
}
