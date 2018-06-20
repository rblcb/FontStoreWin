﻿using System;
using System.IO;
using System.Net;

namespace Protocol.Transport.Http {
  public interface IHttpResponse : IDisposable {
    HttpStatusCode StatusCode { get; }
    Stream ResponseStream { get; }
  }
}
