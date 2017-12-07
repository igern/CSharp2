using MyLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace View
{
    public sealed partial class MainPage : Page
    {
        public SerialGenerator sg { get; set; }
        public Submissions subs { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            sg = new SerialGenerator();
            subs = new Submissions();
            
            
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
            if (!ValidateInput()) return; 
            Submission sub = new Submission();
            sub.firstName = firstName.Text;
            sub.lastName = lastName.Text;
            sub.email = email.Text;
            sub.phoneNumber = phoneNumber.Text;
            var date = dateOfBirth.Date;
            DateTime time = date.Value.DateTime;
            var formattedTime = time.ToString("dd.mm.yyyy");
            sub.dateOfBirth = formattedTime;
            sub.serial = serialNumber.Text;
            
            foreach(Submission someSub in subs.subList)
            {
                if (someSub == null) continue;
            }
            if(!sg.serialList.Contains(sub.serial) || subs.checkSub(sub)) {
                errorMessage.Text = "Please enter a valid serialcode";
                return;
            } else
            {
                Task task = Task.Factory.StartNew(() => subs.WriteToFileAsync(sub));
            }
        }

        private bool CheckSubs(string serial)
        {

            foreach(Submission sub in subs.subList)
            {
                if (sub == null) continue;
                if(serial.Equals(sub.serial))
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateView()
        {
            serialView.Items.Clear();
            foreach(Submission sub in subs.subList)
            {
                if (sub == null) continue;
                serialView.Items.Add(sub.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateView();
        }

        private bool ValidateInput()
        {
            if (firstName.Text.Length < 2)
            {
                errorMessage.Text = "First name must be longer than two characters";
                return false;
            }
            if (lastName.Text.Length < 2)
            {
                errorMessage.Text = "Last name must be longer than two characters";
                return false;
            }
            if (!email.Text.Contains('@'))
            {
                errorMessage.Text = "Must be a valid email";
                return false;
            }
            if (phoneNumber.Text.Length != 8)
            {
                errorMessage.Text = "Phonenumber must be 8 character long";
                return false;
            }
            if (!IsDigitsOnly(phoneNumber.Text))
            {
                errorMessage.Text = "Phonenumber must be digits";
                return false;
            }
            if (dateOfBirth.Date == null)
            {
                errorMessage.Text = "You have not pick a birthdate";
                return false;
            }
            if (serialNumber.Text.Length != 34)
            {
                errorMessage.Text = "Please enter a valid serialcode";
                return false;
            }
            return true;
        }

        static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}
