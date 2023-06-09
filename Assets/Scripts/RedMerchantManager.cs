using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedMerchantManager : MonoBehaviour
{
    private int hp = 200;
    private int currentHp;
    public int swordDamage = 10;
    public int magicDamage = 50;

    public BoxCollider sword;

    public GameObject gameManager;

    public GameObject target;
    public Animator merchantAnimator;
    //Rigidbody merchantRigidbody;

    // PlayerSpriteの初期サイズを保存する変数
    Vector3 defaultLocalScale;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        // 初期状態でPlayerの大きさを保存
        defaultLocalScale = transform.localScale;

        slider.maxValue = hp;
        slider.value = hp;

        currentHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 0)
        {
            Debug.Log("deth");
            Destroy(this.gameObject);
            gameManager.SendMessage("EnemyBreak");
        }

        if (hp != currentHp)
        {
            merchantAnimator.SetTrigger("hurt");
            currentHp = hp;
        }

        if (transform.position.x < target.transform.position.x)
        {
            // キャラの向きをキーの押された方向に指定する
            transform.localScale = new Vector3(defaultLocalScale.x * -1, defaultLocalScale.y, defaultLocalScale.z);
        }
        else
        {
            // キャラの向きをキーの押された方向に指定する
            transform.localScale = new Vector3(defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 1)
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
            gameManager.SendMessage("AddScoreCritical");
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
            gameManager.SendMessage("AddScoreCritical");
        }
        else if (other.gameObject.tag == "GreenMagic")
        {
            hp -= magicDamage / 2;
            Destroy(other.gameObject);
        }

        sword.enabled = false;
        slider.value = hp;
    }
}
