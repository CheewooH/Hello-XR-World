using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FoxSpawner : MonoBehaviour
{
    public ARRaycastManager arRaycastManager;
    public GameObject petPrefab;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Update()
    {
        // ȭ�鿡 ��ġ�� ���� �Ǹ�
        if (Input.touchCount > 0)
        {
            // ��ġ ������ ��������
            Touch touch = Input.GetTouch(0);
            // ��ġ�� ���۵Ǿ��� ���� ����
            if (touch.phase == TouchPhase.Began)
            {
                // ��ġ ��ġ�� ������� AR Raycast�� ����
                if (arRaycastManager.Raycast(touch.position, hits))
                {
                    Pose hitPose = hits[0].pose;
                    // �� �������� �ش� ��ġ�� ����
                    Instantiate(petPrefab, hitPose.position, hitPose.rotation);
                }

            }
        }
    }
}
