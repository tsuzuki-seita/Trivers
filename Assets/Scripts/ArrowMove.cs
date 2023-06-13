using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    public Rigidbody Rb;
    Vector3 moveDirection;
    Vector3 mato;
    GameObject target;
    Vector3 angle;

    // Start is called before the first frame update
    void Start()
    {
        Transform arrowTransform = this.transform;
        target = GameObject.Find("Player");
        Rb = this.gameObject.GetComponent<Rigidbody>();

        mato = target.transform.position + new Vector3(0, 3, 0);    //????????????
        moveDirection = mato - transform.position;

        angle = this.transform.localEulerAngles;

        if (target.transform.position.x <= transform.position.x)
        {
            angle.z = 90;
        }
        else
        {
            angle.z = -90;
        }
        this.transform.localEulerAngles = angle;
        Rb.velocity = moveDirection;

        Rb.velocity *= 2;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
