using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouletteTests
{
    public static class RouletteSessionsTester
    {
        public static void PlayTests()
        {
            int successes = 0;
            int failures = 0;

            int totalGamesPlayed = 0;
            int totalGamesSkippedOrPlayed = 0;

            int oneGameTime = 45;

            int initialCapital = 10;
            int capitalGoal = 30;
            int timesToTry = 1000;
            int baseBetSize = 10;
            int betsToSkip = 15;
            bool useGraphLogs = false;

            while ((successes + failures) < timesToTry)
            {
                //Player player = new Player(2050, 3050, Player.SpinResult.Red, 1, 10000);
                Player player = new Player(initialCapital, capitalGoal, Player.SpinResult.Red, baseBetSize, 10000, PlayMode.SimpleMartingale, betsToSkip);
                player.StartPlaying(out bool result);
                totalGamesPlayed += player.betsHasBeenPlaced;
                totalGamesSkippedOrPlayed += player.totalBetsSkippedOrPlaced;

                if (result)
                {
                    successes++;
                }
                else
                {
                    Console.WriteLine($"Last account size before the last bet {player.lastAccountSizeBeforePlacingBet}, now account is: {player.accountAmount}");
                    failures++;
                }

                
                //Console.ReadKey();
                Console.WriteLine("___________");
                if (useGraphLogs)
                {
                    Console.WriteLine(player.resultsLog);
                    Console.WriteLine("___________");
                    Console.WriteLine("___________");
                    Console.WriteLine("___________");
                }
                

                //timesToTry++;
            }

            Console.WriteLine("_____________________");
            Console.WriteLine("");
            Console.WriteLine($"Ended {timesToTry} sessions");
            Console.WriteLine($"Successes {successes}");
            Console.WriteLine($"Failures {failures}");

            int mathResult = -(initialCapital * failures) + (capitalGoal - initialCapital) * successes;
            Console.WriteLine($"Math Result: {mathResult}");
            Console.WriteLine($"Total games played: {totalGamesPlayed}");
            Console.WriteLine($"Total games there has been: {totalGamesSkippedOrPlayed}");

            int secondsItTookToPlay = totalGamesSkippedOrPlayed * oneGameTime;
            Console.WriteLine($"Time it took to play - Days({TimeSpan.FromSeconds(secondsItTookToPlay).Days}), Hours({TimeSpan.FromSeconds(secondsItTookToPlay).Hours})" +
                $", Minutes({TimeSpan.FromSeconds(secondsItTookToPlay).Minutes})");

            Console.ReadLine();
        }
    }
}
