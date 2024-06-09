using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ViewRadiusVisualizer : MonoBehaviour
{
    public float radius = 10f;
    public int segments = 360;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        DrawCircle();
    }

    void DrawCircle()
    {
        lineRenderer.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * 360f / segments;
            float radians = angle * Mathf.Deg2Rad;
            lineRenderer.SetPosition(i, new Vector3(Mathf.Cos(radians) * radius, Mathf.Sin(radians) * radius, 0));
        }
    }
}