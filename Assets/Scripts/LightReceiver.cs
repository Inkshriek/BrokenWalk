using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightReceiver : MonoBehaviour {

    private int lights = 0;
    public int Lights { get { return lights; } private set { lights = Mathf.Max(0, value); } }
    public bool Lit { get { return lights == 0 ? false : true; } }
    public UnityEvent lit;
    public UnityEvent unlit;

    public void AddLight() {
        bool prev = Lit;
        Lights += 1;
        Debug.Log(name + " has " + Lights + " lights on it.");
        if (Lit && !prev) lit.Invoke();
    }

    public void RemoveLight() {
        bool prev = Lit;
        Lights -= 1;
        Debug.Log(name + " has " + Lights + " lights on it.");
        if (!Lit && prev) unlit.Invoke();
    }
}
