﻿using Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UI.Utilities;

namespace UI.Views {
  /// <summary>
  /// Interaction logic for FontList.xaml
  /// </summary>
  public partial class FontList : Window, IView {
    #region data
    private Protocol.Payloads.UserData _userData;
    private int _installedCount;
    private int _newCount;
    private int _allCount;
    private IFontStorage _storage;
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

    public IFontStorage Storage {
      get {
        return _storage;
      }
      set {
        _storage = value;
        UpdateCounters();
      }
    }
    #endregion

    #region delegate
    #endregion

    #region events
    public event OnExitHandler OnExit;
    public event OnLogoutHandler OnLogout;
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
      Dispatcher.Invoke(action);
    }

    public void LoadingState(bool isLoading) {
      if (isLoading) {
        LoadingBar.Visibility = Visibility.Visible;
        InstalledCountLabel.Visibility = Visibility.Collapsed;
        NewCountLabel.Visibility = Visibility.Collapsed;
        AllCountLabel.Visibility = Visibility.Collapsed;

        FamilyTree.Visibility = Visibility.Hidden;
        FamilyTree.ItemsSource = null;
      } else {
        LoadingBar.Visibility = Visibility.Collapsed;
        InstalledCountLabel.Visibility = Visibility.Visible;
        NewCountLabel.Visibility = Visibility.Visible;
        AllCountLabel.Visibility = Visibility.Visible;

        //List<Family> families = new List<Family>();

        //Family f = new Family() {
        //  Name = "Family1"
        //};
        //f.Fonts.Add(new Font() { Name = "Font1.1", Activated = false });
        //f.Fonts.Add(new Font() { Name = "Font1.2", Activated = true });

        //families.Add(f);
        //f = new Family() {
        //  Name = "Family2"
        //};
        //f.Fonts.Add(new Font() { Name = "Font2.1", Activated = false });
        //f.Fonts.Add(new Font() { Name = "Font2.2", Activated = false });
        //f.Fonts.Add(new Font() { Name = "Font2.3", Activated = false });
        //families.Add(f);

        //FamilyTree.ItemsSource = families;

        FamilyTree.ItemsSource = Storage.Families;
        FamilyTree.Visibility = Visibility.Visible;
      }
    }
    #endregion

    #region private methods
    private void UpdateCounters() {
      AllCountLabel.Content = string.Format("({0})", Storage.Families.Count);
      NewCountLabel.Content = string.Format("({0})", Storage.NewFamilies.Count);
      InstalledCountLabel.Content = string.Format("({0})", Storage.ActivatedFamilies.Count);
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
    }

    private void Logout_Click(object sender, RoutedEventArgs e) {
      OnLogout?.Invoke();
    }

    private void Quit_Click(object sender, RoutedEventArgs e) {
      OnExit?.Invoke();
    }
    #endregion
  }

  public class Family {
    public string Name { get; set; }
    public List<Font> Fonts { get; set; }

    public Family() {
      Fonts = new List<Font>();
    }
  }

  public class Font {
    public string Name { get; set; }
    public bool Activated { get; set; }
  }
}
