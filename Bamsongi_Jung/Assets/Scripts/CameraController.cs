using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;

    float movingSpeed = 10.0f;
    Vector3 moveDir = Vector3.zero;

    // ------- 카메라 회전을 위한 변수
    float rotSpeed = 250.0f;
    Vector3 m_CacVec = Vector3.zero;
    // ------- 카메라 회전을 위한 변수

    // ------- 높이값 찾기 위한 변수
    public Terrain m_RefMap = null;
    // ------- 높이값 찾기 위한 변수

    void Start()
    {

    }

    void Update()
    {
        // ---- 카메라 회전 구현 부분
        if (Input.GetMouseButton(1))
        {
            m_CacVec = transform.eulerAngles;
            m_CacVec.y = m_CacVec.y + (rotSpeed * Time.deltaTime * 
                Input.GetAxis("Mouse X")); //게임창에서 마우스를 왼쪽 오른쪽으로 이동할때 마다 (왼 -음수 : 오른 +양수))
            m_CacVec.x = m_CacVec.x - (rotSpeed * Time.deltaTime * 
                Input.GetAxis("Mouse Y")); //게임창에서 마우스를 왼쪽 오른쪽으로 이동할때 마다 (아래 -음수 : 위 +양수)

            if (270.0f < m_CacVec.x && m_CacVec.x < 340.0f)
                m_CacVec.x = 340.0f;

            if (m_CacVec.x < 90.0f && 12.0f < m_CacVec.x)
                m_CacVec.x = 12.0f;
            transform.eulerAngles = m_CacVec;
        }
        // ---- 카메라 회전 구현 부분

        // ---- 이동 구현 부분
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        // 전후좌우 이동 방향 벡터 계산
        moveDir = Vector3.right * h + Vector3.forward * v;
        //대각선 이동으로 하면서 루트2로 길이가 늘어나기에 1로 만들어준다. (정규화:Normalize)
        moveDir.Normalize();

        // Translate(이동방향 * Time.deltaTime * 속도, 기준좌표)
        transform.Translate(moveDir * Time.deltaTime * movingSpeed, Space.Self);
        // ---- 이동 구현 부분

        // ---- 캐릭터의 높이값 찾기
        if (m_RefMap != null)
        {
            transform.position = new Vector3(transform.position.x,
                                m_RefMap.SampleHeight(transform.position) + 5.0f, 
                                transform.position.z);
        }
        // ---- 캐릭터의 높이값 찾기
    } //void Update()

    public bool IsMove()
    {
        if (h == 0.0f && v == 0.0f)
            return false;
        return true;
    }
}
