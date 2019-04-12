using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject scoreBoard;

    private List<Bird>  birdList = new List<Bird>();
    private List<Pig>   pigList  = new List<Pig>();

    private void Awake()
    {
        scoreBoard.SetActive(false);

        SceneManager.activeSceneChanged += ScanGameobjects;
    }

    public void ScanGameobjects(Scene previous, Scene current)
    {
        Bird[] birdArray = GameObject.FindObjectsOfType<Bird>();
        Pig[]  pigArray  = GameObject.FindObjectsOfType<Pig>();

        foreach (var bird in birdArray)
        {
            bird.Disappeared += () => Debug.Log("Bird Dies");
            birdList.Add(bird);
        }

        foreach (var pig in pigArray)
        {
            pig.Disappeared += OnPigDied;
            pigList.Add(pig);
        }
    }

    private void OnPigDied(System.Object sender, System.EventArgs eventArgs)
    {
        pigList.Remove(sender as Pig);

        if (pigList.Count == 0)
        {
            StartCoroutine(DisplayScoreboard());
        }
    }

    private IEnumerator DisplayScoreboard()
    {
        yield return new WaitForSeconds(1f);

        scoreBoard.SetActive(true);
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= ScanGameobjects;
    }
}
