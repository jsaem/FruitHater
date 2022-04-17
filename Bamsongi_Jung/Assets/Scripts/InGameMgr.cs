using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum GameState
{
    GameReady,
    GameStart,
    GameOver
}

public class InGameMgr : MonoBehaviour
{
    public Terrain m_RefMap = null;

    public GameObject[] FoodArr;
    public Transform FoodGroup;

    [Header("UI")]
    [HideInInspector] public static int gameScore = 0;
    public Text scoreText;
    private float timer = 60.0f;
    public Text timerText;
    public GameObject gameOverPanel = null;
    public Text m_resultText = null;

    public static InGameMgr Inst = null;

    CameraController CameraCtrl;
    float span = 1.0f;
    float delta = 0.0f;

    [Header("Lobby")]
    public Button m_gameStartBtn = null;
    public GameObject m_instructionPanel = null;
    public GameObject m_instructionFoodGroup = null;

    void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.0f;
        gameOverPanel.SetActive(false);

        if (m_gameStartBtn != null)
            m_gameStartBtn.onClick.AddListener(() =>
            {
                m_instructionPanel.SetActive(false);
                m_instructionFoodGroup.SetActive(false);
                Time.timeScale = 1.0f;
                timer = 60.0f;
                StaticRandGen();
            });

        CameraCtrl = FindObjectOfType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_instructionPanel.activeSelf)
        {
            this.timer -= Time.deltaTime;
            this.timerText.text = ((int)this.timer).ToString();
        }
        if (timer <= 0.0f && !gameOverPanel.activeSelf)
        {
            Time.timeScale = 0.0f;
            m_resultText.text = "Score: " + gameScore.ToString();
            gameOverPanel.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            timer = 60.0f;
            this.timerText.text = ((int)this.timer).ToString();
            gameOverPanel.SetActive(false);
            gameScore = 0;
            scoreText.text = "Score: " + gameScore.ToString();
            m_resultText.text = "Score: " + gameScore.ToString();
            Time.timeScale = 1.0f;
        }

        DynamicGenerator();
    }

    void StaticRandGen()
    {
        for (int ii = 0; ii < 200; ii++)
        {
            Vector3 RandomXYZ = new Vector3(
                Random.Range(-250.0f, 250.0f),
                10.0f,
                Random.Range(-250.0f, 250.0f));

            RandomXYZ.y = m_RefMap.SampleHeight(RandomXYZ) + Random.Range(0.0f, 20.0f);

            int Kind = Random.Range(0, FoodArr.Length);
            GameObject go = Instantiate(FoodArr[Kind]) as GameObject;
            go.transform.position = RandomXYZ;
            go.transform.eulerAngles = new Vector3(0.0f,
                Random.Range(0.0f, 360.0f), 0.0f);

            if (FoodGroup != null)
                go.transform.SetParent(FoodGroup);
        }
    }

    public void AddScore(int Value)
    {
        gameScore += Value;

        if (scoreText != null)
            scoreText.text = "Score: " + gameScore.ToString();
    }

    void DynamicGenerator() // 캐릭터가 멈췄을때 눈앞에 랜덤하게 몬스터 생성 함수
    {
        if (CameraCtrl != null && CameraCtrl.IsMove())
            this.delta = 0.0f;

        this.delta += Time.deltaTime;
        if (this.delta > this.span)
        {
            this.delta = 0;

            int Kind = Random.Range(0, FoodArr.Length);

            Vector3 CamForW = Camera.main.transform.forward;
            CamForW.y = 0.0f;
            CamForW.Normalize();
            CamForW = CamForW * (float)Random.Range(10.0f, 22.0f);

            Vector3 CacX = Camera.main.transform.right;
            CacX.y = 0.0f;
            CacX.Normalize();
            CacX = CacX * (float)Random.Range(-12.0f, 12.0f);

            Vector3 CacY = Vector3.up;
            CacY.Normalize();
            CacY = CacY * (float)Random.Range(-5.0f, 5.0f);

            Vector3 SpPos = Camera.main.transform.position + CacX + CacY + CamForW;
            GameObject go = Instantiate(FoodArr[Kind]) as GameObject;
            go.transform.position = SpPos;

            Vector3 a_Dir = Camera.main.transform.forward;
            a_Dir.y = 0.0f;
            a_Dir.Normalize();
            Vector3 a_Rot = Quaternion.LookRotation(a_Dir).eulerAngles;
            go.transform.eulerAngles = new Vector3(0.0f,
                a_Rot.y + Random.Range(-90.0f, 90.0f), 0.0f);
        }
    }
}
