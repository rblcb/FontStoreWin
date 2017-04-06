﻿namespace Protocol.Impl.States {
  class Reconnecting : ConnectionState {
    #region ctor
    public Reconnecting(Connection connection): base("Reconnecting", connection) {

    }

    private Reconnecting(string name, Connection connection) : base(name, connection) {
    }
    #endregion

    #region methods
    public override void Abort() {
      Stop();
    }

    public override void Stop() {
      _context.Transport.Opened -= Transport_Opened;
    }

    protected override void Start() {
      _context.Transport.Opened += Transport_Opened;
      _context.TransportReconnectionStarted();
    }

    private void Transport_Opened() {
      FSM.State = new Connected(_context, _context.UserData);
    }
    #endregion
  }
}
