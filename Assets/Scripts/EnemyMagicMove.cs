using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMagicMove : MonoBehaviour
{
    public Rigidbody Rb;
    Vector3 moveDirection;
    GameObject target;
    GameObject blueObject;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        blueObject = GameObject.Find("Priest");
        Rb = this.gameObject.GetComponent<Rigidbody>();

        if (target.transform.position.x >= blueObject.transform.position.x)
        {
            moveDirection = new Vector3(1, 0, 0);
        }
        else
        {
            moveDirection = new Vector3(-1, 0, 0);
        }


        Rb.velocity = moveDirection;

        Rb.velocity *= 15;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
