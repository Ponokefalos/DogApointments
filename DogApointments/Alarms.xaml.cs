using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DogApointments
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Alarms : Page
    {
        public Alarms()
        {
            this.InitializeComponent();
            SetUpPageAnimation();
        }
        private void SetUpPageAnimation()
        {
            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();

            var info = new ContinuumNavigationTransitionInfo();

            theme.DefaultNavigationTransitionInfo = info;
            collection.Add(theme);
            this.Transitions = collection;
        }
        private void btnDocAlarms_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DoctorAppointments));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible; // Make back btn visible
            currentView.BackRequested += backButton_Tapped;

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible; // Make back btn visible
            currentView.BackRequested -= backButton_Tapped;
        }

        private void backButton_Tapped(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack) Frame.GoBack();
        }  

      
        private void stackMedicine_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PharmacyReminders));
        }

        private void stackDoctor_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DoctorAppointments));
        }

        private void stackFood_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(WalkAlarms));
        }
    }
}
