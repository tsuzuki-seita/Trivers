using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.DOMoveY(650, 2).SetEase(Ease.OutBounce);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
