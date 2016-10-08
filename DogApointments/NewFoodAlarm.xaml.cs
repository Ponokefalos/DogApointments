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
    public sealed partial class NewWalkAlarm : Page
    {
        public NewWalkAlarm()
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
        private async void showMessageDialog(String txtToShow)
        {
            await new Windows.UI.Popups.MessageDialog(txtToShow).ShowAsync();
        }

        private async void stackNewFoodsAppointment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            bool isAppointmentValid = true;

            Appointment appointment = new Appointment();
            // StartTime
            var date = StartTimeDatePicker.Date;
            var time = StartTimeTimePicker.Time;
            var timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
            var startTime = new DateTimeOffset(date.Value.Year, date.Value.Month, date.Value.Day, time.Hours, time.Minutes, 0, timeZoneOffset);

            appointment.StartTime = startTime;

            // Subject
            appointment.Subject = SubjectTextBox.Text;
            if (appointment.Subject.Length > 255)
            {
                isAppointmentValid = false;
                showMessageDialog("The subject cannot be greater than 255 characters.");


            }
            // Location
            appointment.Location = LocationTextBox.Text;

            if (appointment.Location.Length > 32768)
            {
                isAppointmentValid = false;
                showMessageDialog("The location cannot be greater than 32,768 characters.");

            }

            // Details
            appointment.Details = DetailsTextBox.Text;

            if (appointment.Details.Length > 1073741823)
            {
                isAppointmentValid = false;
                showMessageDialog("The details cannot be greater than 1,073,741,823 characters.");
            }

            // Duration
            if (DurationComboBox.SelectedIndex == 0)
            {
                // 30 minute duration is selected
                appointment.Duration = TimeSpan.FromMinutes(30);
            }
            else
            {
                // 1 hour duration is selected
                appointment.Duration = TimeSpan.FromHours(1);
            }

            // appointment.Reminder = TimeSpan.FromMilliseconds(100);

            if (isAppointmentValid)
            {
                showMessageDialog("The appointment was created successfully and is valid.");
            }

            // Get the selection rect of the button pressed to add this appointment
            var rect = GetElementRect(sender as FrameworkElement);



            // ShowAddAppointmentAsync returns an appointment id if the appointment given was added to the user's calendar.
            // This value should be stored in app data and roamed so that the appointment can be replaced or removed in the future.
            // An empty string return value indicates that the user canceled the operation before the appointment was added.
            try
            {
                String appointmentId = await Windows.ApplicationModel.Appointments.AppointmentManager.ShowAddAppointmentAsync(appointment, rect, Windows.UI.Popups.Placement.Default);
                if (appointmentId != String.Empty)
                {
                    Debug.Write("Appointment Id: " + appointmentId);

                }
                else
                {
                    Debug.Write("Appointment not added.");

                }

                //Adding apointmentId to ArrayList()
                App.walkIdList.Add(appointmentId);

                //String Json serielise and save to app data
                String idToSave = JsonConvert.SerializeObject(App.walkIdList);


                //initialize Local App Data
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

                //Creating Conteiner
                Windows.Storage.ApplicationDataContainer container = localSettings.CreateContainer("WalkContainer", Windows.Storage.ApplicationDataCreateDisposition.Always);

                if (localSettings.Containers.ContainsKey("WalkContainer"))
                {
                    localSettings.Containers["WalkContainer"].Values["listWalk"] = idToSave;

                }

                App.walkList.Add(appointment);
              

                if (localSettings.Containers.ContainsKey("WalkContainer"))
                {
                    localSettings.Containers["WalkContainer"].Values["listaMeWalks"] = JsonConvert.SerializeObject(App.walkList);

                }
            }

            catch (Exception ex)
            {
               
            }

        }
        private Windows.Foundation.Rect GetElementRect(FrameworkElement element)
        {
            Windows.UI.Xaml.Media.GeneralTransform buttonTransform = element.TransformToVisual(null);
            Windows.Foundation.Point point = buttonTransform.TransformPoint(new Windows.Foundation.Point());
            return new Windows.Foundation.Rect(point, new Windows.Foundation.Size(element.ActualWidth, element.ActualHeight));
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

       
      
    }
}
