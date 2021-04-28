using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour {
    public void OnTriggerEnter2D(Collider2D other) {
        Player player = other.GetComponent<Player>();
        if (player != null) { 
            player.Active = true; 
            StartCoroutine(EndGame(player));
        }

        
    }

    private IEnumerator EndGame(Player player) {
        player.Active = false;
        player.Controller.InputMotion = Vector2.zero;
        yield return new WaitForSeconds(2);
        HUDController.Fade(5, 0.05f, Color.black, Color.clear);
        yield return new WaitForSeconds(8);
        Application.Quit();
    }
}
