using Puzzle;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentControllerComponent : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent mAgent;
    [SerializeField]
    Animator mAnimator;
    [SerializeField]
    string WalkLeft;
    [SerializeField]
    string WalkRight;
    [SerializeField]
    string WalkFWD;
    [SerializeField]
    string WalkBWD;
    [SerializeField]
    string Idle;

    string mBeforeAnim;
    Vector3 Goal;
    Action Action;
    [SerializeField]
    GameObject FLower;
    public BoxCollider Box;

    void OnEnable()
    {
        if (!FlowerComponent.Instances.Any())
        {
            CreateElements();
            return;
        }

        var i = UnityEngine.Random.Range(0, 2);
        if (i == 0)
        {
            ChangeColor();
        }
        else
        {
            CreateElements();
        }
    }

    void Update()
    {
        if (!mAgent.enabled)
        {
            return;
        }
        mAgent.SetDestination(Goal);
        var normal = new Vector2(mAgent.velocity.x, mAgent.velocity.z).normalized;
        string anim = "";
        if (normal.x > 0.5f)
        {
            anim = WalkRight;
        }
        else if (normal.x < 0.5f)
        {
            anim = WalkLeft;
        }
        if (normal.y > 0.5f)
        {
            anim = WalkFWD;
        }
        else if (normal.y < 0.5f)
        {
            anim = WalkBWD;
        }
        if (Vector3.Distance(transform.position, Goal) <= 2f)
        {
            Action?.Invoke();
            Action = null;
            anim = Idle;
        }
        if (anim.Equals(mBeforeAnim))
        {
            return;
        }
        mAnimator.Play(anim);
        mBeforeAnim = anim;
    }

    void ChangeColor()
    {
        var flower = FlowerComponent.Instances.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First();
        Goal = flower.transform.position;
        Action = () =>
        {
            var flower1 = flower.GetComponent<FlowerComponent>();
            flower1.Color = flower1.Color == new Color(12f / 255f, 255f / 255f, 255f / 255f) ?
            new Color(118f / 255f, 53f / 255f, 231f / 255f) :
            new Color(12f / 255f, 255f / 255f, 255f / 255f);
            gameObject.SetActive(false);
        };
    }

    void CreateElements()
    {
        Goal = GetRandomPositionInBox(Box);
        Action = () =>
        {
            var flower = Instantiate(FLower);
            flower.transform.position = Goal;
            gameObject.SetActive(false);
        };

    }

    Vector3 GetRandomPositionInBox(BoxCollider box)
    {
        // BoxCollider�� �߽ɰ� ũ�⸦ �����ɴϴ�.
        Vector3 center = box.transform.position;
        Vector3 size = box.size;

        // ������ x, y, z ��ǥ�� ����մϴ�.
        float randomX = UnityEngine.Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float randomY = UnityEngine.Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        float randomZ = UnityEngine.Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        // ���� ��ġ�� ��ȯ�մϴ�.
        return box.transform.TransformPoint(new Vector3((int)randomX, (int)randomY, (int)randomZ));
    }
}
