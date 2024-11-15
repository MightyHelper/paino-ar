namespace WS {
  public class KeyEvent {
    public int Key;
    public float Start;
    public bool Done;
	
    public KeyEvent(int key, float start) {
      Key = key;
      Start = start;
    }
  }
}