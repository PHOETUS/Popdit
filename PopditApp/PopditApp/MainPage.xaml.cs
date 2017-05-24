using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PopditApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private bool mSignUp = false;

        public MainPage()
        {            
            InitializeComponent();

            buttonSignIn.IsEnabled = false;
            entryMobile.IsVisible = false;
            entryNickname.IsVisible = false;
            labelSignedIn.IsVisible = false;
            labelCreated.IsVisible = false;
            labelShare.IsVisible = false;
            buttonConfig.IsVisible = false;                        
        }

        private void buttonSignIn_Clicked(object sender, EventArgs e)
        {
            buttonSignIn.IsEnabled = false;
            buttonCreateAccount.IsEnabled = true;
            mSignUp = false;
            mSignUp = false;
            entryMobile.IsVisible = false;
            entryNickname.IsVisible = false;    
        }

        private void buttonCreateAccount_Clicked(object sender, EventArgs e)
        {
            buttonSignIn.IsEnabled = true;
            buttonCreateAccount.IsEnabled = false;
            mSignUp = true;
            entryMobile.IsVisible = true;
            entryNickname.IsVisible = true;
        }

        private void buttonSubmit_Clicked(object sender, EventArgs e)
        {
            buttonSignIn.IsVisible = false;
            buttonCreateAccount.IsVisible = false;
            entryEmail.IsVisible = false;
            entryPassword.IsVisible = false;
            entryMobile.IsVisible = false;
            entryNickname.IsVisible = false;
            buttonSubmit.IsVisible = false;
            if (mSignUp) labelCreated.IsVisible = true;
            else labelSignedIn.IsVisible = true;
            labelShare.IsVisible = true;
            buttonConfig.IsVisible = true;
        }

        private void buttonConfig_Clicked(object sender, EventArgs e)
        {

        }
    }
}
