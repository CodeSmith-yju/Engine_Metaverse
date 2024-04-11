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

        // ���� ����� �ݶ��̴� ã��
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

        // 'Z' Ű�� ������ ���� ����� �ݶ��̴��� �ִ� ��� ��ȣ�ۿ�
        if (Input.GetKeyDown(KeyCode.Z) && nearestCollider != null)
        {
            Debug.Log("��ȣ�ۿ�: " + nearestCollider);
        }*/
    }
}
