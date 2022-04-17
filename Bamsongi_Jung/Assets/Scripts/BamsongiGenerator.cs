using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BamsongiGenerator : MonoBehaviour
{
    public GameObject bamsongiPrefab;
    public GameObject m_gamePanel;

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !m_gamePanel.activeSelf)
        {
            GameObject bamsongi = Instantiate(bamsongiPrefab) as GameObject;
            bamsongi.transform.position = 
                Camera.main.transform.position + Camera.main.transform.forward * 1.5f;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldDir = ray.direction;
            bamsongi.GetComponent<BamsongiController>().Shoot(worldDir.normalized * 3500);
        }
    }
}
