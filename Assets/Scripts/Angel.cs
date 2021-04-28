using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : MonoBehaviour {

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }
    public void SavePlayer() {
        animator.SetTrigger("Save");
    }
}