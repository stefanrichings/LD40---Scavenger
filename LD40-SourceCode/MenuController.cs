using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : UnitySingleton<MenuController>
{
    public static int Difficulty;

    public GameObject canvas;

    protected override void Awake ()
    {
        base.Awake();

        DontDestroyOnLoad(this);

        canvas.SetActive(false);
    }

    public void SelectDifficulty(int difficulty)
    {
        Difficulty = difficulty;
        canvas.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void LoadMenu()
    {
        canvas.SetActive(true);
        SceneManager.LoadScene(0);
    }
}
