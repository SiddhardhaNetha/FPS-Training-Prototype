using UnityEngine;

public class MiniMapPlayerMarker : MonoBehaviour
{
    public Transform player;
    public RectTransform mapArea;
    public RectTransform marker;

    public Vector2 mapWorldMin;
    public Vector2 mapWorldMax;

    private void Update()
    {
        if (player == null || mapArea == null || marker == null)
            return;

        float x = Mathf.InverseLerp(
            mapWorldMin.x,
            mapWorldMax.x,
            player.position.x
        );

        float y = Mathf.InverseLerp(
            mapWorldMin.y,
            mapWorldMax.y,
            player.position.z
        );

        float localX = (x - 0.5f) * mapArea.rect.width;
        float localY = (y - 0.5f) * mapArea.rect.height;

        marker.anchoredPosition = new Vector2(
            localX,
            localY
        );
    }
}