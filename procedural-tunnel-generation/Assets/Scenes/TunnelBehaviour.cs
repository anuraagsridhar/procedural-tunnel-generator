using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelBehaviour : MonoBehaviour
{
    public Camera cameraObject;

    public GameObject tunnelSegmentObject;

    Mesh mesh;

    /*
     * Number of segments to render along the length of the tunnel.
     */   
    public int numSegments;

    /**
     * Number of sectors to render around the tunnel.
     */
    public int numSectors;

    public float length;

    public float radius;

    public int numTunnelSegments;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numTunnelSegments; i++)
        {
            GameObject tunnelSegmentClone = (GameObject)Instantiate(tunnelSegmentObject);
            MeshFilter meshFilter = tunnelSegmentClone.GetComponent<MeshFilter>();
            mesh = new Mesh();
            if (meshFilter != null)
            {
                print("Mesh filter found");
                meshFilter.mesh = mesh;
            }

            Vector3[] vertices = new Vector3[(numSegments + 1) * numSectors];
            int vertexIndex = 0;
            float sectorAngle = (2 * Mathf.PI) / numSectors;
            for (int r = 0; r < (numSegments + 1); r++)
            {
                float z = ((float)r / (float)numSegments) * length;
                for (int c = 0; c < numSectors; c++)
                {
                    float theta = sectorAngle * c;
                    float x = radius * Mathf.Cos(theta);
                    float y = radius * Mathf.Sin(theta);
                    vertices[vertexIndex] = new Vector3(x, y, z);
                    vertexIndex++;
                }
            }
            mesh.vertices = vertices;

            int[] triangles = new int[numSegments * numSectors * 6];
            int triangleIndex = 0;
            for (int r = 0; r < numSegments; r++)
            {
                for (int c = 0; c < numSectors; c++)
                {
                    triangles[triangleIndex++] = r * numSectors + (c % numSectors);
                    triangles[triangleIndex++] = (r + 1) * numSectors + (c % numSectors);
                    triangles[triangleIndex++] = r * numSectors + ((c + 1) % numSectors);

                    triangles[triangleIndex++] = r * numSectors + ((c + 1) % numSectors);
                    triangles[triangleIndex++] = (r + 1) * numSectors + (c % numSectors);
                    triangles[triangleIndex++] = (r + 1) * numSectors + ((c + 1) % numSectors);
                }
            }
            mesh.triangles = triangles;
            tunnelSegmentClone.transform.SetParent(this.transform);
            tunnelSegmentClone.transform.position = new Vector3(0, 0, 0);
            tunnelSegmentClone.transform.Translate(new Vector3(0, 0, i * length));
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}
