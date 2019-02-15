using UnityEngine;

public class TunnelGenerator : MonoBehaviour
{
    public int numSegments;

    public float tunnelLength;

    public GameObject tunnelSegmentPrefab;

    private Vector3 tunnelDirection = new Vector3(0, 0, 1);
    private GameObject[] tunnelSegments;

    // Start is called before the first frame update
    void Start()
    {
        SetupTunnelSegments();
    }

    void SetupTunnelSegments()
    {
        tunnelSegments = new GameObject[numSegments];
        for (int i = 0; i < numSegments; i++)
        {
            GameObject tunnelSegment = Instantiate(tunnelSegmentPrefab, transform, true);
            TunnelSegment segment = tunnelSegment.GetComponent<TunnelSegment>();
            segment.startPosition = GetSegmentStart(i);
            segment.endPosition = GetSegmentEnd(i);
            tunnelSegments[i] = tunnelSegment;
            tunnelSegment.SetActive(true);
        }
    }

    private Vector3 GetSegmentStart(int segmentIndex)
    {
        float segmentLength = tunnelLength / numSegments;
        return tunnelDirection * segmentLength * segmentIndex;
    }

    private Vector3 GetSegmentEnd(int segmentIndex)
    {
        float segmentLength = tunnelLength / numSegments;
        return tunnelDirection * segmentLength * (segmentIndex + 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 offset = this.GetComponent<Transform>().position;
        for (int i = 0; i < numSegments; i++)
        {
            Vector3 start = GetSegmentStart(i);
            Vector3 end = GetSegmentEnd(i);
            Gizmos.DrawLine(offset + start, offset + end);
            Gizmos.DrawWireSphere(offset + start, 0.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
