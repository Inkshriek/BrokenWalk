using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2DController : MonoBehaviour {

    public Transform objectToFollow;
    public Vector2 minConstraints;
    public Vector2 maxConstraints;
    public Vector2 followOffset;
    public bool following = true;
    public bool constrained = true;
    public bool smooth = true;
    public float smoothTime = 0.3f;
    private Vector2 velocity = Vector2.zero;

    private void Start() {
        transform.position = new Vector3(objectToFollow.transform.position.x,objectToFollow.transform.position.y,transform.position.z);
    }

	private void Update () {
        Vector3 position = transform.position;
        Vector3 target = objectToFollow.transform.position + (Vector3)followOffset;

        if (following && objectToFollow != null) {
            if (smooth) {
                float tempz = position.z;
                position = Vector2.SmoothDamp(position, target, ref velocity, smoothTime);
                position = new Vector3(position.x, position.y, tempz);
            }
            else position = new Vector3(target.x, target.y, position.z);
        }
		if (constrained) {
            position = new Vector3(
                Mathf.Clamp(position.x, minConstraints.x, maxConstraints.x), 
                Mathf.Clamp(position.y, minConstraints.y, maxConstraints.y), 
                position.z
            );
        }

        transform.position = position;
	}


}
