using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MenuBehaviour", menuName = "Menu/MenuBehaviour")]
public class MenuBehaviour : ScriptableObject
{
    public void LoadGameScene()  { SceneManager.LoadScene("Game"); }
    public void QuitGame()       { Application.Quit(); }
    public void BacktoMainmenu() { SceneManager.LoadScene("Mainmenu"); }
}
