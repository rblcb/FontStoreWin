﻿namespace Protocol.Impl.States {
  class Running : ConnectionState {
    #region ctor
    public Running(Connection connection): this("Running", connection) {
    }

    private Running(string name, Connection connection) : base(name, connection) {
    }
    #endregion

    #region methods
    public override void Abort() {
      Stop();
    }

    public override void Stop() {
      _context.CatalogChannel.OnFontDescription -= CatalogChannel_OnFontDescription;
      _context.CatalogChannel.OnFontDeletion -= CatalogChannel_OnFontDeletion;
      _context.UserChannel.OnFontActivation -= UserChannel_OnFontActivation;
      _context.UserChannel.OnFontDeactivation -= UserChannel_OnFontDeactivation;
    }

    protected override void Start() {
      _context.CatalogChannel.OnFontDescription += CatalogChannel_OnFontDescription;
      _context.CatalogChannel.OnFontDeletion += CatalogChannel_OnFontDeletion;
      _context.UserChannel.OnFontActivation += UserChannel_OnFontActivation;
      _context.UserChannel.OnFontDeactivation += UserChannel_OnFontDeactivation;

      WillTransition = true; // This state is supposed to change at any moment

      _context.TriggerUpdateFinished();
    }

    //public override bool CanTransitionTo<Disconnecting>() {
    //  return true;
    //}
    #endregion

    #region event handling
    private void UserChannel_OnFontDeactivation(string uid) {
      _context.Storage.DeactivateFont(uid);
    }

    private void UserChannel_OnFontActivation(string uid) {
      _context.Storage.ActivateFont(uid);
    }

    private void CatalogChannel_OnFontDeletion(string uid) {
      _context.Storage.RemoveFont(uid);
    }

    private void CatalogChannel_OnFontDescription(Payloads.FontDescription desc) {
      _context.Storage.AddFont(desc);
    }
    #endregion
  }
}
