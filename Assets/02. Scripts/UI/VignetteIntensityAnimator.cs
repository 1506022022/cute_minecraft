using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteIntensityAnimator : MonoBehaviour
{
    public Volume volume; // Volume ������Ʈ�� ����
    private Vignette vignette; // Vignette ȿ���� ���� ����
    private float targetIntensity = 0f; // ��ǥ Intensity �� (0)
    private float duration = 2f; // 2�� ���� ��ȭ
    private float startTime; // �ִϸ��̼� ���� �ð�

    void Start()
    {
        // Volume���� Vignette ȿ���� ã��
        if (volume.profile.TryGet(out vignette))
        {
            // �ʱ� Intensity �� ����
            vignette.intensity.Override(1f); // ó���� 1�� ����
            startTime = Time.time; // ���� �ð��� ���
        }
    }

    void Update()
    {
        if (vignette != null)
        {
            // ���� �ð��� ���� Intensity �� ���
            float elapsedTime = Time.time - startTime; // ��� �ð�
            float progress = elapsedTime / duration; // 0���� 1�� ��ȭ

            // 2�ʰ� ������ Intensity�� 0���� ����
            vignette.intensity.Override(Mathf.Lerp(1f, targetIntensity, progress));

            // 2�ʰ� ������ �ִϸ��̼��� ���߰� Intensity�� ��Ȯ�� 0���� ����
            if (progress >= 1f)
            {
                vignette.intensity.Override(targetIntensity);
            }
        }
    }
}