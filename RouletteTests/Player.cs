using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouletteTests
{

    public class Player
    {
        public int accountAmount;
        public int finalGoal;
        public SpinResult preferredDefaultSpin;
        public int baseBetSize;
        public int betsToPlace;

        public int betsHasBeenPlaced;
        public int previousBetSize;
        public int previousBetCashResult;
        public int accumulatedLossesInARow;
        public int lastAccountSizeBeforePlacingBet;

        public int biggestRowSize;
        public SpinResult biggestRowColor;

        public Player(int accountAmount, int finalGoal, SpinResult preferredDefaultSpin, int baseBetSize, int betsToPlace)
        {
            this.accountAmount = accountAmount;
            this.finalGoal = finalGoal;
            this.preferredDefaultSpin = preferredDefaultSpin;
            this.baseBetSize = baseBetSize;
            this.betsToPlace = betsToPlace;

            previousBetCashResult = 1;
        }

        public bool StartPlaying(out bool successfulEnding)
        {
            while (CanContinuePlaying(out bool _, false, out int nextBetSize))
            {
                PlaceBet(nextBetSize);
            }

            Console.WriteLine($"Bets has been placed: {betsHasBeenPlaced}");
            Console.WriteLine($"AccountAmount: {accountAmount}");

            return CanContinuePlaying(out successfulEnding, true, out int _);
        }

        public void PlaceBet(int nextBetSize)
        {
            lastAccountSizeBeforePlacingBet = accountAmount;
            int betSize = nextBetSize;
            previousBetSize = betSize;

            var spinResult = possibleSpinResults[rnd.Next(0, possibleSpinResults.Length)];

            if (spinResult != preferredDefaultSpin)
            {
                accountAmount += betSize;
                previousBetCashResult = betSize;
            }
            else
            {
                accountAmount -= betSize;
                previousBetCashResult = -betSize;
            }

            betsHasBeenPlaced++;
        }

        bool CanContinuePlaying(out bool successfulEnding, bool logs, out int nextBetSize)
        {
            successfulEnding = false;
            int betSize = previousBetCashResult > 0 ? baseBetSize : previousBetSize * 2;
            nextBetSize = betSize;

            if (accountAmount <= 0)
            {
                if (logs)
                    Console.WriteLine($"Can't afford to keep playing... LOST ALL MONEY");
                return false;
            }

            if (accountAmount < betSize)
            {
                //if(logs)
                //    Console.WriteLine($"Can't afford to keep playing... accountAmount({accountAmount}) < nextExpectedBet({previousBetCashResult * 2})");
                //return false;
                nextBetSize = accountAmount;
            }

            if (betsHasBeenPlaced >= betsToPlace)
            {
                if (logs)
                    Console.WriteLine($"Reached total amount of bets");
                return false;
            }

            if(accountAmount >= finalGoal)
            {
                if (logs)
                    Console.WriteLine($"Reached final cash goal, accountAmount:{accountAmount}");
                successfulEnding = true;
                return false;
            }
            return true;
        }



        public static Random rnd = new Random();

        public static SpinResult[] possibleSpinResults = new SpinResult[]{SpinResult.Zero, SpinResult.Red, SpinResult.Red, SpinResult.Red, SpinResult.Red, SpinResult.Red, SpinResult.Red, SpinResult.Red
    , SpinResult.Red, SpinResult.Red, SpinResult.Red, SpinResult.Red, SpinResult.Red, SpinResult.Red, SpinResult.Red, SpinResult.Red, SpinResult.Red, SpinResult.Red, SpinResult.Red, SpinResult.Black
    , SpinResult.Black, SpinResult.Black, SpinResult.Black, SpinResult.Black, SpinResult.Black, SpinResult.Black, SpinResult.Black, SpinResult.Black, SpinResult.Black, SpinResult.Black
    , SpinResult.Black, SpinResult.Black, SpinResult.Black, SpinResult.Black, SpinResult.Black, SpinResult.Black, SpinResult.Black};
    

    public enum SpinResult
        {
            Red,
            Black,
            Zero
        }
    }
}
