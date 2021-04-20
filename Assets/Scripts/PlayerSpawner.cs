using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
    public Player player;
    private void Start() {
        if (FindObjectOfType<Player>() == null) {
            Player clone = Instantiate(player);
            DontDestroyOnLoad(clone.gameObject);
        }

    }
}
