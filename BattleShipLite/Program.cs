using BattleShipLiteLibrary;
using BattleShipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipLite
{
    class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();

            PlayerInfoModel activePlayer = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");
            PlayerInfoModel Winner = null;

            do
            {
                //Display grid from activePlayer and where they fired
                DisplayShotGrid(activePlayer);


                //Ask player 1 for a shot
                //Determine if its a valid shot 
                //Determine shot results
                RecordPlayerShot(activePlayer, opponent);


                //Determine if the game is over
                bool DoesGameContinue = GameLogic.PlayerStillActive(opponent);

                //If over, Set Player 1 as the winner


                //Else, swap position (Active to opponent)
                if (DoesGameContinue == true)
                {
                    //Swap using a temp variable
                    PlayerInfoModel temHolder = opponent;
                    opponent = activePlayer;
                    activePlayer = temHolder;

                    // Use Tuple
                    (activePlayer, opponent) = (opponent, activePlayer);


                }
                else
                {
                    Winner = activePlayer;
                }



            } while (Winner == null);

            identifyWinner(Winner);
            
            Console.ReadLine();
        }

        private static void identifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations to {winner.UserName} for winning");
            Console.WriteLine($"{winner.UserName} took {GameLogic.GetshotCount(winner)} shots.");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {

            bool isValidshot = false;
            string row = "";
            int column = 0;

            do
            {
                string shot = AskforShot();
                (string row, int column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                isValidshot = GameLogic.ValidateShot(activePlayer, row, column);

                if (isValidshot==false)
                {
                    Console.WriteLine("Invalid Shot location. Please try again.");
                }


            } while (!isValidshot); //not true or false (isValidshot == false)
            
            bool isAhit = GameLogic.IdentifyShotResult(opponent, row, column);

            //Record shot results
            GameLogic.MarkShotResult(activePlayer, row, column );

            //Ask for a shot (we ask for "B2")
            //Determine what row and column that is - split it apart
            //Determine if its a valid shot
            //Go back to the beginning if its not a valid shot
            
            
            //Determine shot results


        }

        private static string AskforShot()
        {
            Console.WriteLine("Please enter your shot celection");
            string output = Console.ReadLine();

            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }



                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($"{gridSpot.SpotLetter}{gridSpot.SpotNumber}");

                }
                else if (gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X ");
                }
                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O ");
                }
                else
                {
                    Console.Write(" ? ");
                }
            }
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battle Ship Lite");
            Console.WriteLine("Created By Micheal Shodamola");
            Console.WriteLine();
        }

        private static PlayerInfoModel CreatePlayer(string PlayerTitle)
        {


            PlayerInfoModel output = new PlayerInfoModel();

            Console.WriteLine($"Player information for {PlayerTitle}");

            //Ask user for their name
            output.UserName = AskForUsersName();
            //Load up the shot grid
            GameLogic.InitialiseGrid(output);

            //ask the user for their 5 ship placements
            PlaceShips(output);

            //clear the screen
            Console.Clear();

            return output;

        }



        private static string AskForUsersName()
        {

            Console.WriteLine("What is your name");
            string Nameoutput = Console.ReadLine();

            return Nameoutput;
        }
    
        private static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.WriteLine($"Where di you want to place your ship number {model.ShipLocations.Count + 1}: ");
                string location = Console.ReadLine();

                bool isValidLocation = GameLogic.PlaceShip(model, location);

                if (isValidLocation == false)
                {
                    Console.WriteLine("This is not a valid location. please try again");
                }
            } while (model.ShipLocations.Count < 5);
        }
    
    }


}
