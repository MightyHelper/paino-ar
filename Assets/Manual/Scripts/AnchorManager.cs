using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class AnchorManager : InputContext {
  [SerializeField] private GameObject placementPrefab;
  [SerializeField] public Logger logger;
  private HashSet<Guid> _anchorUuids = new();
  private Dictionary<Guid, GameObject> _anchorGameObjects = new();
  private string _savePath;
  [CanBeNull] private GameObject _preview;

  protected virtual void Awake() {
    _savePath = Application.persistentDataPath + "/Anchors.txt";
    LoadAnchorsFromFile();
    _preview = Instantiate(placementPrefab);
  }
  private void Start() {
    OnNewItem(_preview);
  }

  private async void LoadAnchorsFromFile() {
    logger.Log("Loading anchors...");
    if (!System.IO.File.Exists(_savePath)) {
      logger.Log("No anchor file found.");
      return;
    }

    logger.Log("Loading anchors from file...");
    var text = await System.IO.File.ReadAllTextAsync(_savePath);
    _anchorUuids = new HashSet<Guid>(text.Split(',').Select(Guid.Parse));
    logger.Log($"Loaded {_anchorUuids.Count} anchors.");
    RenderLoadedAnchors();
  }

  protected virtual void OnApplicationQuit() {
    SaveAnchorsToFile();
  }

  private async void SaveAnchorsToFile() {
    logger.Log($"Saving {_anchorUuids.Count} anchors to file...");
    await System.IO.File.WriteAllTextAsync(_savePath, string.Join(",", _anchorUuids));
    for (var i = 0; i < _anchorUuids.Count; i++) {
      logger.Log($"Saved anchor {i} ({_anchorUuids.ElementAt(i)}).");
    }
  }

  protected void Update() {
    try {
      if (IsActive) PreviewAnchor();
      _preview?.SetActive(IsActive);
    } catch (Exception e) {
      logger.Log($"Error: {e.Message}");
    }
  }

  public override void Activate() {
    base.Activate();
    logger.Log("AnchorManager: Activate.");
  }

  public override void Deactivate() {
    base.Deactivate();
    logger.Log("AnchorManager: Deactivate.");
  }

  public override void OnKey(OVRInput.Button button) {
    logger.Log($"AnchorManager: Button {button} pressed.");
    switch (button) {
      case OVRInput.Button.PrimaryIndexTrigger:
        AddAnchor();
        break;
      case OVRInput.Button.PrimaryHandTrigger:
        LoadAnchorsFromFile();
        break;
      case OVRInput.Button.SecondaryHandTrigger:
        SaveAnchorsToFile();
        break;
      case OVRInput.Button.SecondaryIndexTrigger:
        ClearSavedAnchors();
        break;
    }
  }

  private void PreviewAnchor() {
    if (!_preview) return;
    _preview.transform.position = PlacePosition;
    _preview.transform.rotation = PlaceRotation;
  }

  private async void ClearSavedAnchors() {
    logger.Clear();
    if (await RemoveOvrAnchors()) return;
    ClearAnchorCaches();
    RemoveGameObjects();
    logger.Log("Cleared saved anchors.");
  }

  private void RemoveGameObjects() {
    FindObjectsByType<OVRSpatialAnchor>(FindObjectsSortMode.None).ToList()
      .ForEach(anchor => OnItemRemoved(anchor.gameObject));
  }

  private void ClearAnchorCaches() {
    _anchorUuids.Clear();
    SaveAnchorsToFile();
  }

  public static List<List<T>> Batch<T>(List<T> source, int batchSize) {
    var batches = new List<List<T>>();

    for (var i = 0; i < source.Count; i += batchSize) {
      batches.Add(source.GetRange(i, Math.Min(batchSize, source.Count - i)));
    }

    return batches;
  }

  private async Task<bool> RemoveOvrAnchors() {
    logger.Log("Erasing anchors...");
    // For some reason EraseAnchorsAsync doesn't work if we have more than 32 anchors, so we need to batch
    var batches = Batch(_anchorUuids.ToList(), 32);
    foreach (var batch in batches) {
      var result = await OVRSpatialAnchor.EraseAnchorsAsync(null, batch);
      logger.Log($"Erasing {batch.Count} anchors... {result.Status} {result.Success}");
      if (result.Success) {
        logger.Log($"Erased {batch.Count} anchors.");
      } else {
        logger.Log($"Failed to erase anchors with error {result.Status}");
        return true;
      }
    }
    return false;
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
      logger.Log($"Unable to create anchor.");
      Destroy(anchor.gameObject);
      return;
    }

    if (!saveAnchor) return;
    var result = await anchor.SaveAnchorAsync();
    if (!result.Success) {
      logger.Log($"Anchor {anchor.Uuid} failed to save with error {result.Status}");
      return;
    }

    _anchorUuids.Add(anchor.Uuid);
  }

  /******************* Load Anchor Methods **********************/
  public async void RenderLoadedAnchors() {
    // Load and localize
    var unboundAnchors = new List<OVRSpatialAnchor.UnboundAnchor>();
    for (var i = 0; i < _anchorUuids.Count; i++) {
      logger.Log($"Loading anchor {i} ({_anchorUuids.ElementAt(i)})...");
    }

    var result = await OVRSpatialAnchor.LoadUnboundAnchorsAsync(_anchorUuids, unboundAnchors);

    if (result.Success) {
      foreach (var anchor in unboundAnchors) {
        var localizationResult = await anchor.LocalizeAsync();
        logger.Log($"Localized anchor {anchor.Uuid} with result {localizationResult}.");
        OnLocalized(localizationResult, anchor);
      }
    } else {
      logger.Log($"Load anchors failed with {result.Status}.");
    }
  }

  private void OnLocalized(bool success, OVRSpatialAnchor.UnboundAnchor unboundAnchor) {
    logger.Log($"Localized anchor! {success}");
    if (!unboundAnchor.TryGetPose(out var pose)) return;
    logger.Log($"Placing anchor at {pose.position}.");
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