using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    public Animator m_Animator;

    [Range(0, 10f)]
    public float f_MoveSpeed;

    [Range(0, 10f)]
    public float f_RunSpeed;

    [Range(0, 100f)]
    public float f_RotateSpeed;

    public GameObject obj_Rotate_Horizontal;
    public GameObject obj_Rotate_Vertical;
    public GameObject obj_Body;
    public GameObject obj_Cam_First, obj_Cam_Quarter;
    public TextMesh player_Name;

    float yRotation;
    float xRotation;


    // Start is called before the first frame update
    private void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            obj_Cam_First.SetActive(false);
            obj_Cam_Quarter.SetActive(true);
            this.gameObject.name = player_Name.text + " (LocalPlayer)";
        }
        else
        {
            obj_Cam_First.SetActive(false);
            obj_Cam_Quarter.SetActive(false);
            this.gameObject.name += player_Name.text + " (OtherPlayer)";
        }
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        float pos_x;
        float pos_z;
        if (GetComponent<PhotonView>().IsMine && !GetComponent<Players>().ui_Opened)
        {
            if (KioskSystem.single.kiosck)
            {
                return;
            }
            else
            {
                pos_x = Input.GetAxis("Horizontal");
                pos_z = Input.GetAxis("Vertical");
            }

            //달리기 ON&OFF
            /*if (Input.GetKey(KeyCode.LeftShift))
            {
                m_Animator.SetBool("Run", true);
            }
            else
            {
                m_Animator.SetBool("Run", false);
            }*/

            if (Input.GetKeyDown(KeyCode.F5))
            {
                if (obj_Cam_First.activeSelf)
                {
                    obj_Cam_First.SetActive(false);
                    obj_Cam_Quarter.SetActive(true);
                }
                else
                {
                    obj_Cam_First.SetActive(true);
                    obj_Cam_Quarter.SetActive(false);
                }
                
            }

            //걷기 ON&OFF 및 캐릭터 이동
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                //Debug.Log(new Vector2(pos_x, pos_z));
                if (pos_x > 0)
                {
                    if (pos_z > 0)
                    {
                        obj_Body.transform.localEulerAngles = new Vector3(0f, 45f, 0f);
                        //transform.Rotate(new Vector3(0f, 45f, 0f));
                    }
                    else if (pos_z < 0)
                    {
                        obj_Body.transform.localEulerAngles = new Vector3(0f, 135f, 0f);
                        //transform.Rotate(new Vector3(0f, 135f, 0f));
                    }
                    else
                    {
                        obj_Body.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
                        //transform.Rotate(new Vector3(0f, 90f, 0f));
                    }
                }
                else if (pos_x < 0)
                {
                    if (pos_z > 0)
                    {
                        obj_Body.transform.localEulerAngles = new Vector3(0f, -45f, 0f);
                        //transform.Rotate(new Vector3(0f, -45f, 0f));
                    }
                    else if (pos_z < 0)
                    {
                        obj_Body.transform.localEulerAngles = new Vector3(0f, -135f, 0f);
                        //transform.Rotate(new Vector3(0f, -135f, 0f));
                    }
                    else
                    {
                        obj_Body.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
                        //transform.Rotate(new Vector3(0f, 270f, 0f));
                    }
                }
                else
                {
                    if (pos_z > 0)
                    {
                        obj_Body.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                        //transform.Rotate(new Vector3(0f, 0f, 0f));
                    }
                    else if (pos_z < 0)
                    {
                        obj_Body.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                        //transform.Rotate(new Vector3(0f, 45f, 0f));
                    }
                }

                m_Animator.SetBool("Walk", true);
                if (m_Animator.GetBool("Run"))
                {
                    transform.Translate(new Vector3(pos_x, 0, pos_z) * Time.deltaTime * f_MoveSpeed * f_RunSpeed);
                }
                else
                {
                    //transform.position += new Vector3(pos_x, 0, pos_z) * Time.deltaTime * f_MoveSpeed;
                    transform.Translate(new Vector3(pos_x, 0, pos_z) * Time.deltaTime * f_MoveSpeed);
                }
            }
            else
            {
                m_Animator.SetBool("Walk", false);
            }

            if (Input.GetMouseButton(1))
            {
                /*float rot_x = Input.GetAxis("Mouse Y");
                float rot_y = Input.GetAxis("Mouse X");

                if (obj_Cam_First.activeSelf)
                {
                    obj_Cam_First.transform.rotation = Quaternion.Euler(rot_x, rot_y, 0f);
                }

                transform.eulerAngles += new Vector3(0, rot_y, 0) * f_RotateSpeed;
                //obj_Rotate_Horizontal.transform.eulerAngles += new Vector3(0, rot_y, 0) * f_RotateSpeed;*/


                float mouseX = Input.GetAxisRaw("Mouse X") * f_RotateSpeed;
                float mouseY = Input.GetAxisRaw("Mouse Y") * f_RotateSpeed;

                yRotation += mouseX;    // 마우스 X축 입력에 따라 수평 회전 값을 조정
                xRotation -= mouseY;    // 마우스 Y축 입력에 따라 수직 회전 값을 조정

                xRotation = Mathf.Clamp(xRotation, -70f, 70f);  // 수직 회전 값을 -90도에서 90도 사이로 제한

                if (obj_Cam_First.activeSelf)
                {
                    obj_Cam_First.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
                }

                transform.rotation = Quaternion.Euler(0, yRotation, 0);             // 플레이어 캐릭터의 회전을 조절
            }
        }

        if (GetComponent<PhotonView>().IsMine && GetComponent<Players>().ui_Opened)
        {
            m_Animator.SetBool("Walk", false);
            m_Animator.SetBool("Run", false);
        }
    }
}