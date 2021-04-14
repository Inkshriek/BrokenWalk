using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private static GameController instance;
    private static SceneData sceneData;
    [SerializeField] private Player player; //The player character.
    public static bool cutsceneRunning = false;
    public static KeyCode jumpKey = KeyCode.Space;
    public static KeyCode useKey = KeyCode.E;

    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("Initializing!");

        cutsceneRunning = false;
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    private void Start() {

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        sceneData = FindObjectOfType<SceneData>();
        InitializeRoom();
    }

    public void InitializeRoom() {
        
	}

    private void LoadGame() {

    }
	
	// Update is called once per frame
	private void Update () {
		if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private IEnumerator Load() {
        /*
        for (int i = 0; i < familiarsDatabase.Length; i++) {
            GameObject obj = Instantiate(familiarsDatabase[i], player.transform.position, Quaternion.identity);
            familiars.Add(obj.GetComponent<Familiar>());
        }
        */

        yield return new WaitForSeconds(3f);
    }

    //public void StartCutscene(Cutscene scene) {
    //    cutsceneRunning = true;
    //}
}
