using UnityEngine;
using System.Collections.Generic;

public class MiniMapController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Camera miniMapCamera;
    public RectTransform mapDisplay;
    public RectTransform playerMarker;
    public RectTransform targetMarkerPrefab;

    private List<RectTransform> targetMarkers =
        new List<RectTransform>();

    private TargetDummy[] targets;

    private void Start()
    {
        FindTargets();
        CreateTargetMarkers();
    }

    private void Update()
    {
        UpdatePlayerMarker();
        UpdateTargetMarkers();
    }

    // =====================================================
    // FIND TARGETS
    // =====================================================

    private void FindTargets()
    {
        targets = FindObjectsByType<TargetDummy>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );
    }

    // =====================================================
    // CREATE TARGET MARKERS
    // =====================================================

    private void CreateTargetMarkers()
    {
        if (targetMarkerPrefab == null ||
            mapDisplay == null)
        {
            Debug.LogError(
                "MiniMapController: Target Marker Prefab or Map Display is missing!"
            );

            return;
        }

        foreach (TargetDummy target in targets)
        {
            if (target == null)
                continue;

            RectTransform marker =
                Instantiate(
                    targetMarkerPrefab,
                    mapDisplay
                );

            marker.gameObject.SetActive(true);

            targetMarkers.Add(marker);
        }
    }

    // =====================================================
    // PLAYER MARKER
    // =====================================================

    private void UpdatePlayerMarker()
    {
        if (player == null ||
            playerMarker == null ||
            mapDisplay == null ||
            miniMapCamera == null)
            return;

        playerMarker.anchoredPosition =
            WorldToMapPosition(player.position);
    }

    // =====================================================
    // TARGET MARKERS
    // =====================================================

    private void UpdateTargetMarkers()
    {
        if (targets == null ||
            miniMapCamera == null)
            return;

        for (int i = 0; i < targets.Length; i++)
        {
            if (i >= targetMarkers.Count)
                break;

            if (targets[i] == null)
                continue;

            RectTransform marker =
                targetMarkers[i];

            marker.anchoredPosition =
                WorldToMapPosition(
                    targets[i].transform.position
                );

            marker.gameObject.SetActive(
                targets[i].gameObject.activeSelf
            );
        }
    }

    // =====================================================
    // WORLD → MINIMAP
    // =====================================================

    private Vector2 WorldToMapPosition(
        Vector3 worldPosition
    )
    {
        Vector3 viewportPosition =
            miniMapCamera.WorldToViewportPoint(
                worldPosition
            );

        float mapX =
            (viewportPosition.x - 0.5f) *
            mapDisplay.rect.width;

        float mapY =
            (viewportPosition.y - 0.5f) *
            mapDisplay.rect.height;

        return new Vector2(
            mapX,
            mapY
        );
    }
}