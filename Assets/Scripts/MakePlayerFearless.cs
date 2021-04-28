using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePlayerFearless : MonoBehaviour {

    [SerializeField] private GameObject platformToBreak;

    public void OnTriggerEnter2D(Collider2D other) {
        Player player = other.GetComponent<Player>();
        if (player != null) {
            player.Fearless = true;
            Destroy(platformToBreak);
            StartCoroutine(Fearless(player));
        }
    }

    private IEnumerator Fearless(Player player) {
        player.Active = false;
        player.Controller.InputMotion = Vector2.zero;
        HUDController.Fade(0.1f, 0.1f, Color.black, Color.clear);
        yield return new WaitForSeconds(5);
        HUDController.Fade(3, 0.05f, Color.clear, Color.black);
        yield return new WaitForSeconds(5);
        player.Relieve();
        player.Active = true;
    }
}