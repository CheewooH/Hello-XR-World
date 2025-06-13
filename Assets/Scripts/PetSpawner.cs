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
        // 화면에 터치가 감지 되면
        if (Input.touchCount > 0)
        {
            // 터치 정보를 가져오고
            Touch touch = Input.GetTouch(0);
            // 터치가 시작되었을 때만 실행
            if (touch.phase == TouchPhase.Began)
            {
                // 터치 위치를 기반으로 AR Raycast를 실행
                if (arRaycastManager.Raycast(touch.position, hits))
                {
                    Pose hitPose = hits[0].pose;
                    // 펫 프리팹을 해당 위치에 생성
                    Instantiate(petPrefab, hitPose.position, hitPose.rotation);
                }

            }
        }
    }
}
