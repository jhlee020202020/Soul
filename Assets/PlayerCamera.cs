using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;        // Player 위치 따라가기
    public Transform cameraRoot;    // 회전 중심
    public float distance = 4f;

    public float rotSpeedX = 180f;
    public float rotSpeedY = 120f;

    public float minY = -20f;
    public float maxY = 70f;

    float rotX;
    float rotY;

    void Start()
    {
        Vector3 angles = cameraRoot.eulerAngles;
        rotY = angles.y;
        rotX = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // 1️⃣ 카메라 회전 처리
        float mouseX = Input.GetAxis("Mouse X") * rotSpeedX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotSpeedY * Time.deltaTime;

        rotY += mouseX;
        rotX = Mathf.Clamp(rotX - mouseY, minY, maxY);

        // CameraRoot만 회전
        cameraRoot.rotation = Quaternion.Euler(rotX, rotY, 0f);

        // 2️⃣ CameraRoot는 Player를 부드럽게 따라감
        cameraRoot.position = player.position + new Vector3(0, 1.6f, 0);

        // 3️⃣ 카메라는 CameraRoot 뒤 고정 위치로 이동
        transform.position = cameraRoot.position - cameraRoot.forward * distance;

        // 4️⃣ 카메라는 CameraRoot 방향을 본다
        transform.rotation = cameraRoot.rotation;
    }
}




