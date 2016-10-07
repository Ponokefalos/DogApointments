using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Appointments;
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
    public sealed partial class WalkAlarms : Page
    {
        public WalkAlarms()
        {
            this.InitializeComponent();
            fillListView();
            gvWalkList.ItemsSource = App.walkList;

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

        public void fillListView()
        {
            //initialize Local App Data
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            // Read data from a setting in a container

            bool hasContainer = localSettings.Containers.ContainsKey("WalkContainer");
            bool hasSetting = false;

            if (hasContainer)
            {
                hasSetting = localSettings.Containers["WalkContainer"].Values.ContainsKey("listWalk");
            }


            if (hasSetting)
            {
                string composite = (string)localSettings.Containers["WalkContainer"].Values["listWalk"];
                string compositeAppointment = (string)localSettings.Containers["WalkContainer"].Values["listaMeWalks"];

                //an yparxoun registerd appointments
                if (composite != null)
                {

                    Debug.WriteLine("yparxoun appointments");
                    App.appointList = JsonConvert.DeserializeObject<List<string>>(composite);
                    try
                    {
                        App.apList = JsonConvert.DeserializeObject<List<Appointment>>(compositeAppointment);
                    }
                    catch (Exception e)
                    {

                    }

                    for (int i = 0; i < App.apList.Count; i++)
                    {
                        Debug.WriteLine(i);
                        // DoctorApoitnsList.Items.Add();


                    }
                    //------------------------------------------------
                    //degug  -- delete after finish
                    txtNothingInside.Text = "There is appointment";
                    txtNothingInside.Visibility = Visibility.Visible;
                    //------------------------------------------------

                }
                //an den yparxoun registerd appointments
                else if (composite == null)
                {

                    txtNothingInside.Text = "There not appointments registered yet! Please make a new one to begin!";
                    txtNothingInside.Visibility = Visibility.Visible;

                }
            }
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

        private async void gvWalkList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string id = App.appointList[gvWalkList.SelectedIndex];
            Debug.Write(id);
            await AppointmentManager.ShowAppointmentDetailsAsync(id);
        }
           

       

        private void stackNewFoods_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewWalkAlarm));

        }
    }
}
