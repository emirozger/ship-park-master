using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Route : MonoBehaviour
{
    [HideInInspector] public bool isActive = true;
    [HideInInspector] public Vector3[] linePoints;
    public float maxLineLength;
    [SerializeField] private LinesDrawer linesDrawer;
    public Line line;
    public Park park;
    public Ship ship;

    [Space]
    [Header("Colors: ")]
    public Color shipColor;
    [SerializeField] private Color lineColor;

    private void Start()
    {
        linesDrawer = FindObjectOfType<LinesDrawer>();
        linesDrawer.OnParkLinkedToLine += OnParkLinkedToLineHandler;

    }

    private void OnParkLinkedToLineHandler(Route route, List<Vector3> points)
    {
        if (route == this)
        {
            linePoints = points.ToArray();
            GameManager.Instance.RegisterRoute(this);
        }
    }

    public void Disactivate()
    {
        isActive = false;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying && line != null && ship != null & park != null)
        {
            line.lineRenderer.SetPosition(0, ship.bottomTransform.position);
            line.lineRenderer.SetPosition(1, park.transform.position);
            ship.SetColor(shipColor);
            line.SetColor(lineColor);
            park.SetColor(shipColor);
        }
    }
#endif
}