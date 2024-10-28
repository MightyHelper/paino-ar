using System;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour {
  public TextMeshProUGUI logText;
  private string _logString = "";

  private static Logger Instance;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(this);
    }
  }
  
  public void Log(string message) {
    _logString += message + "\n";
    
    if (logText != null) {
      logText.text = _logString;
    }
  }
}