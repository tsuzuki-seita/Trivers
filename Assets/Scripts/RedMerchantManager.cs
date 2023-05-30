using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMerchantManager : MonoBehaviour
{
    public int hp = 100;
    public int swordDamage = 70;
    public int magicDamage = 40;

    public BoxCollider sword;

    //Animation merchantAnime;
    //Rigidbody merchantRigidbody;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 0)
        {
            Debug.Log("deth");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    { 
        if (other.gameObject.tag == "RedSword")
        {
            Debug.Log("sodhit");
            hp -= swordDamage;
        }
        if (other.gameObject.tag == "BlueSword")
        {
            hp -= swordDamage * 2;
        }
        if (other.gameObject.tag == "GreenSword")
        {
            hp -= swordDamage / 2;
        }
        sword.enabled = false;
    }
}
