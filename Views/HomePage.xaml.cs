using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;


namespace VaultOfGaming.Views
{
    public partial class HomePage : ContentPage
    {
        public ObservableCollection<InfoAboutGame> Games_NewRealesList { get; } = new ObservableCollection<InfoAboutGame> { };
        public ObservableCollection<InfoAboutGame> Games_UpcomingList { get; } = new ObservableCollection<InfoAboutGame> { };
        public ObservableCollection<InfoAboutGame> Games_BestGameOnList { get; } = new ObservableCollection<InfoAboutGame> { };
        public ObservableCollection<InfoAboutGame> Games_NewPSPlusList { get; } = new ObservableCollection<InfoAboutGame> { };
        public ObservableCollection<InfoAboutGame> Games_NewXboxGamePassList { get; } = new ObservableCollection<InfoAboutGame> { };

        string[][] gamesTitle = new string[][]
        {
            new string[20], //NewReales
            new string[12], //Upcoming
            new string[14], //NewPSPlus
            new string[12] //NewXboxGamePass
        };

        readonly string games_XPath_firstPath = "/html/body/div[1]/div/div/div[2]/div[1]/div[";
        readonly string games_XPath_secondPath = "]/div/div[2]/div/div[";
        readonly string site = @"https://www.metacritic.com/game/";

        public HomePage()
        {
            InitializeComponent();
            BindingContext = this;

            var htmlDoc = new HtmlWeb().Load(site);


            var htmlDoc_bestGameOn = new HtmlWeb().Load(@"https://www.metacritic.com/browse/game/ps5/");
            for (int i = 0; i < 12; i++)
            {
                var GameTitle = htmlDoc_bestGameOn.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[2]/div[1]/main/section/div[3]/div[1]/div[" + (i + 1) + "]/a/div[2]/div[1]/h3");
                string gameTitle = GameTitle.InnerText.Trim();


                var GameCover = htmlDoc_bestGameOn.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[2]/div[1]/main/section/div[3]/div[1]/div[" + (i + 1) + "]/a/div[1]/div/div/div[2]/picture/img");
                Regex regex = new(@"src=""(.*?)""");
                Match match = regex.Match(GameCover.OuterHtml.Trim());
                string gameCover_http = match.Groups[1].Value.Replace("amp;", "");

                
                var GameRate = htmlDoc_bestGameOn.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[2]/div[1]/main/section/div[3]/div[1]/div[" + (i + 1) + "]/a/div[2]/div[4]/span/div/div/span");
                string bestGameRate_overall = GameRate.InnerText.Trim();

                InfoAboutGame game = new()
                {
                    CoverHttp = gameCover_http,
                    Title = gameTitle,
                    Rate = bestGameRate_overall,
                    BackgroundColorRate = SetColorAroundRating(bestGameRate_overall)
                };

                Games_BestGameOnList.Add(game);
            }

            pickerBestGamesOn.SelectedIndexChanged += (sender, e) =>
            {
                string bestGameSite = @"https://www.metacritic.com/browse/game/";
                switch (pickerBestGamesOn.SelectedIndex)
                {
                    case 0:
                        bestGameSite += @"ps5/";
                        break;
                    case 1:
                        bestGameSite += @"pc/";
                        break;
                    case 2:
                        bestGameSite += @"nintendo-switch/";
                        break;
                    case 3:
                        bestGameSite += @"ps4/";
                        break;
                    case 4:
                        bestGameSite += @"xbox-one/";
                        break;
                    case 5:
                        bestGameSite += @"xbox-series-x/";
                        break;
                }
                var htmlDoc_bestGameOn = new HtmlWeb().Load(bestGameSite);

                if(Games_BestGameOnList != null)
                {
                    Games_BestGameOnList.Clear();
                }

                for (int i = 0; i < 12; i++)
                {
                    var GameTitle = htmlDoc_bestGameOn.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[2]/div[1]/main/section/div[3]/div[1]/div[" + (i + 1) + "]/a/div[2]/div[1]/h3");
                    string gameTitle = GameTitle.InnerText.Trim();


                    var GameCover = htmlDoc_bestGameOn.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[2]/div[1]/main/section/div[3]/div[1]/div[" + (i + 1) + "]/a/div[1]/div/div/div[2]/picture/img");
                    Regex regex = new(@"src=""(.*?)""");
                    Match match = regex.Match(GameCover.OuterHtml.Trim());
                    string gameCover_http = match.Groups[1].Value.Replace("amp;", "");


                    var GameRate = htmlDoc_bestGameOn.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[2]/div[1]/main/section/div[3]/div[1]/div[" + (i + 1) + "]/a/div[2]/div[4]/span/div/div/span");
                    string bestGameRate_overall = GameRate.InnerText.Trim();

                    InfoAboutGame game = new()
                    {
                        CoverHttp = gameCover_http,
                        Title = gameTitle,
                        Rate = bestGameRate_overall,
                        BackgroundColorRate = SetColorAroundRating(bestGameRate_overall)
                    };

                    Games_BestGameOnList.Add(game);
                }
            };

            for (int numberOfListGames = 0; numberOfListGames < gamesTitle.Length; numberOfListGames++)
            {
                for (int gameItemFromTheList = 0; gameItemFromTheList < gamesTitle[numberOfListGames].Length; gameItemFromTheList++)
                {
                    GetGameTitleFromSite(htmlDoc, numberOfListGames, gameItemFromTheList);
                    string coverHttp = GetLinkToCover(htmlDoc, numberOfListGames, gameItemFromTheList);
                    string gameRate = GetGameRateFromSite(htmlDoc, numberOfListGames, gameItemFromTheList);
                    Color colorRate = SetColorAroundRating(gameRate);
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
                        case 2:
                            Games_NewPSPlusList.Add(game);
                            break;
                        case 3:
                            Games_NewXboxGamePassList.Add(game);
                            break;
                    }
                }
            }
        }

