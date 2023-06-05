using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    private int rnd;
    private int hp = 200;
    public int swordDamage = 30;
    public int magicDamage = 40;

    public BoxCollider sword;

    public GameObject player;

    public GameObject MagicAura;    //魔法弾オーラ
    public GameObject MagicBullet;    //魔法弾prefab
    [SerializeField] GameObject childObj;
    GameObject shellAura;
    GameObject shell;

    public GameObject target;
    public Animator bossAnimator;
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
        Freeze,
    };

    public enum BossState
    {
        Random,
        Idle
    };

    EnemyState state = EnemyState.Freeze;
    BossState bossstate = BossState.Idle;

    private string attri = "red";

    public PlayerManager playerManager;

    // PlayerSpriteの初期サイズを保存する変数
    Vector3 defaultLocalScale;

    // Start is called before the first frame update
    void Start()
    {
        // 初期状態でPlayerの大きさを保存
        defaultLocalScale = transform.localScale;

        bossAnimator.SetTrigger("walk");

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
            if (shellAura != null)
            {
                Destroy(shellAura);
            }

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

        if (bossstate == BossState.Idle)
        {
            rnd = Random.Range(1, 4);　// ※ 1～9の範囲でランダムな整数値が返る            
        }

        if (rnd == 1)    //プレイヤ追いかけて物理攻撃
        {
            Debug.Log("Mode1");
            bossstate = BossState.Random;

            if (state == EnemyState.Freeze)
            {
                Debug.Log("Mode：walk");
                if (playerManager.attri == "red")
                {
                    this.attri = "blue";
                }
                else if(playerManager.attri == "blue")
                {
                    this.attri = "green";
                }
                else if(playerManager.attri == "green")
                {
                    this.attri = "red";
                }
                state = EnemyState.Walk;
            }         
            if(state == EnemyState.Walk)
            {
                if (Vector3.Distance(transform.position, target.transform.position) > 2)
                {
                    bossAnimator.SetTrigger("walk");
                    transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 3 * Time.deltaTime);
                }
                else
                {
                    state = EnemyState.Attack;
                }
            }
            if(state == EnemyState.Attack)
            {              
                if(timer == 0)
                {
                    bossAnimator.SetTrigger("attack");
                }
                if (timer > 1)
                {
                    bossAnimator.SetTrigger("idle");
                    bossstate = BossState.Idle;
                    state = EnemyState.Freeze;
                    timer = 0;
                }
                timer += Time.deltaTime;
            }
        }
        else if(rnd == 2)   //瞬間移動して魔法攻撃
        {
            Debug.Log("Mode2");
            bossstate = BossState.Random;
            if (state == EnemyState.Freeze)
            {
                if (playerManager.attri == "red")
                {
                    this.attri = "blue";
                }
                else if (playerManager.attri == "blue")
                {
                    this.attri = "green";
                }
                else if (playerManager.attri == "green")
                {
                    this.attri = "red";
                }
                transform.position = new Vector3(target.transform.position.x + 15, transform.position.y,transform.position.z);
                state = EnemyState.Attack;
                bossAnimator.SetTrigger("casting");
            }
            if(state == EnemyState.Attack)
            {
                Invoke("MagicFire", 1);
                shellAura = Instantiate(MagicAura, childObj.transform.position, Quaternion.identity);

                state = EnemyState.Wait;
                bossAnimator.SetTrigger("idle");
            }
            if (state == EnemyState.Wait)
            {
                timer += Time.deltaTime;
                if (timer > 2)
                {
                    bossstate = BossState.Idle;
                    state = EnemyState.Freeze;
                    timer = 0;
                }
            }
        }
        else if (rnd == 3)  //その場で全体攻撃
        {
            Debug.Log("Mode3");
            bossstate = BossState.Random;
            if (state == EnemyState.Freeze)
            {
                if (playerManager.attri == "red")
                {
                    this.attri = "blue";
                }
                else if (playerManager.attri == "blue")
                {
                    this.attri = "green";
                }
                else if (playerManager.attri == "green")
                {
                    this.attri = "red";
                }
                transform.position = new Vector3(target.transform.position.x + 15, transform.position.y, transform.position.z);
                state = EnemyState.Attack;
            }
            if(state == EnemyState.Attack)
            {
                fullFire();
                state = EnemyState.Wait;
            }
            if (state == EnemyState.Wait)
            {
                timer += Time.deltaTime;
                if (timer > 2)
                {
                    bossstate = BossState.Idle;
                    state = EnemyState.Freeze;
                    timer = 0;
                }
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
        bossAnimator.SetTrigger("idle");
    }

    void fullFire()
    {

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
