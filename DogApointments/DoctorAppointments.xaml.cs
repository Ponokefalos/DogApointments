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
using System.Diagnostics;
using System.Collections;
using Windows.ApplicationModel.Appointments;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DogApointments
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoctorAppointments : Page
    {
     

        public DoctorAppointments()
        {
            this.InitializeComponent();

            fillListView();
            gvApointList.ItemsSource = App.apList;

            
            SetUpPageAnimation();


        }
        private async void showMessageDialog(String txtToShow)
        {
            await new Windows.UI.Popups.MessageDialog(txtToShow).ShowAsync();
        }

        public string getDaysLeft()
        {

            return "";
        }

        public void fillListView()
        {
            //initialize Local App Data
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            // Read data from a setting in a container

            bool hasContainer = localSettings.Containers.ContainsKey("AppointmentContainer");
            bool hasSetting = false;

            if (hasContainer)
            {
                hasSetting = localSettings.Containers["AppointmentContainer"].Values.ContainsKey("list");
            }


            if (hasSetting)
            {
                string composite = (string)localSettings.Containers["AppointmentContainer"].Values["list"];
                string compositeAppointment = (string)localSettings.Containers["AppointmentContainer"].Values["listaMeAppointments"];

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
                   

                }         
              
            }
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
        
         private async void gvApointList_SelectionChanged(object sender, SelectionChangedEventArgs e)
       {           
            string id = App.appointList[gvApointList.SelectedIndex];
            Debug.Write(id);           
           await AppointmentManager.ShowAppointmentDetailsAsync(id);

           
    }

        private void stackNewDoctor_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewApointment));
        }
    }
}
