using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magicmove : MonoBehaviour
{
    public Rigidbody Rb;
    Vector3 moveDirection;
    private GameObject playerObject;
    PlayerManager PlayerManager;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("Player");
        PlayerManager = playerObject.GetComponent<PlayerManager>();
        Rb = this.gameObject.GetComponent<Rigidbody>();

        if(PlayerManager.direction >= 0)
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

    //private void FixedUpdate()
    //{
    //    GetComponent<Rigidbody>().velocity = transform.forward * 1500 * Time.fixedDeltaTime;
    //}      //赤字の部分だけ追加しました
}
