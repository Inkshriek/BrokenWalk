using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ReceiverTest : MonoBehaviour {
    
    private SpriteRenderer sprite;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Lit() {
        sprite.color = Color.yellow;
    }

    public void Unlit() { 
        sprite.color = Color.white;
    }
}
