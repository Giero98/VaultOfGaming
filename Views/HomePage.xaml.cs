using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;


namespace VaultOfGaming.Views
{
    public partial class HomePage : ContentPage
    {
        public ObservableCollection<InfoAboutGame> Games_NewRealesList { get; } = new ObservableCollection<InfoAboutGame> { };
        public ObservableCollection<InfoAboutGame> Games_UpcomingList { get; } = new ObservableCollection<InfoAboutGame> { };

        string[][] gamesTitle = new string[][]
        {
            new string[20], //NewReales
            new string[12] //Upcoming
        };

        readonly string games_XPath_firstPath = "/html/body/div[1]/div/div/div[2]/div[1]/div[";
        readonly string games_XPath_secondPath = "]/div/div[2]/div/div[";
        readonly string site = @"https://www.metacritic.com/game/";


        public HomePage()
        {
            InitializeComponent();
            BindingContext = this;

            var htmlDoc = new HtmlWeb().Load(site);

            for (int numberOfListGames = 0; numberOfListGames < gamesTitle.Length; numberOfListGames++)
            {
                for (int gameItemFromTheList = 0; gameItemFromTheList < gamesTitle[numberOfListGames].Length; gameItemFromTheList++)
                {
                    GetGameTitleFromSite(htmlDoc, numberOfListGames, gameItemFromTheList);
                    string coverHttp = GetLinkToCover(htmlDoc, numberOfListGames, gameItemFromTheList);
                    string gameRate = GetGameRateFromSite(htmlDoc, numberOfListGames, gameItemFromTheList);
                    string colorRate = SetColorAroundRating(gameRate);
                    InfoAboutGame game = new() { CoverHttp = coverHttp, 
                                                 Title = gamesTitle[numberOfListGames][gameItemFromTheList],
                                                 Rate = gameRate,
                                                 BackgroundColorRate = colorRate };
                    switch (numberOfListGames)
                    {
                        case 0:
                            Games_NewRealesList.Add(game);
                            break;
                        case 1:
                            Games_UpcomingList.Add(game);
                            break;
                    }
                }
            }
        }

        private void GetGameTitleFromSite(HtmlDocument htmlDoc, int numberOfListGames, int gameItemFromTheList)
        {
            var gameTitle = htmlDoc.DocumentNode.SelectSingleNode(games_XPath_firstPath + (numberOfListGames + 2) + games_XPath_secondPath + (gameItemFromTheList + 1) + "]/a/h3");
            gamesTitle[numberOfListGames][gameItemFromTheList] = gameTitle.InnerText.Trim();
        }

        private string GetLinkToCover(HtmlDocument htmlDoc, int numberOfListGames, int gameItemFromTheList)
        {
            var gameCover = htmlDoc.DocumentNode.SelectSingleNode(games_XPath_firstPath + (numberOfListGames + 2) + games_XPath_secondPath + (gameItemFromTheList + 1) + "]/a/div[1]/div[2]/picture/img");
            Regex regex = new(@"src=""(.*?)""");
            Match match = regex.Match(gameCover.OuterHtml.Trim());
            string gameCover_http = match.Groups[1].Value.Replace("amp;", "");
            return gameCover_http;
        }

        private string GetGameRateFromSite(HtmlDocument htmlDoc, int numberOfListGames, int gameItemFromTheList)
        {
            var gameRate = htmlDoc.DocumentNode.SelectSingleNode(games_XPath_firstPath + (numberOfListGames + 2) + games_XPath_secondPath + (gameItemFromTheList + 1) + "]/a/div[2]/div[1]/div/span");
            string gameRate_overall = gameRate.InnerText.Trim();
            return gameRate_overall;
        }

        public static string SetColorAroundRating(string gameRate)
        {
            if (!gameRate.Equals("tbd"))
            {
                if (int.Parse(gameRate) >= 80) return "#02CE7A";
                else if (int.Parse(gameRate) < 50) return "#FF6874";
                else return "#FFBD3F";
            }
            else return "#FFFFFF";
        }


        private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                if (e.CurrentSelection[0] is InfoAboutGame selectedGame)
                {
                    HtmlNode selectedGameSite = SearchSelectedGameFromListsAndMatchingPage(selectedGame);
                    Regex regex = new Regex(@"href=""(.*?)""");
                    Match match = regex.Match(selectedGameSite.OuterHtml.Trim());
                    var gameSite_html = match.Groups[1].Value;
                    Navigation.PushAsync(new GameInfoPage(gameSite_html));
                }
                (sender as CollectionView).SelectedItem = null;
            }
        }

        private HtmlNode SearchSelectedGameFromListsAndMatchingPage(InfoAboutGame selectedGame)
        {
            var htmlDoc = new HtmlWeb().Load(site);
            for (int numberOfListGames = 0; numberOfListGames < gamesTitle.Length; numberOfListGames++)
            {
                for (int gameItemFromTheList = 0; gameItemFromTheList < gamesTitle[numberOfListGames].Length; gameItemFromTheList++)
                {
                    if (selectedGame.Title.Equals(gamesTitle[numberOfListGames][gameItemFromTheList]))
                    {
                        var selectedGameSite = htmlDoc.DocumentNode.SelectSingleNode(games_XPath_firstPath + (numberOfListGames + 2) + games_XPath_secondPath + (gameItemFromTheList + 1) + "]/a");
                        return selectedGameSite;
                    }
                }
            }
            return null;
        }
    }

    public class InfoAboutGame
    {
        public string CoverHttp { get; set; }
        public string Title { get; set; }
        public string Rate { get; set; }
        public string BackgroundColorRate { get; set; }
    }
}