using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class RaycastTarget : MonoBehaviour
    {
        [SerializeField] UnityEvent<GameObject> _event;
        [SerializeField] float _maxDistance;
        void Update()
        {
            // ���콺 Ŭ���� ����ĳ��Ʈ �߻�
            if (Input.GetMouseButtonDown(0)) // 0: ��Ŭ��
            {
                RaycastFromMouse();
            }
        }

        void RaycastFromMouse()
        {
            // ī�޶󿡼� ���콺 ��ġ�� ���� ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // RaycastHit ����ü�� ����ĳ��Ʈ �浹 ������ ��� ����
            RaycastHit hit;

            // ����ĳ��Ʈ ����: ���̰� �浹�� ������Ʈ�� �ִ��� Ȯ��
            if (Physics.Raycast(ray, out hit, _maxDistance))
            {
                // �浹�� ������Ʈ�� ���� ���
                GameObject hitObject = hit.collider.gameObject;
                _event.Invoke(hitObject);
            }

        }

        public void SetPositoin(GameObject obj)
        {
            transform.position = obj.transform.position;
        }
    }

}
