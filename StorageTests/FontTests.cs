﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Data;
using Storage.Impl.Tests.Utilities;
using TestUtilities;

namespace Storage.Impl.Tests {
  [TestClass]
  public class FontTests {
    [TestMethod]
    [TestCategory("Font.Events")]
    public void RequestActivation_shouldTriggerActivationRequestEvent() {
      Font font = Factory.CreateFont(TestData.Font1_Description);

      bool eventTriggered = false;
      font.OnActivationRequest += delegate {
        eventTriggered = true;
      };

      font.RequestActivation();
      Assert.IsTrue(eventTriggered, "Font.RequestActivation should trigger font activation requested event");
    }

    [TestMethod]
    [TestCategory("Font.Events")]
    public void RequestDeactivation_shouldTriggerDeactivationRequestEvent() {
      Font font = Factory.CreateFont(TestData.Font1_Description);

      bool eventTriggered = false;
      font.OnDeactivationRequest += delegate {
        eventTriggered = true;
      };

      font.RequestDeactivation();
      Assert.IsTrue(eventTriggered, "Font.RequestDeactivation should trigger font deactivation requested event");
    }
  }
}
