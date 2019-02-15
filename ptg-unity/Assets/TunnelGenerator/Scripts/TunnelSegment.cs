using UnityEngine;
using UnityEngine.UI;

public class TunnelSegment : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;

    public int numSectionsPerSegment;

    public int numFacetsPerSection;

    public float tunnelRadius;

    public Toggle wireframeToggle;

    public Material wireframeMaterial;
    public Material rockMaterial;

    private Mesh mesh;

    public void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        if (meshFilter != null)
        {
            meshFilter.mesh = mesh;
        }
        transform.position = new Vector3(0, 0, 0);
        mesh.vertices = GenerateVertices(tunnelRadius, numSectionsPerSegment, numFacetsPerSection);
        mesh.triangles = GenerateTriangles(numSectionsPerSegment, numFacetsPerSection);
        mesh.uv = GenerateUVs(numSectionsPerSegment, numFacetsPerSection);
        GetComponent<MeshRenderer>().material = rockMaterial;
        mesh.RecalculateNormals();

        wireframeToggle.onValueChanged.AddListener(delegate
        {
            if (wireframeToggle.isOn)
            {
                GetComponent<MeshRenderer>().material = wireframeMaterial;
            } else
            {
                GetComponent<MeshRenderer>().material = rockMaterial;
            }
        });
    }

    public Vector3[] GenerateVertices(float radius, int numSections, int numFacets)
    {
        float length = (endPosition - startPosition).magnitude;
        Vector3[] vertices = new Vector3[(numSections + 1) * numFacets];
        int vertexIndex = 0;
        float sectorAngle = (2 * Mathf.PI) / numFacets;
        for (int r = 0; r < (numSections + 1); r++)
        {
            float z = ((float)r / (float)numSections) * length;
            for (int c = 0; c < numFacets; c++)
            {
                float theta = sectorAngle * c;
                float x = radius * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(theta);
                vertices[vertexIndex] = startPosition + new Vector3(x, y, z);
                vertexIndex++;
            }
        }
        return vertices;
    }

    public Vector2[] GenerateUVs(int numSections, int numFacets)
    {
        Vector2[] uvs = new Vector2[(numSections + 1) * numFacets];
        int vertexIndex = 0;
        float sectorAngle = (2 * Mathf.PI) / numFacets;
        for (int r = 0; r < (numSections + 1); r++)
        {
            float u = (float)r / numSections;
            for (int c = 0; c < numFacets; c++)
            {
                float v = (float) c / numFacets;
                uvs[vertexIndex] = new Vector2(u, v);
                vertexIndex++;
            }
        }
        return uvs;
    }

    public int[] GenerateTriangles(int numSections, int numFacets) {
        int[] triangles = new int[numSections * numFacets * 6];
        int triangleIndex = 0;
        for (int r = 0; r < numSections; r++)
        {
            for (int c = 0; c < numFacets; c++)
            {
                triangles[triangleIndex++] = r * numFacets + c;
                triangles[triangleIndex++] = (r + 1) * numFacets + c;
                triangles[triangleIndex++] = r * numFacets + ((c + 1) % numFacets);

                triangles[triangleIndex++] = r * numFacets + ((c + 1) % numFacets);
                triangles[triangleIndex++] = (r + 1) * numFacets + c;
                triangles[triangleIndex++] = (r + 1) * numFacets + ((c + 1) % numFacets);
            }
        }
        return triangles;
    }
}