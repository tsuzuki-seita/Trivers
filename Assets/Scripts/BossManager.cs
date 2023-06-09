using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    private int rnd;
    private int hp = 200;
    private int currentHp;
    public int swordDamage = 30;
    public int magicDamage = 40;

    public GameObject gameManager;

    public BoxCollider sword;
    public BoxCollider bossSword;

    public GameObject magicAura;    //魔法弾オーラ
    public GameObject magicBullet;    //魔法弾prefab
    [SerializeField] GameObject childObj;
    GameObject shellAura;
    GameObject shell;

    public GameObject target;
    public Animator bossAnimator;
    //Animation merchantAnime;
    //Rigidbody merchantRigidbody;

    private float timer = 0;

    public Slider slider;
    public Image sliderImage;

    public string awake = "stay";

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
        Awake,
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

        slider.maxValue = hp;
        slider.value = hp;

        currentHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if(awake == "awake")
        {
            Debug.Log("Awake");
            if (hp < 0)
            {
                Debug.Log("deth");
                gameManager.SendMessage("AddScoreClear");
                Destroy(this.gameObject);
                if (shellAura != null)
                {
                    Destroy(shellAura);
                }

            }

            if (hp != currentHp)
            {
                bossAnimator.SetTrigger("hurt");
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

            if (bossstate == BossState.Idle)
            {
                rnd = Random.Range(1, 4); // ※ 1～9の範囲でランダムな整数値が返る            
            }

            if (rnd == 1)    //プレイヤ追いかけて物理攻撃
            {
                Debug.Log("Mode1");
                bossstate = BossState.Random;

                if (state == EnemyState.Freeze)
                {
                    Debug.Log("Mode：walk");
                    colorChange();
                    state = EnemyState.Walk;
                }
                if (state == EnemyState.Walk)
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
                if (state == EnemyState.Attack)
                {
                    if (timer == 0)
                    {
                        if (attri == "red")
                        {
                            bossSword.tag = "BossRedSword";
                        }
                        else if (attri == "blue")
                        {
                            bossSword.tag = "BossBlueSword";
                        }
                        else if (attri == "green")
                        {
                            bossSword.tag = "BossGreenSword";
                        }
                        bossSword.enabled = true;
                        bossAnimator.SetTrigger("attack");
                    }
                    if (timer > 1.2f)
                    {
                        bossAnimator.SetTrigger("idle");
                        bossstate = BossState.Idle;
                        state = EnemyState.Freeze;
                        timer = 0;
                    }
                    timer += Time.deltaTime;
                }
            }
            else if (rnd == 2)   //瞬間移動して魔法攻撃
            {
                Debug.Log("Mode2");
                bossstate = BossState.Random;
                if (state == EnemyState.Freeze)
                {
                    colorChange();
                    if (target.transform.position.x < 20)
                    {
                        transform.position = new Vector3(target.transform.position.x + 10, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        transform.position = new Vector3(target.transform.position.x - 10, transform.position.y, transform.position.z);
                    }
                    state = EnemyState.Attack;
                    bossAnimator.SetTrigger("casting");
                }
                if (state == EnemyState.Attack)
                {
                    Invoke("MagicFire", 1);
                    shellAura = Instantiate(magicAura, childObj.transform.position, Quaternion.identity);

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
                    colorChange();
                    state = EnemyState.Attack;
                }
                if (state == EnemyState.Attack)
                {
                    bossAnimator.SetTrigger("full");
                    fullFire();
                    state = EnemyState.Wait;
                }
                if (state == EnemyState.Wait)
                {
                    timer += Time.deltaTime;
                    if (timer > 2)
                    {
                        bossAnimator.SetTrigger("idle");
                        bossstate = BossState.Idle;
                        state = EnemyState.Freeze;
                        timer = 0;
                    }
                }
            }
        }
        else if(awake == "stay")
        {
            transform.position = new Vector3(target.transform.position.x - 6, 3, transform.position.z);
        }
        else
        {

        }
    }

    void MagicFire()
    {
        Destroy(shellAura);
        if (attri == "red")
        {
            magicBullet.tag = "Leg";
            magicBullet.gameObject.GetComponent<SpriteRenderer>().color = new Color32(248, 93, 6, 255);
        }
        else if (attri == "blue")
        {
            magicBullet.tag = "Magic";
            magicBullet.gameObject.GetComponent<SpriteRenderer>().color = new Color32(250, 250, 250, 255);
        }
        else if (attri == "green")
        {
            magicBullet.tag = "Arrow";
            magicBullet.gameObject.GetComponent<SpriteRenderer>().color = new Color32(6, 248, 37, 255);
        }
        if (target.transform.position.x <= transform.position.x)
        {
            shell = Instantiate(magicBullet, childObj.transform.position, Quaternion.Euler(20, 0, 0));
        }
        else
        {
            shell = Instantiate(magicBullet, childObj.transform.position, Quaternion.Euler(20, 180f, 0));
        }
        bossAnimator.SetTrigger("idle");
    }

    void colorChange()
    {
        if (playerManager.attri == "red")
        {
            this.attri = "blue";
            sliderImage.color = new Color32(6, 191, 248, 255);
        }
        else if (playerManager.attri == "blue")
        {
            this.attri = "green";
            sliderImage.color = new Color32(6, 248, 37, 255);
        }
        else if (playerManager.attri == "green")
        {
            this.attri = "red";
            sliderImage.color = new Color32(248, 93, 6, 255);
        }
    }

    void fullFire()
    {

    }

    private void OnTriggerStay(Collider collision)
    {
        Debug.Log("sodhit");
        if (collision.gameObject.tag == "RedSword")
        {
            if (attri == "red")
            {
                hp -= swordDamage;
            }
            else if (attri == "blue")
            {
                hp -= swordDamage / 2;
            }
            else if (attri == "green")
            {
                hp -= swordDamage * 2;
                gameManager.SendMessage("AddScoreCritical");
            }
        }
        else if (collision.gameObject.tag == "BlueSword")
        {
            if (attri == "red")
            {
                hp -= swordDamage * 2;
                gameManager.SendMessage("AddScoreCritical");
            }
            else if (attri == "blue")
            {
                hp -= swordDamage;
            }
            else if (attri == "green")
            {
                hp -= swordDamage / 2;
            }
        }
        else if (collision.gameObject.tag == "GreenSword")
        {
            if (attri == "red")
            {
                hp -= swordDamage / 2;
            }
            else if (attri == "blue")
            {
                hp -= swordDamage * 2;
                gameManager.SendMessage("AddScoreCritical");
            }
            else if (attri == "green")
            {
                hp -= swordDamage;
            }
        }
        else if (collision.gameObject.tag == "RedMagic")
        {
            if (attri == "red")
            {
                hp -= swordDamage;
            }
            else if (attri == "blue")
            {
                hp -= swordDamage / 2;
            }
            else if (attri == "green")
            {
                hp -= swordDamage * 2;
                gameManager.SendMessage("AddScoreCritical");
            }

            Destroy(collision.gameObject);
            Debug.Log("destroy");
        }
        else if (collision.gameObject.tag == "BlueMagic")
        {
            if (attri == "red")
            {
                hp -= magicDamage * 2;
                gameManager.SendMessage("AddScoreCritical");
            }
            else if (attri == "blue")
            {
                hp -= magicDamage;
            }
            else if (attri == "green")
            {
                hp -= magicDamage / 2;
            }
            Destroy(collision.gameObject);
            Debug.Log("destroy");
        }
        else if (collision.gameObject.tag == "GreenMagic")
        {
            if (attri == "red")
            {
                hp -= magicDamage / 2;
            }
            else if (attri == "blue")
            {
                hp -= magicDamage * 2;
                gameManager.SendMessage("AddScoreCritical");
            }
            else if (attri == "green")
            {
                hp -= magicDamage;
            }
            Destroy(collision.gameObject);
            Debug.Log("destroy");
        }

        sword.enabled = false;
        slider.value = hp;
    }
}


