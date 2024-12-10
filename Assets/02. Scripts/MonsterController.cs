using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ڵ� ǥ��
[RequireComponent(typeof(Animator))]
public class MonsterController : MonoBehaviour
{
    private static readonly byte[] ENTER_BOSS = { 10 }; // ����
    private static readonly byte[] EXIT_BOSS = { 11 }; // ����

    // �ʵ� ������ ���λ�
    // _
    private readonly Queue<byte[]> _commandQueue = new(); // ��� ť
    private MonsterState _characterState;
    private bool _isActing = false; // �ൿ ���� ��ġ�� �ʵ���
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void InstreamData(byte[] data)
    {
        _commandQueue.Enqueue(data); // Ŀ�ǵ� ť�� �߰�

        if (!_isActing)
        {
            ProcessNextCommand(); // �ൿ ���� �ƴϸ� ���� Ŀ�ǵ� ����
        }
    }

    private void ProcessNextCommand()
    {
        if (_commandQueue.Count == 0) // Ŀ�ǵ尡 ������ ����
        {
            _isActing = false;
            return;
        }

        var command = _commandQueue.Dequeue(); // Ŀ�ǵ� ��������
        _isActing = true;

        // ����Ʈ ���� ���� ��� ����
        if (command.Length == 1 && command == ENTER_BOSS)
        {
            if (_characterState == MonsterState.None)
            {
                TrasitionState(MonsterState.Enter);
            }
        }
        else if (command.Length == 1 && command == EXIT_BOSS)
        {
            if (_characterState == MonsterState.None)
            {
                TrasitionState(MonsterState.Die);
            }
        }
        else if (command.Length == 3) // ���̰� 3�ϰ�� ������ ��ȯ ��ƾ
        {
            if (_characterState == MonsterState.None)
            {
                TrasitionState(MonsterState.Action1);
            }
        }
        else
        {
            Debug.LogWarning("�� �� ���� �Է� ��");
            _isActing = false;
            ProcessNextCommand();
        }
    }

    // �ִϸ��̼� Ű �̺�Ʈ
    public void SetState(MonsterState state)
    {
        _characterState = state;
    }

    private void TrasitionState(MonsterState state)
    {
        _characterState = state;
        _animator.SetTrigger(_characterState.ToString());
        _isActing = false;
        ProcessNextCommand();
    }

    private void Update()
    {
        if (_commandQueue.Count == 0 ||
            !_isActing) // Ŀ�ǵ尡 ������ ����
        {
            _isActing = false;
            return;
        }

        ProcessNextCommand();
    }
}

public enum MonsterState // �ִϸ����� Ʈ����
{
    None,
    Enter,
    Action1,
    Die
}