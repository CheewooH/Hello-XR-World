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
    private int arPlaneLayerMask;

    void Start()
    {
        arPlaneLayerMask = 1 << LayerMask.NameToLayer("ARPlane"); // ARPlane 레이어만 활성화
        arPlaneLayerMask = ~arPlaneLayerMask; // ARPlane 레이어만 제외
    }

    private void Update()
    {
        // 화면에 터치가 감지 되면
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // 터치가 시작되었을 때만 실행
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit physicsHit, Mathf.Infinity, arPlaneLayerMask))
                {
                    PetController pet = physicsHit.collider.GetComponent<PetController>();

                    if (pet != null)
                    {
                        pet.OnTouched();
                        return;
                    }
                }

                // arRaycastManager를 사용하여 ARPlane 위에 터치가 있는지 확인
                if (arRaycastManager.Raycast(touch.position, arHits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = arHits[0].pose;

                    if (spawnedPet == null) // 소환된 펫이 없다면 새로 소환
                    {
                        spawnedPet = Instantiate(petPrefab, hitPose.position, hitPose.rotation);
                    }
                    else // 해당 위치로 이동
                    {
                        spawnedPet.GetComponent<PetController>().MoveTo(hitPose.position);
                    }
                }
            }
        }

        // 터치가 없으면 아무것도 하지 않음
        if (Input.touchCount == 0)
            return;

    }
}
