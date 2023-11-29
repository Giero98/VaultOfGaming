using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace VaultOfGaming.Views
{
    public partial class HomePage : ContentPage
    {
        public ObservableCollection<GameInfo> Games_NewReales { get; } = new ObservableCollection<GameInfo> { };
        public ObservableCollection<GameInfo2> Games_Upcoming { get; } = new ObservableCollection<GameInfo2> { };

        string[] gameTitles_NewReales = new string[20];
        string[] gameTitles_Upcoming = new string[20];

        string gamesNewRealesList_beginHtml = "/html/body/div[1]/div/div/div[2]/div[1]/div[2]/div/div[2]/div/div[";
        string gamesUpcomingList_beginHtml = "/html/body/div[1]/div/div/div[2]/div[1]/div[3]/div/div[2]/div/div[";

        public HomePage()
        {
            InitializeComponent();
            BindingContext = this;

            var html = @"https://www.metacritic.com/game/";
            var htmlDoc = new HtmlWeb().Load(html);
            

            for (int i = 0; i < 20; i++)
            {
                var gameTitle = htmlDoc.DocumentNode.SelectSingleNode(gamesNewRealesList_beginHtml + (i + 1) + "]/a/h3");
                gameTitles_NewReales[i] = gameTitle.InnerText.Trim();

                var gameImage = htmlDoc.DocumentNode.SelectSingleNode(gamesNewRealesList_beginHtml + (i + 1) + "]/a/div[1]/div[2]/picture/img");
                Regex regex = new Regex(@"src=""(.*?)""");
                Match match = regex.Match(gameImage.OuterHtml.Trim());
                string gameImage_html = match.Groups[1].Value.Replace("amp;", "");

                var gameRate = htmlDoc.DocumentNode.SelectSingleNode(gamesNewRealesList_beginHtml + (i + 1) + "]/a/div[2]/div[1]/div/span");
                string gameRate_overall = gameRate.InnerText.Trim();

                Games_NewReales.Add(new GameInfo { ImagePath = gameImage_html, Title = gameTitles_NewReales[i], Rate = gameRate_overall });

                if (i < 12)
                {
                    var gameTitle2 = htmlDoc.DocumentNode.SelectSingleNode(gamesUpcomingList_beginHtml + (i + 1) + "]/a/h3");
                    gameTitles_Upcoming[i] = gameTitle2.InnerText.Trim();

                    var gameImage2 = htmlDoc.DocumentNode.SelectSingleNode(gamesUpcomingList_beginHtml + (i + 1) + "]/a/div[1]/div[2]/picture/img");
                    Regex regex2 = new Regex(@"src=""(.*?)""");
                    Match match2 = regex2.Match(gameImage2.OuterHtml.Trim());
                    string gameImage_html2 = match2.Groups[1].Value.Replace("amp;", "");

                    var gameRate2 = htmlDoc.DocumentNode.SelectSingleNode(gamesUpcomingList_beginHtml + (i + 1) + "]/a/div[2]/div[1]/div/span");
                    string gameRate_overall2 = gameRate2.InnerText.Trim();

                    Games_Upcoming.Add(new GameInfo2 { ImagePath = gameImage_html2, Title = gameTitles_Upcoming[i], Rate = gameRate_overall2 });
                }
            }
        }
        

        private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                if (e.CurrentSelection[0] is GameInfo selectedItem)
                {
                    if(selectedItem.Title.Equals(gameTitles_NewReales[0]))
                    {
                        var html = @"https://www.metacritic.com/game/";
                        var htmlDoc = new HtmlWeb().Load(html);
                        var gameSite = htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[2]/div[1]/div[2]/div/div[2]/div/div[" + 1 + "]/a");

                        Regex regex = new Regex(@"href=""(.*?)""");
                        Match match = regex.Match(gameSite.OuterHtml.Trim());

                        var gameSite_html = gameSite.InnerText.Trim();

                        Navigation.PushAsync(new GameInfoPage(gameSite_html));
                    }
                }
                (sender as CollectionView).SelectedItem = null;
            }
        }
    }

    public class GameInfo
    {
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Rate { get; set; }
    }

    public class GameInfo2
    {
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Rate { get; set; }
    }
}