using System;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour {
  public TextMeshProUGUI logText;
  private string _logString = "";

  private static Logger _instance;

  private void Awake() {
    if (_instance == null) {
      _instance = this;
    } else {
      Destroy(this);
    }
  }
  
  public void Log(string message) {
    _logString += message + "\n";
    if (logText) logText.text = _logString;
  }

  public void Clear() {
    _logString = "";
    if (logText) logText.text = _logString;
  }
}