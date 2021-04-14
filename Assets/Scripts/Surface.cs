using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour {
    [SerializeField] private ContactAttributes attributes;
    public ContactAttributes Attributes { get {return attributes;} set {attributes = value;} }
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
