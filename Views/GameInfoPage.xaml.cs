using HtmlAgilityPack;

namespace VaultOfGaming.Views
{
    public partial class GameInfoPage : ContentPage
    {

        public GameInfoPage(string html)
        {
            InitializeComponent();

            string gameSite = new Uri(new Uri(@"https://www.metacritic.com"), html).AbsoluteUri;

            var htmlDoc = new HtmlWeb().Load(gameSite);

            var gameTitle = htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[2]/div[1]/div[1]/div/div/div[2]/div[3]/div[1]/div");

            GameTitle.Text = gameTitle.InnerText.Trim();
        }
    }
}