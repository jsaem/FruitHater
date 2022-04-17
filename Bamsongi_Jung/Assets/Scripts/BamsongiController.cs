using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BamsongiController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Shoot(new Vector3(0, 200, 2000));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Shoot(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);

        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<ParticleSystem>().Play();

        Destroy(this.gameObject, 4.0f);

        if (other.gameObject.tag == "Fruit")
        {
            // ---- 밤송이 외형 안보이게 정리
            GetComponent<SphereCollider>().enabled = false;

            MeshRenderer[] a_ChildList = gameObject.GetComponentsInChildren<MeshRenderer>();
            for (int ii = 0; ii < a_ChildList.Length; ii++)
            {
                a_ChildList[ii].enabled = false;
            }
            // ---- 밤송이 외형 안보이게 정리

            // 보상
            Destroy(other.gameObject);
            InGameMgr.Inst.AddScore(100);
        }
        else if (other.gameObject.tag == "Food")
        {
            // ---- 밤송이 외형 안보이게 정리
            GetComponent<SphereCollider>().enabled = false;

            MeshRenderer[] a_ChildList = gameObject.GetComponentsInChildren<MeshRenderer>();
            for (int ii = 0; ii < a_ChildList.Length; ii++)
            {
                a_ChildList[ii].enabled = false;
            }
            // ---- 밤송이 외형 안보이게 정리

            // 보상
            Destroy(other.gameObject);
            InGameMgr.Inst.AddScore(-50);
        }
    } //void OnCollisionEnter(Collision other)
}
