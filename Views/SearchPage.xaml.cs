using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace VaultOfGaming.Views
{
    public partial class SearchPage : ContentPage
    {
        public ObservableCollection<InfoAboutSearchGame> Games_FromSearch { get; } = new ObservableCollection<InfoAboutSearchGame> { };

        readonly string searchingGames_XPath_firstPath = "/html/body/div[1]/div/div/div[2]/div[1]/div[2]/div[2]/div[";
        readonly string searchingGames_XPath_secondPath = "]/div/div[2]/div/div[";
        readonly string site_firstPath = @"https://www.metacritic.com/search/";
        readonly string site_secondPath = @"/?page=";
        readonly string site_thirdPath = @"&category=13";

        readonly double screenWidth = DeviceDisplay.MainDisplayInfo.Width;

        public SearchPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            string nameOfSearchingGame = searchBarData.Text;
            Games_FromSearch?.Clear();
            StartDownloadDataFromSearchPage(nameOfSearchingGame);
        }

        public void StartDownloadDataFromSearchPage(string nameOfSearchingGame)
        {
            var page = 1;
            var site = site_firstPath + nameOfSearchingGame + site_secondPath + page + site_thirdPath;
            var htmlDoc = new HtmlWeb().Load(site);

            for (int itemNumber = 0; itemNumber < 30; itemNumber++)
            {
                try
                {
                    var GameTitle = htmlDoc.DocumentNode.SelectSingleNode(searchingGames_XPath_firstPath + (itemNumber + 1) + "]/a/div[2]/p");
                    string gameTitle = GameTitle.InnerText.Trim();

                    var GameCover = htmlDoc.DocumentNode.SelectSingleNode(searchingGames_XPath_firstPath + (itemNumber + 1) + "]/a/div[1]/div/picture/img");
                    Regex regex = new(@"src=""(.*?)""");
                    Match match = regex.Match(GameCover.OuterHtml.Trim());
                    string gameCover_http = match.Groups[1].Value.Replace("amp;", "");

                    var GameRealeseDate = htmlDoc.DocumentNode.SelectSingleNode(searchingGames_XPath_firstPath + (itemNumber + 1) + "]/a/div[2]/div/span/span[1]");
                    string gameRealeseDate = GameRealeseDate.InnerText.Trim();

                    var GamePlatform = htmlDoc.DocumentNode.SelectSingleNode(searchingGames_XPath_firstPath + (itemNumber + 1) + "]/a/div[2]/div/span/span[2]");
                    string gamePlatform = GamePlatform.InnerText.Trim();

                    var GameRate = htmlDoc.DocumentNode.SelectSingleNode(searchingGames_XPath_firstPath + (itemNumber + 1) + "]/a/div[3]/div/div/span");
                    string bestGameRate_overall = GameRate.InnerText.Trim();

                    InfoAboutSearchGame game = new()
                    {
                        CoverHttp = gameCover_http,
                        ImageWidth = SetImageWidth(),
                        Title = gameTitle,
                        Rate = bestGameRate_overall,
                        BackgroundColorRate = HomePage.SetColorAroundRating(bestGameRate_overall),
                        RealaseDate = gameRealeseDate,
                        Platform = gamePlatform
                    };

                    Games_FromSearch.Add(game);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        private string SetImageWidth()
        {
            var imageWidth = screenWidth / 3;
            return imageWidth.ToString();
        }
    }

    public class InfoAboutSearchGame
    {
        public string CoverHttp { get; set; }
        public string ImageWidth {  get; set; }
        public string Title { get; set; }
        public string Rate { get; set; }
        public Color BackgroundColorRate { get; set; }
        public string RealaseDate { get; set; }
        public string Platform { get; set; }
    }
}
