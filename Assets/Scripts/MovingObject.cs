using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {
    public Vector2[] positions;
    [Min(0.1f)] public float animationTime = 0.5f;
    public bool active = true;
    private Vector3 origin;
    private int i = 0;
    private Vector2 velocity = Vector2.zero;
    private float timer;
    private void Awake() {
        origin = transform.position;
        timer = animationTime * 2;
    }

    private void FixedUpdate() {
        if (positions.Length < 2 || !active) return;

        Vector3 position = transform.position;
        Vector2 target = (Vector2)origin + positions[i];

        float tempz = position.z;
        position = Vector2.SmoothDamp(position, target, ref velocity, animationTime);
        position = new Vector3(position.x, position.y, tempz);

        timer -= Time.deltaTime;
        if (timer <= 0) {
            i = i < positions.Length - 1 ? i + 1 : 0;
            timer = animationTime * 2;
        }

        transform.position = position;
    }
}
