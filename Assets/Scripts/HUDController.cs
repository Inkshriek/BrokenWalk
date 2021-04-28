using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    private static HUDController instance;

    [SerializeField] public Image fade;

	void Awake() {
        instance = this;
    }

    public static void Fade(float duration, float tick, Color target, Color start) {
        try {
            instance.StartCoroutine(instance.FadeAnimation(duration, tick, target, start));
        }
        catch {
            Debug.Log("The Fade animation could not be started. Is the HUD missing from the current scene?");
        }
    }

    public IEnumerator FadeAnimation(float duration, float tick, Color target, Color start) {
        Color old;
        if (start == null) old = instance.fade.color;
        else old = start;

        float interp = 0;
        while (instance.fade.color != target) {
            interp += tick / duration;
            instance.fade.color = Color.Lerp(old, target, interp);

            yield return new WaitForSeconds(tick);
        }
    }
}
