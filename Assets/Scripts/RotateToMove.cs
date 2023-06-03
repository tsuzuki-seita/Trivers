using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMove : MonoBehaviour
{
    private Vector3 latestPos;

    private void Start()
    {
    
    }

    private void Update()
    {
        Vector3 diff = transform.position - latestPos;   //前回からどこに進んだかをベクトルで取得
        latestPos = transform.position;  //前回のPositionの更新

        //ベクトルの大きさが0.01以上の時に向きを変える処理をする
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0,0, diff.z + 90)); //向きを変更する
        }
    }
}
