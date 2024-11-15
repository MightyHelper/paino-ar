using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : InputContext {
  public InputManagerContextEntry[] Contexts;
  public OVRInput.Button unContextButton = OVRInput.Button.PrimaryThumbstick;
  private InputContext _currentContext;
  [SerializeField] private Logger logger;

  private void Start() {
    _currentContext = Contexts[0].context;
  }

  public override void Activate() {
    base.Activate();
    logger.Log($"InputManager [{name}]: Activate.");
    Contexts.ToList().ForEach(
      context => {
        logger.Log($"InputManager [{name}]: Context {context.button} {context.context.name}.");
      }
    );
  }

  public override void Deactivate() {
    base.Deactivate();
    logger.Log($"InputManager [{name}]: Deactivate.");
  }

  public override void OnKey(OVRInput.Button button) {
    logger.Log($"InputManager [{name}]: Button {button} pressed.");
    if (button == unContextButton) {
      logger.Log($"Deactivating context, back at {name}.");
      _currentContext.Deactivate();
      Activate();
    }
    logger.Log($"Current context is {_currentContext.name}, active is {_currentContext.IsActive}.");
    if (_currentContext.IsActive) {
      _currentContext.OnKey(button);
    } else {
      var inputContext = Contexts.FirstOrDefault(context => context.button == button);
      if (inputContext == null) return;
      logger.Clear();
      logger.Log($"Switching to {inputContext.context.name}.");
      _currentContext = inputContext.context;
      _currentContext.Activate();
    }
  }
}