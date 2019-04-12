using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private Bird[] birds;
    private Pig[]  pigs;

    public void ScanGameobjects(Scene previous, Scene current)
    {
        birds = GameObject.FindObjectsOfType<Bird>();
        pigs  = GameObject.FindObjectsOfType<Pig>();

        foreach (var bird in birds)
        {
            bird.Disappeared += () => Debug.Log("Bird Dies");
        }

        foreach (var pig in pigs)
        {
            pig.Disappeared += () => Debug.Log("Pig Dies");
        }
    }
}
