public class VerbsSession : BaseSession
{
    public VerbsSession(DataSession sessionData) : base(sessionData) {}
    public override void Start() { }
    public override void DisplayAfterSession() { }
    public override void DisplayBeforeSession() { base.DisplayBeforeSession(); }
    public override void DisplayStatusFor(string logs) { throw new NotImplementedException(); }
}
