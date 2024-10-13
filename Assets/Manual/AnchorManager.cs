using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnchorManager : MonoBehaviour {
  private static AnchorManager _instance;
  [SerializeField] private GameObject placementPrefab;
  private HashSet<Guid> _anchorUuids = new();
  private readonly string _savePath = Application.persistentDataPath + "/Anchors.txt";
  private Logger _logger;

  private void Awake() {
    if (_instance == null) {
      _instance = this;
      _logger = GetComponent<Logger>();
      LoadAnchorsFromFile();
    } else {
      Destroy(this);
    }
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

  private void OnApplicationQuit() {
    SaveAnchorsToFile();
  }

  private async void SaveAnchorsToFile() {
    _logger.Log($"Saving {_anchorUuids.Count} anchors to file...");
    await System.IO.File.WriteAllTextAsync(_savePath, string.Join(",", _anchorUuids));
    for (var i = 0; i < _anchorUuids.Count; i++) {
      _logger.Log($"Saved anchor {i} ({_anchorUuids.ElementAt(i)}).");
    }
  }

  void Update() {
    if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) AddAnchor();
    if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger)) LoadAnchorsFromFile();
    if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger)) SaveAnchorsToFile();
    if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) ClearSavedAnchors();
  }

  private async void ClearSavedAnchors() {
    // Remove from ovr
    var result = await OVRSpatialAnchor.EraseAnchorsAsync(null, _anchorUuids);
    if (!result.Success) {
      _logger.Log($"Failed to erase anchors with error {result.Status}");
      return;
    }
    
    
    // Remove from list and local storag
    _anchorUuids.Clear();
    SaveAnchorsToFile();
    
    // Delete gameobjects
    foreach (var anchor in FindObjectsOfType<OVRSpatialAnchor>()) {
      Destroy(anchor.gameObject);
    }
    
    
    _logger.Log("Cleared saved anchors.");
  }

  private void AddAnchor() {
    var go = Instantiate(
      placementPrefab,
      OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),
      OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch)
    );
    SetupAnchorAsync(go.AddComponent<OVRSpatialAnchor>(), saveAnchor: true);
  }

  private async void SetupAnchorAsync(OVRSpatialAnchor anchor, bool saveAnchor) {
    // Keep checking for a valid and localized anchor state
    if (!await anchor.WhenLocalizedAsync()) {
      _logger.Log($"Unable to create anchor.");
      Destroy(anchor.gameObject);
      return;
    }

    _logger.Log($"Creating anchor at {anchor.transform.position}.");

    if (!saveAnchor) return;
    var result = await anchor.SaveAnchorAsync();
    if (result.Success) {
      _logger.Log($"Anchor {anchor.Uuid} saved successfully.");
      _anchorUuids.Add(anchor.Uuid);
    } else {
      _logger.Log($"Anchor {anchor.Uuid} failed to save with error {result.Status}");
    }

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
  }
}