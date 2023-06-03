using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueEnemyManager : MonoBehaviour
{
    private int hp = 100;
    public int swordDamage = 40;
    public int magicDamage = 40;

    public BoxCollider sword;

    public GameObject player;

    public GameObject MagicAura;    //魔法弾オーラ
    public GameObject MagicBullet;    //魔法弾prefab
    [SerializeField] GameObject childObj;
    GameObject shellAura;
    GameObject shell;

    public GameObject target;
    public Animator blueAnimator;
    //Animation merchantAnime;
    //Rigidbody merchantRigidbody;

    private float timer = 0;

    public Slider slider;

    public enum EnemyState
    {
        Walk,
        Wait,
        Chase,
        Attack,
        Freeze
    };
    EnemyState state;

    // PlayerSpriteの初期サイズを保存する変数
    Vector3 defaultLocalScale;

    // Start is called before the first frame update
    void Start()
    {
        // 初期状態でPlayerの大きさを保存
        defaultLocalScale = transform.localScale;

        state = EnemyState.Chase;
        blueAnimator.SetTrigger("walk");

        slider.maxValue = hp;
        slider.value = hp;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (hp < 0)
        {
            Debug.Log("deth");
            Destroy(this.gameObject);
            if(shellAura != null)
            {
                Destroy(shellAura);
            }
            
        }

        if(transform.position.x < target.transform.position.x)
        {
            // キャラの向きをキーの押された方向に指定する
            transform.localScale = new Vector3(defaultLocalScale.x * -1, defaultLocalScale.y, defaultLocalScale.z);
        }
        else
        {
            // キャラの向きをキーの押された方向に指定する
            transform.localScale = new Vector3(defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);
        }

        if (state == EnemyState.Chase)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > 15f)
            {
                
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 3 * Time.deltaTime);
            }
            else
            {
                state = EnemyState.Attack;
                blueAnimator.SetTrigger("casting");
            }    
        }
        if(state == EnemyState.Attack)
        {
            Invoke("MagicFire", 1);
            shellAura = Instantiate(MagicAura, childObj.transform.position, Quaternion.identity);
            
            state = EnemyState.Wait;
            timer = 0;
            blueAnimator.SetTrigger("idle");
        }
        if(state == EnemyState.Wait)
        {
            
            timer += Time.deltaTime;
            if (timer > 2)
            {
                state = EnemyState.Walk;
                blueAnimator.SetTrigger("walk");
            }
        }
        if(state == EnemyState.Walk)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > 15f)
            {

                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 3 * Time.deltaTime);
            }
            else
            {
                state = EnemyState.Attack;
                blueAnimator.SetTrigger("casting");
            }
        }
    }

    void MagicFire()
    {
        Destroy(shellAura);
        if (player.transform.position.x <= transform.position.x)
        {
            shell = Instantiate(MagicBullet, childObj.transform.position, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            shell = Instantiate(MagicBullet, childObj.transform.position, Quaternion.Euler(0, 180f, 0));
        }
        blueAnimator.SetTrigger("idle");
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("sodhit");
        if (collision.gameObject.tag == "RedSword")
        {
            Debug.Log("damage");
            hp -= swordDamage / 2;
        }
        else if (collision.gameObject.tag == "BlueSword")
        {
            hp -= swordDamage;
        }
        else if (collision.gameObject.tag == "GreenSword")
        {
            hp -= swordDamage * 2;
        }
        else if (collision.gameObject.tag == "RedMagic")
        {
            hp -= magicDamage / 2;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "BlueMagic")
        {
            hp -= magicDamage;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "GreenMagic")
        {
            hp -= magicDamage * 2;
            Destroy(collision.gameObject);
        }

        sword.enabled = false;
        slider.value = hp;
    }
}
