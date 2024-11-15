using System.Linq;
using UnityEngine;

public class RootInputManager: MonoBehaviour {
  private readonly OVRInput.Button[] _buttons = {
    OVRInput.Button.PrimaryIndexTrigger,
    OVRInput.Button.PrimaryHandTrigger,
    OVRInput.Button.SecondaryHandTrigger,
    OVRInput.Button.SecondaryIndexTrigger,
    OVRInput.Button.One,
    OVRInput.Button.Two,
    OVRInput.Button.Three,
    OVRInput.Button.Four,
    OVRInput.Button.PrimaryThumbstick,
    OVRInput.Button.SecondaryThumbstick,
  };
  [SerializeField] private Logger logger;
  
  [SerializeField] public InputManager rootInputManager;

  private void Start() {
    rootInputManager.Activate();
  }

  private void Update() {
    _buttons.Where(button => OVRInput.GetDown(button)).ToList().ForEach(button => {
      logger.Log($"Root: Button {button} pressed.");
      rootInputManager.OnKey(button);
    });
  }
}
