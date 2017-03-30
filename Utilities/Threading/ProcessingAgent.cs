﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.Threading {
  public class ProcessingAgent {
    #region private data
    private ConcurrentQueue<Action> _commandQueue;
    private int _concurrentFactor;
    private bool _running;
    private CancellationTokenSource _cancelSource;
    private bool _processingStarted;
    #endregion

    #region properties
    public bool Running {
      get {
        return _running;
      }
      protected set {
        if (_running != value) {
          _running = value;

          if (_running) {
            OnProcessingStarted?.Invoke();
          }
          else {
            OnProcessingFinished?.Invoke();
          }
        }
      }
    }
    #endregion

    #region delegates
    public delegate void ProcessingStartedHandler();
    public delegate void ProcessingFinishedHandler();
    #endregion

    #region events
    public event ProcessingStartedHandler OnProcessingStarted;
    public event ProcessingFinishedHandler OnProcessingFinished;
    #endregion

    #region ctor
    public ProcessingAgent(int concurrentFactor, CancellationTokenSource cancelSource) {
      _concurrentFactor = concurrentFactor;
      _running = false;
      _commandQueue = new ConcurrentQueue<Action>();
      _processingStarted = false;

      if (cancelSource != null) {
        _cancelSource = cancelSource;
      } else {
        _cancelSource = new CancellationTokenSource();
      }
    }
    #endregion

    #region methods
    public void Start() {
      _processingStarted = true;
      StartProcessing();
    }

    public void Stop() {
      _processingStarted = false;
    }

    public void Enqueue(Action command) {
      _commandQueue.Enqueue(command);
      StartProcessing();
    }
    #endregion

    #region private methods

    private async void StartProcessing() {
      if (_processingStarted && !Running) {
        await ProcessCommands();
      }
    }


    private async Task ProcessCommands() {
      await Task.Run(delegate {
        while (_processingStarted && !_commandQueue.IsEmpty) {
          Running = true;

          List<Task> batch = new List<Task>();
          for (int it = 0; it < _concurrentFactor && !_commandQueue.IsEmpty; it++) {
            Action action;
            if (_commandQueue.TryDequeue(out action)) {
              batch.Add(Task.Run(action));
            }
          }
          Task.WaitAll(batch.ToArray());
        }
        Running = false;
      });
    }
    #endregion
  }
}
