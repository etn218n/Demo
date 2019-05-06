using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelState { Undetermined, Won, Lost }

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject scoreBoard;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject topUI;

    private List<Bird>  birdList = new List<Bird>();
    private List<Pig>   pigList  = new List<Pig>();

    public LevelState State { get; private set; } = LevelState.Undetermined;

    private void Awake()
    {
        scoreBoard.SetActive(false);
        gameOver.SetActive(false);

        SceneManager.activeSceneChanged += ScanGameobjects;
    }

    // Scan all relevant gameObjects every time levelscene is loaded
    public void ScanGameobjects(Scene previous, Scene current)
    {
        Bird[] birdArray = GameObject.FindObjectsOfType<Bird>();
        Pig[]  pigArray  = GameObject.FindObjectsOfType<Pig>();

        foreach (var bird in birdArray)
        {
            bird.OnDisappeared += CountBirdsLeft;
            birdList.Add(bird);
        }

        foreach (var pig in pigArray)
        {
            pig.OnDisappeared += CountPigsLeft;
            pigList.Add(pig);
        }
    }

    private void CountPigsLeft(System.Object sender, System.EventArgs eventArgs)
    {
        pigList.Remove(sender as Pig);

        if (pigList.Count == 0)
        {
            StartCoroutine(DetermineLevelState("Pig"));
        }
    }

    private void CountBirdsLeft(System.Object sender, System.EventArgs eventArgs)
    {
        birdList.Remove(sender as Bird);

        if (birdList.Count == 0)
        {
            StartCoroutine(DetermineLevelState("Bird"));
        }
    }

    private IEnumerator DetermineLevelState(string gameCase)
    {
        switch (gameCase)
        {
            case "Pig":
                {
                    State = LevelState.Won;

                    yield return new WaitForSeconds(0.5f);

                    // Display winning board
                    topUI.SetActive(false);
                    scoreBoard.SetActive(true);

                    break;
                }

            case "Bird":
                {
                    // Wait few seconds to make sure pigs supposed to disappear, properly disappear
                    // to determine correct level state
                    yield return new WaitForSeconds(2f);

                    if (State == LevelState.Undetermined)
                    {
                        State = LevelState.Lost;

                        // Display gameover board
                        topUI.SetActive(false);
                        gameOver.SetActive(true);
                    }

                    break;
                }
        }

        yield return null;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= ScanGameobjects;
    }
}
