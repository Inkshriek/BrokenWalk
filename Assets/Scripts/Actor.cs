using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorMethods : MonoBehaviour {
    //This is a set of helpful methods to help with managing actor behavior. No implementations are required.

    //This here allows you to temporarily make the actor "act" something out over a duration. Handy and automatic.
    public bool Busy { get; private set; }
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
    //If you're using this though, ALWAYS call FinishAction to set Busy back to false at the end of a coroutine. That's what isn't automatic, so be sure to.


}

namespace Actors {

    public interface IActor {
        //The most basic interface meant for all actors (characters) to use.
        string DisplayName { get; }
        ActorController Controller { get; }
        bool Active { get; set; }
    }

    public interface IHealth {
        //This interface works with "health" values on a basic level.
        float Health { get; set; }
        float MaxHealth { get; set; }
        void OnDamage(float value, DamageType type);
    }

    public interface IClimb {
        //This interface assists with receiving information from nearby ledges and other climbables.
        void OnLedgeNear(bool direction, Vector2 position);
    }

    public enum NavType {
        //A set of navigation types used for defining what an actor is capable of doing.
        Normal,
        Flying,
        Familiar
    }

    public enum DamageType {
        //A set of damage types. In particular, used for determining how an actor reacts to certain forms of damage.
        Normal,
        Fire,
        Poison
    }

    public delegate IEnumerator Action();

}

