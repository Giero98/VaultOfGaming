using System.Collections.ObjectModel;

namespace VaultOfGaming.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public ObservableCollection<Game> Games { get; } = new ObservableCollection<Game>
        {
            new Game { ImagePath = "game.png", Title = "Alan Wake II", Rate = "90"},
            new Game { ImagePath = "game.png", Title = "Tytuł 2", Rate = "90"},
            new Game { ImagePath = "game.png", Title = "Tytuł 1", Rate = "90"},
            new Game { ImagePath = "game.png", Title = "Tytuł 2", Rate = "90"},
            new Game { ImagePath = "game.png", Title = "Tytuł 1", Rate = "90"},
            new Game { ImagePath = "game.png", Title = "Tytuł 2", Rate = "90"},
            new Game { ImagePath = "game.png", Title = "Tytuł 1", Rate = "90"},
            new Game { ImagePath = "game.png", Title = "Tytuł 2", Rate = "90"},
            new Game { ImagePath = "game.png", Title = "Tytuł 1", Rate = "90"},
            new Game { ImagePath = "game.png", Title = "Tytuł 2", Rate = "90"},
            // Dodaj więcej elementów YourItem
        };

        private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                if (e.CurrentSelection[0] is Game selectedItem)
                {
                    if(selectedItem.Title.Equals("Alan Wake II"))
                    {
                        Navigation.PushAsync(new GameInfoPage(@"https://www.metacritic.com/game/alan-wake-ii/"));
                    }
                }
                (sender as CollectionView).SelectedItem = null;
            }
        }
    }

    public class Game
    {
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Rate { get; set; }
    }
}