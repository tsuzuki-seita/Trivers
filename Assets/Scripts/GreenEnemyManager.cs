using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreenEnemyManager : MonoBehaviour
{
    private int hp = 100;
    public int swordDamage = 40;
    public int magicDamage = 40;

    public BoxCollider sword;
    public GameObject player;

    public GameObject arrow;    //矢
    [SerializeField] GameObject childObj;
    GameObject shell;

    public GameObject target;
    public Animator greenAnimator;
    
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
        greenAnimator.SetTrigger("walk");

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
                greenAnimator.SetTrigger("attack");
            }
        }
        if (state == EnemyState.Attack)
        {
            Invoke("ArrowFire", 1);
            
            state = EnemyState.Wait;
            timer = 0;
            greenAnimator.SetTrigger("idle");
        }
        if (state == EnemyState.Wait)
        {

            timer += Time.deltaTime;
            if (timer > 2)
            {
                state = EnemyState.Walk;
                greenAnimator.SetTrigger("walk");
            }
        }
        if (state == EnemyState.Walk)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > 15f)
            {

                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 3 * Time.deltaTime);
            }
            else
            {
                state = EnemyState.Attack;
                greenAnimator.SetTrigger("attack");
            }
        }
    }

    void ArrowFire()
    {
        if (player.transform.position.x <= transform.position.x)
        {
            shell = Instantiate(arrow, childObj.transform.position, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            shell = Instantiate(arrow, childObj.transform.position, Quaternion.Euler(0, 180f, 0));
        }
        greenAnimator.SetTrigger("idle");
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
        slider.value = hp;
    }
}
