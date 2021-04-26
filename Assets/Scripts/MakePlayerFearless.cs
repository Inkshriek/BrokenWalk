using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePlayerFearless : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D other) {
        Player player = other.GetComponent<Player>();
        if (player != null) player.Fearless = true;
    }
}