        private void GetGameTitleFromSite(HtmlDocument htmlDoc, int numberOfListGames, int gameItemFromTheList)
        {
            int nextGameList = 2;
            if (numberOfListGames == 2) nextGameList = 5;
            else if (numberOfListGames == 3) nextGameList = 7;

            var gameTitle = htmlDoc.DocumentNode.SelectSingleNode(games_XPath_firstPath + (numberOfListGames + nextGameList) + games_XPath_secondPath + (gameItemFromTheList + 1) + "]/a/h3");
            gamesTitle[numberOfListGames][gameItemFromTheList] = gameTitle.InnerText.Trim();
        }

        private string GetLinkToCover(HtmlDocument htmlDoc, int numberOfListGames, int gameItemFromTheList)
        {
            int nextGameList = 2;
            if (numberOfListGames == 2) nextGameList = 5;
            else if (numberOfListGames == 3) nextGameList = 7;

            var gameCover = htmlDoc.DocumentNode.SelectSingleNode(games_XPath_firstPath + (numberOfListGames + nextGameList) + games_XPath_secondPath + (gameItemFromTheList + 1) + "]/a/div[1]/div[2]/picture/img");
            Regex regex = new(@"src=""(.*?)""");
            Match match = regex.Match(gameCover.OuterHtml.Trim());
            string gameCover_http = match.Groups[1].Value.Replace("amp;", "");
            return gameCover_http;
        }

        private string GetGameRateFromSite(HtmlDocument htmlDoc, int numberOfListGames, int gameItemFromTheList)
        {
            int nextGameList = 2;
            if (numberOfListGames == 2) nextGameList = 5;
            else if (numberOfListGames == 3) nextGameList = 7;

            var gameRate = htmlDoc.DocumentNode.SelectSingleNode(games_XPath_firstPath + (numberOfListGames + nextGameList) + games_XPath_secondPath + (gameItemFromTheList + 1) + "]/a/div[2]/div[1]/div/span");
            string gameRate_overall = gameRate.InnerText.Trim();
            return gameRate_overall;
        }

        private Color SetColorAroundRating(string gameRate)
        {
            if (!gameRate.Equals("tbd"))
            {
                if (int.Parse(gameRate) >= 80) return Color.FromArgb("#02CE7A");
                else if (int.Parse(gameRate) < 50) return Color.FromArgb("#FF6874");
                else return Color.FromArgb("#FFBD3F");
            }
            else return Colors.White;
        }


        private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                if (e.CurrentSelection[0] is InfoAboutGame selectedGame)
                {
                    HtmlNode selectedGameSite = SearchSelectedGameFromListsAndMatchingPage(selectedGame);
                    Regex regex = new(@"href=""(.*?)""");
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
        public Color BackgroundColorRate { get; set; }
    }
}