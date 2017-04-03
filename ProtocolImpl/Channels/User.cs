﻿using Newtonsoft.Json;
using Protocol.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Impl.Channels {
  class User {
    #region private data
    private IConnection _connection;
    private IBroadcastChannel _underlying;
    #endregion

    #region ctor
    public User(IConnection connection) {
      _connection = connection;
      _underlying = connection.Transport.Channel(string.Format("users:{0}", connection.UserData.UID));
    }
    #endregion

    #region methods
    public IBroadcastChannelResult Join() {
      _underlying.On("font:activation", (Payloads.FontId fid) => {
        OnFontActivation?.Invoke(fid.UID);
      });
      _underlying.On("font:deactivation", (Payloads.FontId fid) => {
        OnFontDeactivation?.Invoke(fid.UID);
      });
      _underlying.On("update:complete", () => {
        OnUpdateComplete?.Invoke();
      });

      return _underlying.Join();
    }

    public IBroadcastChannelResult Leave() {
      _underlying.Off("font:activation");
      _underlying.Off("font:deactivation");
      _underlying.Off("udpate:complete");
      return _underlying.Leave();
    }

    public void SendDisconnect(string reason) {
      _underlying.Send("disconnect", JsonConvert.SerializeObject(new Payloads.Disconnect {
        Reason = reason
      }));
    }

    public void SendUpdateRequest() {
      _underlying.Send("update:request", null);
    }

    public void SendFontInstallationReport(string uid, bool succeed) {
      string evt = succeed ? "font:installation-success" : "font:installation-failure";
      _underlying.Send(evt, JsonConvert.SerializeObject(new Payloads.FontId() {
        UID = uid
      }));
    }

    public void SendFontUninstallationReport(string uid, bool succeed) {
      string evt = succeed ? "font:uninstallation-success" : "font:uninstallation-failure";
      _underlying.Send(evt, JsonConvert.SerializeObject(new Payloads.FontId() {
        UID = uid
      }));
    }

    public void SendUpdateComplete() {
      _underlying.Send("ready", null);
    }
    #endregion

    #region delegates
    public delegate void FontActivationHandler(string uid);
    public delegate void FontDeactivationHandler(string uid);
    public delegate void UpdateCompleteHandler();
    #endregion

    #region events
    public event FontActivationHandler OnFontActivation;
    public event FontDeactivationHandler OnFontDeactivation;
    public event UpdateCompleteHandler OnUpdateComplete;
    #endregion
  }
}
