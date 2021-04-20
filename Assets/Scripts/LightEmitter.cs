using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEmitter : MonoBehaviour {

    [SerializeField] public Vector2 originOffset;
    [SerializeField] private float lightDistance;
    [SerializeField][Range(0,359)] private int lightDirection;
    [SerializeField][Range(0,180)] private int lightRange;
    private List<LightReceiver> receiversLit;

    public float LightDistance {
        get {
            return lightDistance;
        }
        set {
            value = Mathf.Clamp(value, 0f, 100f);
            lightDistance = value;
        }
    }
    public int LightDirection {
        get {
            return lightDirection;
        }
        set {
            value = value % 360;
            lightDirection = value;
        }
    }
    public int LightRange {
        get {
            return lightRange;
        }
        set {
            value = Mathf.Clamp(value, 0, 180);
            lightRange = value;
        }
    }

    private void Awake() {
        receiversLit = new List<LightReceiver>();
    }

    private void Update() {
        Vector2 position = (Vector2)transform.position + originOffset;

        if (Physics2D.OverlapPoint(position, LayerMask.GetMask("DisturbanceZone")) == null) {
            ManageReceiversLit(position);
            FindReceivers(position);
        }
        else {
            InDisturbanceZone();
        }

    }

    private void FindReceivers(Vector2 position) {
        RaycastHit2D[] check = Physics2D.CircleCastAll(position, lightDistance, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("LightReceiver"));
        foreach (RaycastHit2D hit in check) {
            Vector2 target = hit.collider.transform.position;

            if (!CheckCriteria(position, target)) {
                continue;
            }

            LightReceiver component = hit.collider.GetComponent<LightReceiver>();
            if (component != null) {
                if (receiversLit.Count == 0) {
                    component.AddLight();
                    receiversLit.Add(component);
                }
                else {
                    //Checking to see if this receiver was already in the found list.
                    bool same = false;
                    foreach (LightReceiver receiver in receiversLit) {
                        if (receiver.gameObject == component.gameObject) {
                            same = true;
                            break;
                        }
                    }
                    if (!same) {
                        component.AddLight();
                        receiversLit.Add(component);
                    }
                }
            }
        }
    }

    private void ManageReceiversLit(Vector2 position) {
        if (receiversLit.Count == 0) return;

        for (int i = 0; i < receiversLit.Count; i++) {
            Vector2 target = receiversLit[i].transform.position;

            if (!CheckCriteria(position, target)) {
                receiversLit[i].RemoveLight();
                receiversLit.RemoveAt(i);
            }
        }
    }

    private bool CheckCriteria(Vector2 origin, Vector2 target) {

        Vector2 lightAngle = new Vector2(Mathf.Cos(Mathf.Deg2Rad * lightDirection), Mathf.Sin(Mathf.Deg2Rad * lightDirection));
        float angleDifference = Vector2.Angle(target - origin, lightAngle);
        float distance = Mathf.Abs(Vector2.Distance(origin, target));

        if (angleDifference <= (lightRange / 2) && distance <= lightDistance && !Physics2D.Linecast(origin, target, LayerMask.GetMask("Environment"))) {
            return true;
        }
        else {
            return false;
        }
    }

    private void InDisturbanceZone() {
        if (receiversLit.Count != 0) {
            foreach (LightReceiver receiver in receiversLit) receiver.RemoveLight();
            receiversLit.Clear();
        }
    }
}
