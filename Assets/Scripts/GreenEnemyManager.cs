using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenEnemyManager : MonoBehaviour
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
        else if (other.gameObject.tag == "BlueSword")
        {
            hp -= swordDamage * 2;
        }
        else if (other.gameObject.tag == "GreenSword")
        {
            hp -= swordDamage / 2;
        }
        else if (other.gameObject.tag == "RedMagic")
        {
            hp -= magicDamage * 2;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "BlueMagic")
        {
            hp -= magicDamage / 2;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "GreenMagic")
        {
            hp -= magicDamage;
            Destroy(other.gameObject);
        }

        sword.enabled = false;
    }
}
