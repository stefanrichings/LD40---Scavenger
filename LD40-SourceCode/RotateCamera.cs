using UnityEngine;
using UnityEngine.UI;

public class RotateCamera : MonoBehaviour
{
    public Button casualButton;
    public Button normalButton;
    public Button difficultButton;
    public Button hardcoreButton;

    void Start()
    {
        MenuController.Instance.canvas.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        casualButton.onClick.AddListener(() =>
        {
            MenuController.Instance.SelectDifficulty(1);
        });

        normalButton.onClick.AddListener(() =>
        {
            MenuController.Instance.SelectDifficulty(2);
        });

        difficultButton.onClick.AddListener(() =>
        {
            MenuController.Instance.SelectDifficulty(3);
        });

        hardcoreButton.onClick.AddListener(() =>
        {
            MenuController.Instance.SelectDifficulty(4);
        });
    }

    void Update()
    {
        transform.Rotate(Vector3.back * Time.deltaTime);
    }
}
