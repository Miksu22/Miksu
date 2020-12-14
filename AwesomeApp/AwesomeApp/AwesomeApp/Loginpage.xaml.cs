using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AwesomeApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class Loginpage : ContentPage
    {
        public Loginpage()
        {
            InitializeComponent();

        }

        int count = 0;
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            count++;
            ((Button)sender).Text = $"Klikkasit {count} kertaa.";
        }

        async void Handle_Register(object sender, System.EventArgs e)
        {

            if (!string.IsNullOrEmpty(nick.Text) && !string.IsNullOrEmpty(password.Text))
            {
                var authAPI = RestService.For<IAuthAPI>("http://13.74.41.52:8082");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("username", nick.Text);
                data.Add("password", password.Text);
                SimpleServerResponse res = await authAPI.Register(data);
                infoLabel.Text = res.result.ToString();
            }

        }

        async void Handle_Login(object sender, System.EventArgs e)
        {

            if (!string.IsNullOrEmpty(nick.Text) && !string.IsNullOrEmpty(password.Text))
            {
                var authAPI = RestService.For<IAuthAPI>("http://13.74.41.52:8082");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("username", nick.Text);
                data.Add("password", password.Text);
                UserAccess res = await authAPI.Login(data);
                infoLabel.Text = res.result.ToString();

                if (res.result.Contains("Onnistui"))
                {

                    await Navigation.PushAsync(new Page1(res.accesskey));


                }

            }

        }
    }
}