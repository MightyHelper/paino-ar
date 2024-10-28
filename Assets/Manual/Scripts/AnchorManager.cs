using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class AnchorManager : MonoBehaviour {
  [SerializeField] private GameObject placementPrefab;
  private HashSet<Guid> _anchorUuids = new();
  private Dictionary<Guid, GameObject> _anchorGameObjects = new();
  private string _savePath;
  protected Logger _logger;
  private GameObject _preview;

  protected virtual void Awake() {
    _savePath = Application.persistentDataPath + "/Anchors.txt";
    _logger = GetComponent<Logger>();
    LoadAnchorsFromFile();
    _preview = Instantiate(placementPrefab);
    OnNewItem(_preview);
  }

  private async void LoadAnchorsFromFile() {
    _logger.Log("Loading anchors...");
    if (!System.IO.File.Exists(_savePath)) {
      _logger.Log("No anchor file found.");
      return;
    }

    _logger.Log("Loading anchors from file...");
    var text = await System.IO.File.ReadAllTextAsync(_savePath);
    _anchorUuids = new HashSet<Guid>(text.Split(',').Select(Guid.Parse));
    _logger.Log($"Loaded {_anchorUuids.Count} anchors.");
    RenderLoadedAnchors();
  }

  protected virtual void OnApplicationQuit() {
    SaveAnchorsToFile();
  }

  private async void SaveAnchorsToFile() {
    _logger.Log($"Saving {_anchorUuids.Count} anchors to file...");
    await System.IO.File.WriteAllTextAsync(_savePath, string.Join(",", _anchorUuids));
    for (var i = 0; i < _anchorUuids.Count; i++) {
      _logger.Log($"Saved anchor {i} ({_anchorUuids.ElementAt(i)}).");
    }
  }

  protected virtual void Update() {
    try {
      PreviewAnchor();      
      if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) AddAnchor();
      if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger)) LoadAnchorsFromFile();
      if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger)) SaveAnchorsToFile();
      if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) ClearSavedAnchors();
    } catch (Exception e) {
      _logger.Log($"Error: {e.Message}");
    }
  }
  
  private void PreviewAnchor() {
    if (!_preview) return;
    _preview.transform.position = PlacePosition;
    _preview.transform.rotation = PlaceRotation;
  }

  private async void ClearSavedAnchors() {
    if (await RemoveOvrAnchors()) return;
    ClearAnchorCaches();
    RemoveGameObjects();
    _logger.Log("Cleared saved anchors.");
  }

  private void RemoveGameObjects() {
    FindObjectsByType<OVRSpatialAnchor>(FindObjectsSortMode.None).ToList()
      .ForEach(anchor => OnItemRemoved(anchor.gameObject));
  }

  private void ClearAnchorCaches() {
    _anchorUuids.Clear();
    SaveAnchorsToFile();
  }

  private async Task<bool> RemoveOvrAnchors() {
    var result = await OVRSpatialAnchor.EraseAnchorsAsync(null, _anchorUuids);

    if (result.Success) {
      _logger.Log("Erased anchors.");
      return false;
    }
    _logger.Log($"Failed to erase anchors with error {result.Status}");
    return true;
  }

  private async void AddAnchor() {
    var go = Instantiate(
      placementPrefab,
      PlacePosition,
      PlaceRotation
    );
    await SetupAnchorAsync(go.AddComponent<OVRSpatialAnchor>(), saveAnchor: true);
    OnNewItem(go);
  }

  private static Quaternion PlaceRotation => OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
  private static Vector3 PlacePosition => OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

  private async Task SetupAnchorAsync(OVRSpatialAnchor anchor, bool saveAnchor) {
    if (!await anchor.WhenLocalizedAsync()) {
      _logger.Log($"Unable to create anchor.");
      Destroy(anchor.gameObject);
      return;
    }
    if (!saveAnchor) return;
    var result = await anchor.SaveAnchorAsync();
    if (!result.Success) {
      _logger.Log($"Anchor {anchor.Uuid} failed to save with error {result.Status}");
      return;
    }

    _anchorUuids.Add(anchor.Uuid);
  }

  /******************* Load Anchor Methods **********************/
  public async void RenderLoadedAnchors() {
    // Load and localize
    var unboundAnchors = new List<OVRSpatialAnchor.UnboundAnchor>();
    for (var i = 0; i < _anchorUuids.Count; i++) {
      _logger.Log($"Loading anchor {i} ({_anchorUuids.ElementAt(i)})...");
    }

    var result = await OVRSpatialAnchor.LoadUnboundAnchorsAsync(_anchorUuids, unboundAnchors);

    if (result.Success) {
      foreach (var anchor in unboundAnchors) {
        var localizationResult = await anchor.LocalizeAsync();
        _logger.Log($"Localized anchor {anchor.Uuid} with result {localizationResult}.");
        OnLocalized(localizationResult, anchor);
      }
    } else {
      _logger.Log($"Load anchors failed with {result.Status}.");
    }
  }

  private void OnLocalized(bool success, OVRSpatialAnchor.UnboundAnchor unboundAnchor) {
    _logger.Log($"Localized anchor! {success}");
    if (!unboundAnchor.TryGetPose(out var pose)) return;
    _logger.Log($"Placing anchor at {pose.position}.");
    var go = Instantiate(placementPrefab, pose.position, pose.rotation);
    var anchor = go.AddComponent<OVRSpatialAnchor>();
    unboundAnchor.BindTo(anchor);
    OnNewItem(go);
  }

  protected virtual void OnNewItem(GameObject go) {
    go.TryGetComponent<OVRSpatialAnchor>(out var anchor);
    if (!anchor) return;
    _anchorGameObjects.Add(anchor.Uuid, go);
  }

  protected virtual void OnItemRemoved(GameObject go) {
    go.TryGetComponent<OVRSpatialAnchor>(out var anchor);
    Destroy(go);
    if (!anchor) return;
    _anchorGameObjects.Remove(anchor.Uuid);
  }
}