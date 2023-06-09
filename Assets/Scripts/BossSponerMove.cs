using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSponerMove : MonoBehaviour
{
    public GameObject target;
    bool awake = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(awake == false)
        {
            transform.position = new Vector3(target.transform.position.x + 5, 3, transform.position.z);
        }

    }

    public void spone()
    {
        awake = true;
    }
}
