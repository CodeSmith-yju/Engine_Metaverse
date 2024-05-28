using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 100f;
    private Rigidbody rb;
    private float rotationY = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;  // ���콺�� ȭ�� �߾ӿ� ����
    }

    // Update is called once per frame
    void Update()
    {
        if (KioskSystem.single.kiosck)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;  // ���콺�� ȭ�� �߾ӿ� ����
        }
        // ���콺 �Է��� ���� ȭ�� ȸ��
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotationY += mouseX;

        // �÷��̾� ������Ʈ ȸ��
        transform.localRotation = Quaternion.Euler(0f, rotationY, 0f);


        // �÷��̾� �̵�
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        float fall = rb.velocity.y;

        // ���� ��ǥ�迡�� �̵� ���� ���
        Vector3 move = transform.right * inputX + transform.forward * inputZ;

        // �ӵ��� �߷��� �����Ͽ� �̵� ���� ����
        Vector3 velo = move * speed;
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
