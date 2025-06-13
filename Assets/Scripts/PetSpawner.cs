using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FoxSpawner : MonoBehaviour
{
    public ARRaycastManager arRaycastManager;
    public GameObject petPrefab;

    private GameObject spawnedPet;
    private List<ARRaycastHit> arHits = new List<ARRaycastHit>();

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
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit physicsHit))
                {
                    PetController pet = physicsHit.collider.GetComponent<PetController>();
                    if (pet != null)
                    {
                        pet.OnTouched();
                        return;
                    }
                }

                if (arRaycastManager.Raycast(touch.position, arHits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = arHits[0].pose;

                    if (spawnedPet == null) // ��ȯ�� ���� ���� ���� ���� ��ȯ
                    {
                        spawnedPet = Instantiate(petPrefab, hitPose.position, hitPose.rotation);
                    }
                }
            }
        }
    }
}
