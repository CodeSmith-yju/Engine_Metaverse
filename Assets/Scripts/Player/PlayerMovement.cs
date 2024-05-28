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
        Cursor.lockState = CursorLockMode.Locked;  // 마우스를 화면 중앙에 고정
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
            Cursor.lockState = CursorLockMode.Locked;  // 마우스를 화면 중앙에 고정
        }
        // 마우스 입력을 통한 화면 회전
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotationY += mouseX;

        // 플레이어 오브젝트 회전
        transform.localRotation = Quaternion.Euler(0f, rotationY, 0f);


        // 플레이어 이동
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        float fall = rb.velocity.y;

        // 로컬 좌표계에서 이동 방향 계산
        Vector3 move = transform.right * inputX + transform.forward * inputZ;

        // 속도와 중력을 적용하여 이동 벡터 설정
        Vector3 velo = move * speed;
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
