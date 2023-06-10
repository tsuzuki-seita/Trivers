using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAuraMove : MonoBehaviour
{
    GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("AuraSpone");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position;
    }
}
