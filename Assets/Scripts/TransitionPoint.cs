using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPoint : MonoBehaviour {

    public TransitionType type; //The type of the transition.
    public string destinationScene; //The scene you're intending to go to. Use its name from Assets.
    public int destinationTPoint; //The transition point in the room you want the player to enter from, by index.
    public float delayOut = 1f;
    public float delayIn = 1f;
    public delegate void TransitionEvent();
    public static event TransitionEvent TransitionOut;
    public static event TransitionEvent TransitionIn;
    private bool transitioning = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null) return;

        if (!transitioning && player.Active == true) StartCoroutine(Transition(player));
    }

    private IEnumerator Transition(Player player) {
        //This stuff is insanely messy lol
        DontDestroyOnLoad(this);
        transitioning = true;

        player.Active = false;
        ControlPlayer(player);
        TransitionOut?.Invoke();
        HUDController.Fade(delayOut, delayOut / 20, Color.black, Color.clear);
        yield return new WaitForSeconds(delayOut);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(destinationScene);
        while (!asyncLoad.isDone) {
            yield return null;
        }

        SceneData sceneData = FindObjectOfType<SceneData>();
        player = sceneData.player;
        try {
            player.Active = false;
            ControlPlayer(player);
            player.transform.position = sceneData.transitions[destinationTPoint].transform.position;
        }
        catch {
            Debug.LogError("An error occurred with the SceneData in the scene being entered. Are you sure it's set up properly?");
            yield break;
        }

        TransitionIn?.Invoke();
        HUDController.Fade(delayIn, delayIn / 20, Color.clear, Color.black);
        yield return new WaitForSeconds(delayIn);
        player.Controller.InputMotion = Vector2.zero;
        player.Active = true;

        Destroy(this.gameObject);
    }

    private void ControlPlayer(Player player) {
        switch (type) {
            case TransitionType.Left:
            player.Controller.InputMotion = new Vector2(-3, 0);
            break;
            
            case TransitionType.Right:
            player.Controller.InputMotion = new Vector2(3, 0);
            break;

            default:
            player.Controller.InputMotion = Vector2.zero;
            break;
        }
    }

    public enum TransitionType {
        Left,
        Right,
        None
    }
}
