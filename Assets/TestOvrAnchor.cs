using System.Collections.Generic;
using UnityEngine;

public class TestOvrAnchor : MonoBehaviour {
  
  // The cube prefab to use
  public GameObject cubePrefab;
  
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  async void Start() {
    Debug.Log("Fetching anchors...");
    var anchors = new List<OVRAnchor>();
    var result = await OVRAnchor.FetchAnchorsAsync(
      anchors,
      new OVRAnchor.FetchOptions {
        SingleComponentType = typeof(OVRRoomLayout)
      }
    );
    

    // no rooms - call Space Setup or check Scene permission
    if (!result.Success || anchors.Count == 0) {
      Debug.LogError("No rooms found. Please call Space Setup or check Scene permission.");
      return;
    }

    Debug.Log($"Found {anchors.Count} rooms");
    // get the component to access its data
    foreach (var room in anchors) {
      if (!room.TryGetComponent(out OVRBounded3D bounded3D))
        continue;

      var cube = Instantiate(cubePrefab);
      cube.transform.position = bounded3D.BoundingBox.center;
      cube.transform.localScale = bounded3D.BoundingBox.size;
    }
  }

  // Update is called once per frame
  void Update() {
  }
}