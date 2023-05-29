using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // PlayerのAnimator
    public Animator playerAnimator;
    // PlayerのRigidbody
    public Rigidbody playerRigidbody;

    // 以下変数
    // 移動速度の速さを指定
    public float maxSpeed = 9f;
    // PlayerSpriteの初期サイズを保存する変数
    Vector3 defaultLocalScale;

    public enum MyState
    {
        Idle,
        Walk,
        Attack,
        Casting,
        hurt
    };

    MyState state = MyState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        // 初期状態でPlayerの大きさを保存
        defaultLocalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
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
            float direction = Mathf.Sign(horizontalInput);
            // キャラの向きをキーの押された方向に指定する
            transform.localScale = new Vector3(defaultLocalScale.x * direction, defaultLocalScale.y, defaultLocalScale.z);
            state = MyState.Walk;
        }

        playerAnimator.SetFloat("Horizontal", horizontalInput);
        playerAnimator.SetFloat("Vertical", verticalInput);


        if (Input.GetMouseButtonDown(0))
        {
            if (state != MyState.Attack)
            {
                // アニメーションの再生
                playerAnimator.SetTrigger("attack");
                playerRigidbody.velocity = Vector2.zero;
                state = MyState.Attack;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            state = MyState.Idle;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (state != MyState.Casting)
            {
                // アニメーションの再生
                playerAnimator.SetTrigger("casting");
                playerRigidbody.velocity = Vector2.zero;
                state = MyState.Casting;
            }

        }
        if (Input.GetMouseButtonUp(1))
        {
            state = MyState.Idle;
        }
    }
}
