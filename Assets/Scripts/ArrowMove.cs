using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    public Rigidbody Rb;
    Vector3 moveDirection;
    GameObject target;
    //GameObject greenObject;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        //greenObject = GameObject.Find("Peasant");
        Rb = this.gameObject.GetComponent<Rigidbody>();

        //if (target.transform.position.x >= blueObject.transform.position.x)
        //{
        //    moveDirection = new Vector3(1, 0, 0);
        //}
        //else
        //{
        //    moveDirection = new Vector3(-1, 0, 0);
        //}

        moveDirection = target.transform.position - transform.position;
        moveDirection = Quaternion.Euler(0, 40, 0) * moveDirection;
        Rb.velocity = moveDirection;

        Rb.velocity *= 2;
    }

    // Update is called once per frame
    void Update()
    {

    }
}