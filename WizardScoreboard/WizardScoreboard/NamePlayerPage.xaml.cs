using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WizardScoreboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    
    public partial class NamePlayerPage : ContentPage
    {
        public List<Player> players = new List<Player>();

        public NamePlayerPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            players = getPlayerNames();

            Navigation.PushAsync(new GamePage(players));
        }

        private List<Player> getPlayerNames()
        {
            var p1Name = Player1.Text;
            var p2Name = Player2.Text;
            var p3Name = Player3.Text;
            var p4Name = Player4.Text;
            var p5Name = Player5.Text;
            var p6Name = Player6.Text;

            if (!String.IsNullOrEmpty(p1Name))
            {
                Player player = new Player(p1Name);
                players.Add(player);
            }
            if (!String.IsNullOrEmpty(p2Name))
            {
                Player player = new Player(p2Name);
                players.Add(player);
            }
            if (!String.IsNullOrEmpty(p3Name))
            {
                Player player = new Player(p3Name);
                players.Add(player);
            }
            if (!String.IsNullOrEmpty(p4Name))
            {
                Player player = new Player(p4Name);
                players.Add(player);
            }
            if (!String.IsNullOrEmpty(p5Name))
            {
                Player player = new Player(p5Name);
                players.Add(player);
            }
            if (!String.IsNullOrEmpty(p6Name))
            {
                Player player = new Player(p6Name);
                players.Add(player);
            }

            return players;
        }
    }





}