using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace VaultOfGaming.Views
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {

        }
    }
}
