﻿using Protocol.Payloads;
using Protocol.Transport;
using System;

namespace Protocol {
  public abstract class AConnection: IConnection {
    #region properties
    public IConnectionTransport Transport { get; private set; }
    public UserData UserData { get; protected set; }

    public TimeSpan AuthenticationRetryInterval { get; protected set; }
    public TimeSpan ConnectionRetryInterval { get; protected set; }
    #endregion

    #region ctor
    public AConnection(IConnectionTransport transport) {
      Transport = transport;
    }
    #endregion

    #region methods
    public abstract void Connect(string email, string password);
    public abstract void Disconnect();
    #endregion

    #region IConnectionObservable
    public abstract event ConnectionEstablishedHandler OnEstablished;
    public abstract event ConnectionValidationFailedHandler OnValidationFailure;
    #endregion
  }
}
