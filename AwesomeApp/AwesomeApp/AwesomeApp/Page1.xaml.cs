using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Refit;

namespace AwesomeApp
{
    public partial class Page1 : ContentPage
    {

        private ObservableCollection<string> _rivit;
        public Page1(string accesskey)
        {
            InitializeComponent();
            _rivit = new ObservableCollection<string>();
            Lista.ItemsSource = _rivit;
        }

        int count = 0;
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var authAPI = RestService.For<IAuthAPI>("http://10.0.2.2:3000");
            string result = await authAPI.HelloWorld();
            count++;
            ((Button)sender).Text = result + " vastaanotti " + count.ToString() + " kertaa.";


        }
        async void Add_Clicked(object sender, System.EventArgs e)
        {
            var authAPI = RestService.For<IAuthAPI>("http://10.0.2.2:3000");
            string result = await authAPI.Add();
            ((Button)sender).Text = result;
        }
        async void List_Clicked(object sender, System.EventArgs e)
        {
            var authAPI = RestService.For<IAuthAPI>("http://10.0.2.2:3000");
            List < Rivi > result = await authAPI.List();
            _rivit.Clear();
            foreach (Rivi rivi in result)

            {
                _rivit.Add(rivi.testidata);
            }
        }

    }

}