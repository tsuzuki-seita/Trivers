using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int bestScore;

    public int enemyCount = 0;

    public Text timerText;
    public Text scoreText;

    public float blance = 200;

    bool clear = false;

    public GameObject bossAura;
    public GameObject boss;
    [SerializeField] GameObject childObj;
    GameObject shellAura;
    GameObject shell;

    public　BossManager bossManager;
    public BossSponerMove bossSponerMove;

    // Start is called before the first frame update
    void Start()
    {
        boss.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(clear == false)
        {
            blance -= Time.deltaTime;
        }
        timerText.text = blance.ToString("f0");
        scoreText.text = "Score：" + score.ToString("f0");
    }

    void BossSpone()
    {
        Destroy(shellAura);
        boss.SetActive(true);
        boss.transform.DOMoveY(0, 2).SetEase(Ease.OutBounce).OnComplete(BossAwake);
    }

    void BossAwake()
    {
        bossManager.awake = "awake";
    }

    public void AddScoreCritical()
    {
        score += 100;
    }

    public void AddScoreClear()
    {
        clear = true;
        score +=  (int)blance * 10;
        if(bestScore < score)
        {
            bestScore = score;
        }
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score);
        //SceneManager.LoadScene();
    }

    public void EnemyBreak()
    {
        enemyCount++;
        Debug.Log(enemyCount);
        if (enemyCount >= 6)
        {
            shellAura = Instantiate(bossAura, childObj.transform.position, Quaternion.identity);
            Invoke("BossSpone", 2);
            bossManager.awake = "kourin";
            bossSponerMove.spone();
        }
    }
}
