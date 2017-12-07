using System.Collections;
using TMPro;
using UnityEngine;

public class UIController : UnitySingleton<UIController>
{
    public HoldMessage holdMessage;
    public GameObject carryingCore;
    public GameObject remainingMessage;
    public GameObject endGameScreen;
    public TextMeshProUGUI oxygenLabel;

    public vp_FPPlayerEventHandler player;
    bool returnToMenu;

    protected override void Awake ()
    {
        base.Awake();

        returnToMenu = false;
        holdMessage.Hide();
        carryingCore.SetActive(false);
        remainingMessage.SetActive(false);
        endGameScreen.SetActive(false);
        GameController.OnCoreChange += (bool val) => { carryingCore.SetActive(val); };
    }

    void OnEnable ()
    {
        if (player != null)
        {
            player.Register(this);
        }
    }

    void OnDisable ()
    {
        if (player != null)
        {
            player.Unregister(this);
        }
    }

    void Update()
    {
        if (returnToMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ReturnToMenu();
            }
        }
    }

    public void ShowHoldMessage(string msg, string k)
    {
        holdMessage.Show(msg, k);
    }

    public void HideHoldMessage()
    {
        holdMessage.Hide();
    }

    public void ShowRemainingMessage()
    {
        remainingMessage.SetActive(true);
        remainingMessage.GetComponentInChildren<TextMeshProUGUI>().text = GameController.Instance.NumberOfCoresRemaining + " cores remaining";
        StartCoroutine(HideRemainingMessage());
    }

    public void ShowEndGameScreen(bool win)
    {
        endGameScreen.SetActive(true);
        TextMeshProUGUI t = endGameScreen.GetComponentInChildren<TextMeshProUGUI>();
        if (win)
        {
            t.text = "Salvage complete - all cores recovered";
        }
        else
        {
            t.text = "You died - now you're space spider food";
        }
        returnToMenu = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReturnToMenu()
    {
        MenuController.Instance.LoadMenu();
    }

    void OnStart_Dead ()
    {
        ShowEndGameScreen(false);
    }

    public void UpdateOxygenLabel(float level)
    {
        oxygenLabel.text = "O2 Efficiency: " + level.ToString("F2") + "%";
    }

    public void HideOxygenLabel()
    {
        oxygenLabel.transform.parent.gameObject.SetActive(false);
    }

    IEnumerator HideRemainingMessage()
    {
        yield return new WaitForSeconds(2f);
        remainingMessage.SetActive(false);
    }

    public string CurrentMessage
    {
        get
        {
            return holdMessage.Message;
        }
    }
}
