using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WizardScoreboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : TabbedPage
    {
        private int i = 0;
        private int totalGuess;
        private int errorCount;
        private int totalResult;
        private int howManyPlayers;
        private int numberOfRounds;
        private readonly List<Player> Players;
        public string pname;
        public string pnameGuessPlaceholder;
        public string pnameResultPlaceholder;
        public string pnameEntryName;
        public int count;
        public Dictionary<string, Entry> entryByNameGuess = new Dictionary<string, Entry>();
        public Dictionary<string, Entry> entryByNameResult = new Dictionary<string, Entry>();
        public List<Grid> scoreGrid = new List<Grid>();
        public List<int> guessInputs = new List<int>();

        public Button nextButton = new Button() { Text = "Next Round!" };
        public Button resultButton = new Button() { Text = "Get new scores!" };
        public Button guessButton = new Button() { Text = "Commit guesses!" };

        public GamePage(List<Player> players)
        {
            InitializeComponent();
            Players = players;
            AddGuessEntryFields();
            AddScoreLabels();
            playGame(Players);
        }

        public void playGame(List<Player> Players)
        {
            howManyPlayers = Players.Count;
            numberOfRounds = 60 / howManyPlayers;

            playRound(i, howManyPlayers, numberOfRounds, Players);
        }

        public void playRound(int turn, int howManyPlayers, int numberOfRounds, List<Player> Players)
        {
            // GET ROUND NUMBER AND DEALER NAME
            var result = getRoundAndDealer(turn, howManyPlayers, numberOfRounds, Players);
            RoundNr.Text = result.Item1;
            RoundDealer.Text = result.Item2;

            AddGuessButton();

        }

        private void AddGuessEntryFields()
        {
            count = 1;
            foreach (var player in Players)
            {
                pname = player.name.ToString();
                pnameGuessPlaceholder = pname + " tricks guessed";
                pnameEntryName = pname + count.ToString();

                Entry entry = new Entry() { Placeholder = pnameGuessPlaceholder,
                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                    Keyboard = Keyboard.Numeric };
                entryByNameGuess.Add(pnameEntryName, entry);

                RoundNrAndDealer_GuessAndResults.Children.Add(entry);

                count++;
            }
        }

        private void AddResultsEntryFields()
        {
            count = 1;
            foreach (var player in Players)
            {
                pname = player.name.ToString();
                pnameResultPlaceholder = pname + " tricks result";
                pnameEntryName = pname + count.ToString();

                Entry entry = new Entry()
                {
                    Placeholder = pnameResultPlaceholder,
                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                    Keyboard = Keyboard.Numeric
                };

                entryByNameResult.Add(pnameEntryName, entry);
                RoundNrAndDealer_GuessAndResults.Children.Add(entry);

                count++;
            }
        }

        private void AddResultButton()
        {
            resultButton.Clicked += new EventHandler(resultButton_Click);
            RoundNrAndDealer_GuessAndResults.Children.Add(resultButton);
        }

        private void AddGuessButton()
        {
            guessButton.Clicked += new EventHandler(guessButton_Click);
            RoundNrAndDealer_GuessAndResults.Children.Add(guessButton);
        }


        private void guessButton_Click(object sender, EventArgs e)
        {
            ValidateGuessInput(entryByNameGuess);
        }

        private void resultButton_Click(object sender, EventArgs e)
        {
            ValidateResultInput(entryByNameResult);
        }

        private void UpdateScores()
        {
            foreach (var player in Players)
            {
                var guess = player.trickGuess.Last();
                var result = player.trickResult.Last();
                var diff = Math.Abs(guess - result);

                if (diff == 0)
                {
                    player.score += guess * 10 + 20;
                }
                else
                {
                    player.score -= diff * 10;
                }
            }
        }

        private void ValidateGuessInput(Dictionary<string, Entry> entryByName)
        {
            errorCount = 0;
            totalGuess = 0;
            int turn = i + 1;

            foreach (var ent in entryByName)
            {
                var guess = Convert.ToInt32(ent.Value.Text);
                totalGuess += guess;

                if (guess > turn)
                {
                    DisplayAlert("Error!", "A guess is higher than possible!", "Get new input!");
                    errorCount++;
                }
                else if(guess < 0)
                {
                    DisplayAlert("Error!", "A guess is negative!", "Get new input!");
                    errorCount++;
                }
                else
                {
                    continue;
                }
            }

            if(totalGuess == turn)
            {
                DisplayAlert("Error!", "The total guess equals the round number!", "Get new input!");
                errorCount++;
            }

            if(errorCount == 0)
            {
                AddGuessToPlayer(entryByName);
                AddResultsEntryFields();
                AddResultButton();
                DisplayAlert("Guesses submitted!", "Please enter obtained tricks", "OK!");
            }
        }

        private void ValidateResultInput(Dictionary<string, Entry> entryByName)
        {
            errorCount = 0;
            totalResult = 0;
            int turn = i + 1;

            foreach (var ent in entryByName)
            {
                var result = Convert.ToInt32(ent.Value.Text);
                totalResult += result;

                if (result > turn)
                {
                    DisplayAlert("Error!", "A result is higher than possible!", "Get new input!");
                    errorCount++;
                }
                else if (result < 0)
                {
                    DisplayAlert("Error!", "A result is negative!", "Get new input!");
                    errorCount++;
                }
                else
                {
                    continue;
                }
            }

            if (totalResult != turn)
            {
                DisplayAlert("Error!", "The total number of tricks is not equals the round number!", "Get new input!");
                errorCount++;
            }

            if (errorCount == 0)
            {
                AddResultToPlayer(entryByNameResult);
                UpdateScores();
                DisplayAlert("Success", "Results added and scores updated!", "OK!");
                RemoveScoreLabels();
                AddScoreLabels();
                AddNextRoundButton();
                CurrentPage = Children[1];
            }
        }

        private void AddGuessToPlayer(Dictionary<string, Entry> entryByName)
        {
            count = 0;
            foreach (var ent in entryByName)
            {
                // Add guess to player guess list
                Players[count].trickGuess.Add(Convert.ToInt32(ent.Value.Text));
                count++;
            }
        }

        private void AddResultToPlayer(Dictionary<string, Entry> entryByName)
        {
            count = 0;
            foreach (var ent in entryByName)
            {
                // Add guess to player guess list
                Players[count].trickResult.Add(Convert.ToInt32(ent.Value.Text));
                count++;
            }
        }



        public Tuple<string, string> getRoundAndDealer(int turn, int howManyPlayers, int numberOfRounds, List<Player> Players)
        {
            var dealerNumber = turn % howManyPlayers;
            var dealerName = Players[dealerNumber].name;
            var round = turn + 1;

            var RoundNr = "Round Nr: " + round.ToString() + " / " + numberOfRounds.ToString();
            var RoundDealer = "Dealer: " + dealerName;

            var result = new Tuple<string, string>(RoundNr, RoundDealer);

            return result;
        }

        private void AddScoreLabels()
        {

            Grid grid = new Grid()
            {
                BackgroundColor = Color.White,
                RowSpacing = 5,
                ColumnSpacing = 0,
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                },
            };


            var rowStart = 0;

            foreach (var player in Players)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60, GridUnitType.Absolute) });

                var player_name = player.name;
                var player_score = player.score;

                BoxView bw1 = new BoxView()
                {
                    Color = Color.FromHex("#1A0E50")
                };

                Label lblName = new Label()
                {
                    Text = player_name,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.FromHex("#ffffff"),
                    FontSize = 20
                };

                grid.Children.Add(bw1, 0, 1, rowStart, rowStart + 1);
                grid.Children.Add(lblName, 0, 1, rowStart, rowStart + 1);
                
                BoxView bw2 = new BoxView()
                {
                    Color = Color.FromHex("#1A0E50")
                };

                Label lblScore = new Label()
                {
                    Text = player_score.ToString(),
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.FromHex("#ffffff"),
                    FontSize = 20
                };

                grid.Children.Add(bw2, 1, 2, rowStart, rowStart + 1);
                grid.Children.Add(lblScore, 1, 2, rowStart, rowStart + 1);

                rowStart++;

            }
            scoreboard.Children.Add(grid);
            scoreGrid.Add(grid);
        }


        private void RemoveScoreLabels()
        {
            foreach (var grid in scoreGrid)
            {
                scoreboard.Children.Remove(grid);
            }
        }

        private void AddNextRoundButton()
        {
            nextButton.Clicked += new EventHandler(nextButton_Click);
            RoundNrAndDealer_GuessAndResults.Children.Add(nextButton);

        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            nextButton.Clicked -= new EventHandler(nextButton_Click);
            resultButton.Clicked -= new EventHandler(resultButton_Click);
            guessButton.Clicked -= new EventHandler(guessButton_Click);

            RoundNrAndDealer_GuessAndResults.Children.Remove(nextButton);

            RemoveTextFromEntryField();
            RemoveResultEntryFields();

            RoundNrAndDealer_GuessAndResults.Children.Remove(resultButton);

            entryByNameResult.Clear();

            i++;

            if (i < numberOfRounds)
            {
                playRound(i, howManyPlayers, numberOfRounds, Players);

            }
            else
            {
                RemoveScoreLabels();

                Label GameOver = new Label()
                {
                    Text = "Game Over!",
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.FromHex("#000000"),
                    FontSize = 20
                };

                scoreboard.Children.Add(GameOver);

                AddScoreLabels();

                CurrentPage = Children[1];
            }
        }



        private void RemoveResultEntryFields()
        {
            foreach (var ent in entryByNameResult)
            {
                RoundNrAndDealer_GuessAndResults.Children.Remove(ent.Value);
            }
        }

        private void RemoveTextFromEntryField()
        {
            foreach (var ent in entryByNameGuess)
            {
                ent.Value.Text = String.Empty;
            }
        }

    }
}