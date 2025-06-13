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
        // 화면에 터치가 감지 되면
        if (Input.touchCount > 0)
        {
            // 터치 정보를 가져오고
            Touch touch = Input.GetTouch(0);
            // 터치가 시작되었을 때만 실행
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

                    if (spawnedPet == null) // 소환된 펫이 없을 때만 새로 소환
                    {
                        spawnedPet = Instantiate(petPrefab, hitPose.position, hitPose.rotation);
                    }
                }
            }
        }
    }
}
