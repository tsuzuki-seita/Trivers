using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // PlayerのAnimator
    public Animator playerAnimator;
    // PlayerのRigidbody
    public Rigidbody playerRigidbody;

    public BoxCollider sword;   //剣の当たり判定
    public BoxCollider playerCollider;

    public GameObject magicAura;    //魔法弾オーラ
    public GameObject magicBullet;    //魔法弾prefab
    [SerializeField] GameObject childObj;

    public float direction;

    GameObject shellAura;
    GameObject shell;

    private int count;  //魔法弾で使う
    private float magiccount = 0;
    private float attackcount = 0;

    // 移動速度の速さを指定
    public float maxSpeed = 6f;
    // PlayerSpriteの初期サイズを保存する変数
    Vector3 defaultLocalScale;

    private int hp = 5000;
    private int currentHp;
    public int magicDamage = 40;
    public int legDamage = 20;
    public string attri = "red";

    public Slider slider;
    public Image sliderImage;
    public Slider mpSlider;
    public float mp = 0;

    public GameObject gameOver; // 追加

    // Start is called before the first frame update
    void Start()
    {
        // 初期状態でPlayerの大きさを保存
        defaultLocalScale = transform.localScale;

        sword.enabled = false;

        slider.maxValue = hp;
        slider.value = hp;

        mpSlider.maxValue = 1;
        mpSlider.value = 0;

        currentHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 0)
        {
            GameOver();
            SceneManager.LoadScene("GameOverScene");
            playerCollider.enabled = false;
            Debug.Log("deth");
        }

        //if(hp != currentHp)
        //{
        //    playerAnimator.SetTrigger("hurt");
        //    currentHp = hp;
        //}

        if (Input.GetKeyDown("q"))
        {
            attri = "red";
            sliderImage.color = new Color32(248, 93, 6, 255);
        }
        else if (Input.GetKeyDown("e"))
        {
            attri = "blue";
            sliderImage.color = new Color32(6, 191, 248, 255);
        }
        else if (Input.GetKeyDown("r"))
        {
            attri = "green";
            sliderImage.color = new Color32(6, 248, 37, 255);
        }
 
        magiccount += Time.deltaTime;
        attackcount += Time.deltaTime;

        // 移動の横方向をInputから値で取得
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //現在のy軸の位置を取得
        float posY = transform.position.y;
        //現在のX軸の位置を取得
        float posX = transform.position.x;

        // 速度をセットする
        playerRigidbody.velocity = new Vector2(horizontalInput * maxSpeed, verticalInput * maxSpeed);

        //追加　現在のポジションを保持する
        Vector3 currentPos = transform.position;

        //追加　Mathf.ClampでX,Yの値それぞれが最小～最大の範囲内に収める。
        //範囲を超えていたら範囲内の値を代入する
        //currentPos.x = Mathf.Clamp(currentPos.x, -40, 0);
        currentPos.y = Mathf.Clamp(currentPos.y, -2.5f, 2);

        //追加　positionをcurrentPosにする
        transform.position = currentPos;

        // もし左右のキーどちらかが押されているなら
        if (horizontalInput != 0)
        {
            // キャラがどっちに向いているかを判定する
            direction = Mathf.Sign(horizontalInput);
            // キャラの向きをキーの押された方向に指定する
            transform.localScale = new Vector3(defaultLocalScale.x * direction, defaultLocalScale.y, defaultLocalScale.z);
      
        }

        playerAnimator.SetFloat("Horizontal", horizontalInput);
        playerAnimator.SetFloat("Vertical", verticalInput);


        if(attackcount > 0.75f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (attri == "red")
                {
                    sword.tag = "RedSword";
                }
                else if (attri == "blue")
                {
                    sword.tag = "BlueSword";
                }
                else if (attri == "green")
                {
                    sword.tag = "GreenSword";
                }

                sword.enabled = true;

                //Invoke("col", 0.f);

                // アニメーションの再生
                playerAnimator.SetTrigger("attack");
                playerRigidbody.velocity = Vector2.zero;
                attackcount = 0;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            
        }

        //魔法弾発射
        if (Input.GetMouseButtonDown(1))
        {
            magiccount = 0;
            // アニメーションの再生
            playerAnimator.SetTrigger("casting");
            playerRigidbody.velocity = Vector2.zero;

            shellAura = Instantiate(magicAura, childObj.transform.position, Quaternion.identity);
            
        }
        if (Input.GetMouseButton(1))
        {
            //playerAnimator.SetTrigger("casting");
            mp += Time.deltaTime;
            mpSlider.value = mp;
        }
        if (Input.GetMouseButtonUp(1))
        {
            if (magiccount >= 1)
            {
                MagicFire();
                playerAnimator.SetTrigger("idle");
            }
            else
            {
                Destroy(shellAura);
                playerAnimator.SetTrigger("idle");
            }
            mpSlider.value = 0;
            mp = 0;
        }

    }

    void MagicFire()
    {
        Destroy(shellAura);
        if (attri == "red")
        {
            magicBullet.tag = "RedMagic";
            magicBullet.gameObject.GetComponent<SpriteRenderer>().color = new Color32(248, 93, 6, 255);
        }
        else if (attri == "blue")
        {
            magicBullet.tag = "BlueMagic";
            magicBullet.gameObject.GetComponent<SpriteRenderer>().color = new Color32(250, 250, 250, 255);
        }
        else if (attri == "green")
        {
            magicBullet.tag = "GreenMagic";
            magicBullet.gameObject.GetComponent<SpriteRenderer>().color = new Color32(6, 248, 37, 255);
        }
        if (direction >= 0)
        {
            shell = Instantiate(magicBullet, childObj.transform.position, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            shell = Instantiate(magicBullet, childObj.transform.position, Quaternion.Euler(0, 180f, 0));
        }
        
    }

    private void OnTriggerStay(Collider collision)
    {
        Debug.Log("playerhit");
        if (collision.gameObject.tag == "Leg")
        {
            if (attri == "red")
            {
                hp -= legDamage;
            }
            else if (attri == "blue")
            {
                hp -= legDamage / 2;
            }
            else if (attri == "green")
            {
                hp -= legDamage * 2;
            }
        }
        else if (collision.gameObject.tag == "Magic")
        {
            if (attri == "red")
            {
                hp -= magicDamage * 2;
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
        else if (collision.gameObject.tag == "BossRedSword")
        {
            if (attri == "red")
            {
                hp -= legDamage;
            }
            else if (attri == "blue")
            {
                hp -= legDamage / 2;
            }
            else if (attri == "green")
            {
                hp -= legDamage * 2;
            }
            collision.enabled = false;
        }
        else if (collision.gameObject.tag == "BossBlueSword")
        {
            if (attri == "red")
            {
                hp -= magicDamage * 2;
            }
            else if (attri == "blue")
            {
                hp -= magicDamage;
            }
            else if (attri == "green")
            {
                hp -= magicDamage / 2;
            }
            collision.enabled = false;
        }
        else if (collision.gameObject.tag == "BossGreenSword")
        {
            if (attri == "red")
            {
                hp -= magicDamage * 2;
            }
            else if (attri == "blue")
            {
                hp -= magicDamage;
            }
            else if (attri == "green")
            {
                hp -= magicDamage / 2;
            }
            collision.enabled = false;
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            if (attri == "red")
            {
                hp -= magicDamage / 2;
            }
            else if (attri == "blue")
            {
                hp -= magicDamage * 2;
            }
            else if (attri == "green")
            {
                hp -= magicDamage;
            }
            Destroy(collision.gameObject);
            Debug.Log("arrowdestroy");
        }
        
        slider.value = hp;
    }

    void GameOver()
    {
        playerAnimator.SetTrigger("die");
    }
}
