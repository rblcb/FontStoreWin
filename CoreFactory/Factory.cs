﻿using Protocol;
using Storage;

namespace Core {
  public static class Factory {
    public static IConnection InitializeServerConnection(IFontStorage storage) {
      Protocol.Transport.IConnectionTransport transport = new Protocol.Transport.Phoenix.ConnectionTransport();
      return new Protocol.Impl.Connection(transport, storage);
    }

    public static IFontStorage InitializeStorage() {
      return new Storage.Impl.FSFontStorage();
    }
  }
}
