using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x > -3 && player.transform.position.x < 50)
        {
            transform.position = new Vector3(player.position.x + 3, transform.position.y, transform.position.z);
        }
        
    }
}
