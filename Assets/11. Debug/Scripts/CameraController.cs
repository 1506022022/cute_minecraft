using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;     // ī�޶� �̵� �ӵ�
    public float heightSpeed = 5f;    // ī�޶� ���� ���� �ӵ�
    public float lookSpeed = 3f;      // ī�޶� ȸ�� �ӵ�
    public float maxLookAngle = 80f;  // ī�޶��� �ִ� ���� ȸ�� ����

    private float pitch = 0f;         // ī�޶��� ���� ȸ�� ����
    private float yaw = 0f;           // ī�޶��� �¿� ȸ�� ����

    void Update()
    {
        // ī�޶� �̵�
        float horizontal = Input.GetAxis("Horizontal");  // A/D �Ǵ� ��/�� ȭ��ǥ
        float vertical = Input.GetAxis("Vertical");      // W/S �Ǵ� ��/�Ʒ� ȭ��ǥ
        float heightChange = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            heightChange = -heightSpeed * Time.deltaTime; // Q Ű�� ������ �� ī�޶� ��������
        }
        else if (Input.GetKey(KeyCode.E))
        {
            heightChange = heightSpeed * Time.deltaTime;  // E Ű�� ������ �� ī�޶� �ø���
        }

        // �̵� ���� ���
        Vector3 moveDirection = new Vector3(horizontal, heightChange, vertical);
        moveDirection = transform.TransformDirection(moveDirection);  // ���� �������� ���� �������� ��ȯ

        // ī�޶� �̵�
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // ���콺 �̵��� ���� ī�޶� ȸ��
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * lookSpeed;  // �¿� ȸ��
        pitch -= mouseY * lookSpeed;  // ���� ȸ��
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);  // ���� ȸ�� ���� ����

        // ī�޶� ȸ�� ����
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
