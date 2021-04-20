using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class TransitionPoint : MonoBehaviour {

    [SerializeField] public TransitionType type; //The type of the transition.
    [SerializeField] public string destinationScene; //The scene you're intending to go to. Use its name from Assets.
    [SerializeField] public int destinationTPoint; //The transition point in the room you want the player to enter from, by index.
    private bool transitioning;

    private void Awake() {
        transitioning = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null) {
            return;
        }

        if (player.gameObject.CompareTag("Player") && !transitioning && player.Active == true) {
            StartCoroutine(Transition(player));
        }
    }

    public IEnumerator Transition(Player player) {
        DontDestroyOnLoad(this);

        player.Active = false;

        switch (type) {
            case TransitionType.Left:
                player.Controller.InputMotion = new Vector2(-3, 0);

                break;
            case TransitionType.Right:
                player.Controller.InputMotion = new Vector2(3, 0);

                break;
            default:

                break;
        }

        StartCoroutine(HUDController.Fade(1f, 0.05f, new Color(0, 0, 0, 1)));
        yield return new WaitForSeconds(1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(destinationScene);
        while (!asyncLoad.isDone) {
            yield return null;
        }

        SceneData sceneData = FindObjectOfType<SceneData>();
        try {
            player.transform.position = sceneData.transitions[destinationTPoint].transform.position;
        }
        catch {
            Debug.LogError("An error occurred trying to get the player into position at the destination. It's either missing SceneData or the player object wasn't set to DontDestroyOnLoad.");

            yield break;
        }
        StartCoroutine(HUDController.Fade(1f, 0.05f, new Color(0, 0, 0, 0)));
        yield return new WaitForSeconds(1);

        player.Controller.InputMotion = Vector2.zero;
        player.Active = true;

        Destroy(this.gameObject);
        
    }

    public enum TransitionType {
        Left,
        Right,
        None
    }
}
