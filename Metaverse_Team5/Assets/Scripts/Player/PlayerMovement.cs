using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }



    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        float fall = rb.velocity.y;

        Vector3 velo = new Vector3(inputX, 0, inputZ);

        velo *= speed;
        velo.y = fall;
        rb.velocity = velo;
/*

        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance, interactableLayer);

        // 가장 가까운 콜라이더 찾기
        float minDistance = float.MaxValue;
        nearestCollider = null;

        foreach (Collider col in colliders)
        {
            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCollider = col;
            }
        }

        // 'Z' 키를 누르고 가장 가까운 콜라이더가 있는 경우 상호작용
        if (Input.GetKeyDown(KeyCode.Z) && nearestCollider != null)
        {
            Debug.Log("상호작용: " + nearestCollider);
        }*/
    }
}
