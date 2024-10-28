using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TestOvrAnchor : MonoBehaviour {
  
  public GameObject cubePrefab;
  
  async void Start() {
    Debug.Log("Fetching anchors...");
    var anchors = new List<OVRAnchor>();
    var result = await OVRAnchor.FetchAnchorsAsync(
      anchors,
      new OVRAnchor.FetchOptions {
        ComponentTypes = new[] {
          typeof(OVRTriangleMesh),
        },
      }
    );
    

    // no rooms - call Space Setup or check Scene permission
    if (!result.Success || anchors.Count == 0) {
      Debug.LogError("No rooms found. Please call Space Setup or check Scene permission.");
      // Create a cube and set the material to red
      SpawnCube(new Vector3(-1, 0, 0), Color.red);
      return;
    }
    SpawnCube(new Vector3(1, 0, 0), Color.yellow);
    

    Debug.Log($"Found {anchors.Count} rooms");
    // get the component to access its data
    foreach (var room in anchors) {
      if (!room.TryGetComponent(out OVRTriangleMesh bounded3D))
        continue;
      SpawnCube(new Vector3(1.5f, 0, 0), Color.green);
      // Get counts
      if (!bounded3D.TryGetCounts(out var vertexCount, out var triangleCount)) {
        Debug.LogError("Failed to get counts");
        return;
      }
      // Plot triangles
      var vertices = new NativeArray<Vector3>(vertexCount, Allocator.Temp);
      var indices = new NativeArray<int>(triangleCount * 3, Allocator.Temp);
      if (!bounded3D.TryGetMeshRawUntransformed(vertices, indices)) {
        Debug.LogError("Failed to get mesh");
        return;
      }
      // Render the mesh using unity
      var mesh = new Mesh();
      mesh.SetVertices(vertices);
      mesh.SetIndices(indices, MeshTopology.Triangles, 0);
      mesh.RecalculateNormals();
      var meshFilter = gameObject.AddComponent<MeshFilter>();
      meshFilter.mesh = mesh;
      var meshRenderer = gameObject.AddComponent<MeshRenderer>();
      meshRenderer.material.color = Color.blue;
    }
  }

  private void SpawnCube(Vector3 position, Color color) {
    var cube3 = Instantiate(cubePrefab);
    cube3.GetComponent<Renderer>().material.color = color;
    cube3.transform.position = position;
    cube3.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
  }

  // Update is called once per frame
  void Update() {
  }
}