using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouletteTests
{
    class Program
    {
        static void Main(string[] args)
        {
            int timesToTry = 200;

            int successes = 0;
            int failures = 0;

            //int initialCapital = 2050;
            //int capitalGoal = 3050;

            int initialCapital = 64;
            int capitalGoal = 128;

            int totalGamesPlayed = 0;
            int oneGameTime = 45;

            while ((successes + failures) < timesToTry)
            {
                //Player player = new Player(2050, 3050, Player.SpinResult.Red, 1, 10000);
                Player player = new Player(initialCapital, capitalGoal, Player.SpinResult.Red, 1, 10000);
                player.StartPlaying(out bool result);
                totalGamesPlayed += player.betsHasBeenPlaced;

                if (result)
                {
                    successes++;
                }
                else
                {
                    Console.WriteLine($"Last account size before the last bet {player.lastAccountSizeBeforePlacingBet}");
                    failures++;
                }

                //Console.ReadKey();
                Console.WriteLine("___");

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
