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
            //int timesToTry = 100;
            int timesToTry = 100;

            int successes = 0;
            int failures = 0;

            //int initialCapital = 2050;
            //int capitalGoal = 3050;

            int initialCapital = 16385;
            int capitalGoal = 17385;

            int totalGamesPlayed = 0;
            int oneGameTime = 45;
            int baseBetSize = 1;

            while ((successes + failures) < timesToTry)
            {
                //Player player = new Player(2050, 3050, Player.SpinResult.Red, 1, 10000);
                Player player = new Player(initialCapital, capitalGoal, Player.SpinResult.Red, baseBetSize, 10000);
                player.StartPlaying(out bool result);
                totalGamesPlayed += player.betsHasBeenPlaced;

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
                Console.WriteLine(player.resultsLog);
                Console.WriteLine("___________");
                Console.WriteLine("___________");
                Console.WriteLine("___________");

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

            int secondsItTookToPlay = totalGamesPlayed * oneGameTime;
            Console.WriteLine($"Time it took to play - Days({TimeSpan.FromSeconds(secondsItTookToPlay).Days}), Hours({TimeSpan.FromSeconds(secondsItTookToPlay).Hours})" +
                $", Minutes({TimeSpan.FromSeconds(secondsItTookToPlay).Minutes})");

            Console.ReadLine();
        }
    }
}
