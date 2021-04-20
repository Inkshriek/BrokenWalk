using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour {
    public string DisplayName { get; set; }
    public ActorController Controller { get; protected set; }
    public bool Active { get; set; }
    public bool Busy { get; protected set; }
    private Coroutine coroutine;
    public void StartAction(Actors.Action method) {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        Busy = true;
        coroutine = StartCoroutine(method());
    }
    public void FinishAction() {
        StopCoroutine(coroutine);
        Busy = false;
    }


}

namespace Actors {

    public delegate IEnumerator Action();

}

