using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObject : MonoBehaviour {

    [SerializeField] private GameObject objectToEnable;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            objectToEnable.SetActive(true);
        }
    }
}
