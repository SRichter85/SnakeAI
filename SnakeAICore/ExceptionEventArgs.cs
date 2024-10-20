namespace SnakeAICore;

public class ExceptionEventArgs : EventArgs {
    public ExceptionEventArgs(Exception ex) { Exception = ex; }
    public Exception Exception {get;}
}
