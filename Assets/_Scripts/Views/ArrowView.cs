using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ArrowView : MonoBehaviour
{
    public GameObject ArrowHeadPrefab;
    public GameObject ArrowBodyPrefab;
    public int arrowNodeNum;
    public float scaleFactor = 1f;
    public float bodyScaleFactor = 1f;
    private RectTransform origin;
    private List<RectTransform> arrowNodes = new();
    private List<Vector2> controlPoints = new();
    private readonly List<Vector2> controlPointFactors = new() { new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f)};

    public void SetupArrow(Vector3 startPosition)
    {
        transform.position = startPosition;
    }

    private void Awake()
    {
        this.origin = this.GetComponent<RectTransform>();

        for(int i = 0; i < this.arrowNodeNum; ++i)
        {
            this.arrowNodes.Add(Instantiate(this.ArrowBodyPrefab, this.transform).GetComponent<RectTransform>());
        }

        this.arrowNodes.Add(Instantiate(this.ArrowHeadPrefab, this.transform).GetComponent<RectTransform>());

        this.arrowNodes.ForEach(node => node.GetComponent<RectTransform>().position = new Vector2(-1000, -1000));

        for(int i = 0; i < 4; ++i)
        {
            this.controlPoints.Add(Vector2.zero);
        }
    }

    private void Update()
    {
        this.controlPoints[0] = new Vector2(this.origin.position.x, this.origin.position.y);

        this.controlPoints[3] = new Vector2(MouseUtils.GetMousePositionInWorldSpace().x, MouseUtils.GetMousePositionInWorldSpace().y);

        this.controlPoints[1] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactors[0];
        this.controlPoints[2] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactors[1];

        for(int i = 0; i < this.arrowNodes.Count; ++i)
        {
            var t = Mathf.Log(1f * i / (this.arrowNodes.Count -1) +1f, 2f);

            this.arrowNodes[i].position = Mathf.Pow(1-t, 3) * this.controlPoints[0] + 
                3 * Mathf.Pow(1-t, 2) * t * this.controlPoints[1] + 
                3 * (1-t) * Mathf.Pow(t, 2) * this.controlPoints[2] +
                Mathf.Pow(t, 3) * this.controlPoints[3];

            if(i > 0)
            {
                var euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, this.arrowNodes[i].position - this.arrowNodes[i-1].position));
                this.arrowNodes[i].rotation = Quaternion.Euler(euler);
            }

            bool isHead = i == this.arrowNodes.Count - 1;
            var scale = this.scaleFactor * (1f - 0.03f * (this.arrowNodes.Count - 1 - i));
            if (!isHead) scale *= this.bodyScaleFactor;
            this.arrowNodes[i].localScale = new Vector3(scale, scale, 1f);
        }

        this.arrowNodes[0].transform.rotation = this.arrowNodes[1].transform.rotation;
    }
}
