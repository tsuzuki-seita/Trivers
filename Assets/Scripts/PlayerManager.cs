using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // PlayerのAnimator
    public Animator playerAnimator;
    // PlayerのRigidbody
    public Rigidbody playerRigidbody;

    public BoxCollider sword;   //剣の当たり判定

    public GameObject MagicAura;    //魔法弾オーラ
    public GameObject MagicBullet;    //魔法弾prefab
    [SerializeField] GameObject childObj;

    public float direction;

    GameObject shellAura;
    GameObject shell;

    private int count;  //魔法弾で使う
    private float magiccount = 0;

    // 以下変数
    // 移動速度の速さを指定
    public float maxSpeed = 9f;
    // PlayerSpriteの初期サイズを保存する変数
    Vector3 defaultLocalScale;

    // Start is called before the first frame update
    void Start()
    {
        // 初期状態でPlayerの大きさを保存
        defaultLocalScale = transform.localScale;

        sword.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
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
            

            shellAura = Instantiate(MagicAura, childObj.transform.position, Quaternion.identity);
            
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
        if(direction >= 0)
        {
            shell = Instantiate(MagicBullet, childObj.transform.position, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            shell = Instantiate(MagicBullet, childObj.transform.position, Quaternion.Euler(0, 180f, 0));
        }
        
    }
}
