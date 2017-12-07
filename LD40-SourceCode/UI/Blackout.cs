using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Blackout : UnitySingleton<Blackout>
{
    Image image;
    bool blinking;

    void Start()
    {
        image = GetComponent<Image>();
        blinking = false;

        image.CrossFadeAlpha(0, 0.5f, false);
    }

    public void Blink()
    {
        if (blinking) return;

        blinking = true;
        StartCoroutine(BlackoutBlink(OxygenController.Instance.oxygenLevels / 2f));
    }

    IEnumerator BlackoutBlink(float time)
    {
        yield return new WaitForSeconds(time);

        image.CrossFadeAlpha(1, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
        image.CrossFadeAlpha(0, 0.5f, false);

        StartCoroutine(BlackoutBlink(OxygenController.Instance.oxygenLevels));
    }
}
