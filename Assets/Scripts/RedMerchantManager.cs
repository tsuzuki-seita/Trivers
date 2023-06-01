using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMerchantManager : MonoBehaviour
{
    private int hp = 100;
    public int swordDamage = 40;
    public int magicDamage = 40;

    public BoxCollider sword;

    public GameObject target;
    public Animator merchantAnimator;
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

        if (Vector3.Distance(transform.position, target.transform.position) < 1.5f)
        {
            merchantAnimator.SetTrigger("attack");
        }
        else
        {
            merchantAnimator.SetTrigger("walk");
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 3 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("sodhit");
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
            hp -= magicDamage;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "BlueMagic")
        {
            hp -= magicDamage * 2;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "GreenMagic")
        {
            hp -= magicDamage / 2;
            Destroy(other.gameObject);
        }

        sword.enabled = false;
    }
}
