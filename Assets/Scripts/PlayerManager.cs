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

    // 移動速度の速さを指定
    public float maxSpeed = 6f;
    // PlayerSpriteの初期サイズを保存する変数
    Vector3 defaultLocalScale;

    private int hp = 500;
    public int magicDamage = 50;
    public int legDamage = 50;
    private string attri = "red";

    public Slider slider;
    public Image sliderImage;

    public GameObject gameOver; // 追加

    // Start is called before the first frame update
    void Start()
    {
        // 初期状態でPlayerの大きさを保存
        defaultLocalScale = transform.localScale;

        sword.enabled = false;

        slider.maxValue = hp;
        slider.value = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 0)
        {
            GameOver();
            playerCollider.enabled = false;
            Debug.Log("deth");
        }

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

        // 移動の横方向をInputから値で取得
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //現在のy軸の位置を取得
        float posY = transform.position.y;
        //現在のX軸の位置を取得
        float posX = transform.position.x;

        // 速度をセットする
        playerRigidbody.velocity = new Vector2(horizontalInput * maxSpeed, verticalInput * maxSpeed);

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


        if (Input.GetMouseButtonDown(0))
        {
            if(attri == "red")
            {
                sword.tag = "RedSword";
            }
            else if(attri == "blue")
            {
                sword.tag = "BlueSword";
            }
            else if(attri == "green")
            {
                sword.tag = "GreenSword";
            }

            sword.enabled = true;
   
            //Invoke("col", 0.f);

            // アニメーションの再生
            playerAnimator.SetTrigger("attack");
            playerRigidbody.velocity = Vector2.zero;
            

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
            
        }

    }

    void MagicFire()
    {
        Destroy(shellAura);
        if (attri == "red")
        {
            magicBullet.tag = "RedMagic";
        }
        else if (attri == "blue")
        {
            magicBullet.tag = "BlueMagic";
        }
        else if (attri == "green")
        {
            magicBullet.tag = "GreenMagic";
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

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("enemyhit");
        if (other.gameObject.tag == "Leg")
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
        else if (other.gameObject.tag == "Magic")
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
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Arrow")
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
            Destroy(other.gameObject);
        }
        slider.value = hp;
    }

    void GameOver()
    {
        playerAnimator.SetTrigger("die");
        gameOver.SetActive(true);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
