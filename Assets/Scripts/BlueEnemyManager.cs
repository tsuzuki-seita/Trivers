using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueEnemyManager : MonoBehaviour
{
    private int hp = 100;
    private int currentHp;
    public int swordDamage = 50;
    public int magicDamage = 50;

    public BoxCollider sword;

    public GameObject player;
    public GameObject gameManager;

    public GameObject magicAura;    //魔法弾オーラ
    public GameObject magicBullet;    //魔法弾prefab
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

        currentHp = hp;

        magicBullet.tag = "Magic";
        magicBullet.gameObject.GetComponent<SpriteRenderer>().color = new Color32(250, 250, 250, 255);
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
            gameManager.SendMessage("EnemyBreak");
        }

        if (hp != currentHp)
        {
            blueAnimator.SetTrigger("hurt");
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
            shellAura = Instantiate(magicAura, childObj.transform.position, Quaternion.identity);
            
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
            shell = Instantiate(magicBullet, childObj.transform.position, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            shell = Instantiate(magicBullet, childObj.transform.position, Quaternion.Euler(0, 180f, 0));
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
            gameManager.SendMessage("AddScoreCritical");
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
            gameManager.SendMessage("AddScoreCritical");
        }

        sword.enabled = false;
        slider.value = hp;
    }
}
