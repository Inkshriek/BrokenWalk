using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Contact Attributes", menuName = "ScriptableObjects/ContactAttributes", order = 2)]
public class ContactAttributes : ScriptableObject {
    [Range(0, 1)] public float friction;
    [Range(0, 1)] public float bounce;

}
