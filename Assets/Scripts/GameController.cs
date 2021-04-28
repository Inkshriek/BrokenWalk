using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private static GameController instance;
    [SerializeField] private Player player;
    [SerializeField] private Angel angel;
    [SerializeField] private Camera2DController cam;
    
    private void Awake() {
        instance = this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    public static void GameOver() {
        try {
            instance.StartCoroutine(instance.RestartSequence());
        }
        catch {
            Debug.Log("The game cannot be Game Overed. There is no GameController in the scene.");
        }
    }

    public IEnumerator RestartSequence() {
        Angel angelinstance = Instantiate(angel, new Vector3(cam.transform.position.x, cam.transform.position.y, -1), Quaternion.identity);
        angelinstance.SavePlayer();
        yield return new WaitForSeconds(1);
        HUDController.Fade(0.5f, 0.05f, Color.white, Color.clear);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
