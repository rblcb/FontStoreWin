﻿using Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using UI.Utilities;

namespace UI.Views {
  /// <summary>
  /// Interaction logic for FontList.xaml
  /// </summary>
  public partial class FontList : Window, IView {
    #region data
    private Protocol.Payloads.UserData _userData;
    private IStorage _storage;

    private ViewModels.FamilyCollectionVM _collectionVM;
    #endregion

    #region properties
    public UIElement DragHandle {
      get {
        if (IsInitialized) {
          return HeaderGrid;
        }
        return null;
      }
    }

    public IStorage Storage { get; set; }
    #endregion

    #region events
    public event OnExitHandler OnExit;
    public event OnLogoutHandler OnLogout;
    public event OnAboutClickedHandler OnAboutClicked;
    #endregion

    #region ctor
    public FontList(Protocol.Payloads.UserData userData) {
      _userData = userData;

      InitializeComponent();

      NameLabel.Content = string.Format("{0} {1}", _userData.FirstName, _userData.LastName);
    }
    #endregion

    #region methods
    public void InvokeOnUIThread(Action action) {
      try {
        Dispatcher.Invoke(action);
      }
      catch (Exception) { }
    }

    public void UpdateCounters() {
      AllCountLabel.Content = string.Format("({0})", Storage.FamilyCollection.Families.Count);
      NewCountLabel.Content = string.Format("({0})", Storage.NewFamilies.Count);
      InstalledCountLabel.Content = string.Format("({0})", Storage.ActivatedFamilies.Count);
    }

    public void LoadingState(bool isLoading) {
      if (isLoading) {
        Loader.Visibility = Visibility.Visible;
        InstalledCountLabel.Visibility = Visibility.Collapsed;
        NewCountLabel.Visibility = Visibility.Collapsed;
        AllCountLabel.Visibility = Visibility.Collapsed;

        FamilyTree.Visibility = Visibility.Hidden;
        FamilyTree.ItemsSource = null;
      }
      else {
        Loader.Visibility = Visibility.Collapsed;
        InstalledCountLabel.Visibility = Visibility.Visible;
        NewCountLabel.Visibility = Visibility.Visible;
        AllCountLabel.Visibility = Visibility.Visible;

        UpdateCounters();

        _collectionVM = new ViewModels.FamilyCollectionVM(Storage.FamilyCollection);
        FamilyTree.ItemsSource = _collectionVM.Families;
        FamilyTree.Visibility = Visibility.Visible;
      }
    }

    public void Terminated(string message) {
      MessageBox.Show(this, message, "Fontstore - Connection closed", MessageBoxButton.OK);
    }

    public void Disconnected() {
      Loader.Visibility = Visibility.Visible;
      if (MessageBox.Show(this, "The application has been disconnected.\nFontstore will try to reconnect automatically.",
                          "Fontstore - Connection lost",
                          MessageBoxButton.OKCancel) == MessageBoxResult.Cancel) {
        OnLogout?.Invoke();
      }
    }
    #endregion

    #region UI event handling
    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      MenuButton.ContextMenu.IsEnabled = true;
      MenuButton.ContextMenu.PlacementTarget = MenuButton;
      MenuButton.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
      MenuButton.ContextMenu.IsOpen = true;
    }

    private void Account_Click(object sender, RoutedEventArgs e) {
      ViewsUtility.NavigateToUri(new Uri(_userData.Urls.Account));
      e.Handled = true;
    }

    private void Settings_Click(object sender, RoutedEventArgs e) {
      ViewsUtility.NavigateToUri(new Uri(_userData.Urls.Settings));
      e.Handled = true;
    }

    private void Visit_Click(object sender, RoutedEventArgs e) {
      ViewsUtility.NavigateToUri(new Uri("http://fontstore.com"));
      e.Handled = true;
    }

    private void Help_Click(object sender, RoutedEventArgs e) {
      ViewsUtility.NavigateToUri(new Uri("http://fontstore.com/faqs"));
      e.Handled = true;
    }

    private void About_Click(object sender, RoutedEventArgs e) {
      OnAboutClicked?.Invoke();
    }

    private void Logout_Click(object sender, RoutedEventArgs e) {
      OnLogout?.Invoke();
    }

    private void Quit_Click(object sender, RoutedEventArgs e) {
      OnExit?.Invoke();
    }
    #endregion
  }
}
