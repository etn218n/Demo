using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private AudioManager audioManager;
    private LevelManager levelManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Init()
    {
        audioManager = GetComponent<AudioManager>();
        levelManager = GetComponent<LevelManager>();

        SceneManager.activeSceneChanged += levelManager.ScanGameobjects;
    }
}
