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
        public int totalBetsSkippedOrPlaced;
        
        public int previousBetSize;
        public int previousBetCashResult;
        public bool previousBetResult;
        public int accumulatedLossesInARow;
        public int maxAccumulatedLossesInARow;
        public int lastAccountSizeBeforePlacingBet;

        public int biggestRowSize;
        public SpinResult biggestRowColor;

        public StringBuilder resultsLog;

        List<SpinResult> latestSpinResults;
        PlayMode chosenPlayMode;
        public int preferredOffsetToPlaceBets;
        private bool isInAgainstSequenceMode;
        
        public Player(int accountAmount, int finalGoal, SpinResult preferredDefaultSpin, int baseBetSize, int betsToPlace, PlayMode playMode, int preferredOffsetToPlaceBets = 5)
        {
            this.accountAmount = accountAmount;
            this.finalGoal = finalGoal;
            this.preferredDefaultSpin = preferredDefaultSpin;
            this.baseBetSize = baseBetSize;
            this.betsToPlace = betsToPlace;
            this.accumulatedLossesInARow = 0;
            this.maxAccumulatedLossesInARow = 0;

            previousBetCashResult = 1;

            resultsLog = new StringBuilder();

            latestSpinResults = new List<SpinResult>();
            chosenPlayMode = playMode;
            this.preferredOffsetToPlaceBets = preferredOffsetToPlaceBets;
        }

        public bool StartPlaying(out bool successfulEnding)
        {
            while (CanContinuePlaying(out bool success, false, out int nextBetSize))
            {
                if (chosenPlayMode == PlayMode.SimpleMartingale || isInAgainstSequenceMode)
                {
                    PlaceBet(nextBetSize);
                }else if (chosenPlayMode == PlayMode.MartingaleWithOffset)
                {
                    if (IsSequenceIsLongEnoughOfSameElements(out SpinResult newPreferredSpinResult))
                    {
                        preferredDefaultSpin = newPreferredSpinResult;
                        PlaceBet(nextBetSize);
                    }
                    else
                    {
                        SkipBet();
                    }
                }
            }
            Console.WriteLine(accountAmount >= finalGoal ? "WINNER" : "LOSER");
            Console.WriteLine($"Max losses in a row: {maxAccumulatedLossesInARow}");
            Console.WriteLine($"Bets has been placed: {betsHasBeenPlaced}");
            Console.WriteLine($"Total bets there has been: {totalBetsSkippedOrPlaced}");
            Console.WriteLine($"AccountAmount: {accountAmount}");

            return CanContinuePlaying(out successfulEnding, true, out int _);
        }

        public void SkipBet()
        {
            var spinResult = possibleSpinResults[rnd.Next(0, possibleSpinResults.Length)];
            if (latestSpinResults.Count < preferredOffsetToPlaceBets)
            {
                latestSpinResults.Add(spinResult);
            }
            else
            {
                latestSpinResults.RemoveAt(0);
                latestSpinResults.Add(spinResult);
            }
            
            resultsLog.Append($"[{spinResult}]");
            totalBetsSkippedOrPlaced++;
        }

        bool IsSequenceIsLongEnoughOfSameElements(out SpinResult newPreferredSpinResult)
        {
            newPreferredSpinResult = SpinResult.None;
            
            int blacksCount = latestSpinResults.Where(a => a == SpinResult.Black).ToList().Count;
            int redsCount = latestSpinResults.Where(a => a == SpinResult.Red).ToList().Count;

            if (blacksCount == preferredOffsetToPlaceBets)
            {
                newPreferredSpinResult = SpinResult.Red;
                return true;
            }
            if(redsCount == preferredOffsetToPlaceBets)
            {
                newPreferredSpinResult = SpinResult.Black;
                return true;
            }
                
            return false;
        }

        public void PlaceBet(int nextBetSize)
        {
            lastAccountSizeBeforePlacingBet = accountAmount;
            int betSize = nextBetSize;
            previousBetSize = betSize;

            var spinResult = possibleSpinResults[rnd.Next(0, possibleSpinResults.Length)];

            previousBetResult = spinResult == preferredDefaultSpin;

            if (spinResult == preferredDefaultSpin)
            {
                resultsLog.Append($"[{SpinResultToString(spinResult)}W]");
                accumulatedLossesInARow = 0;
                accountAmount += betSize;
                previousBetCashResult = betSize;

                if (chosenPlayMode != PlayMode.SimpleMartingale)
                {
                    isInAgainstSequenceMode = false;
                    latestSpinResults = new List<SpinResult>();
                }
            }
            else
            {
                resultsLog.Append($"[{SpinResultToString(spinResult)}L]");
                accumulatedLossesInARow += 1;
                if(accumulatedLossesInARow > maxAccumulatedLossesInARow)
                {
                    maxAccumulatedLossesInARow = accumulatedLossesInARow;
                }
                accountAmount -= betSize;
                previousBetCashResult = -betSize;
            }

            betsHasBeenPlaced++;
            totalBetsSkippedOrPlaced++;
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

        string SpinResultToString(SpinResult spinResult)
        {
            switch (spinResult)
            {
                case SpinResult.Black:
                    return "B-";
                case SpinResult.Red:
                    return "R-";
                case SpinResult.Zero:
                    return "Z-";
                case SpinResult.None:
                    return "N-";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(spinResult), spinResult, null);
            }
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
            Zero,
            None
        }
    }

    public enum PlayMode
    {
        SimpleMartingale,
        MartingaleWithOffset
    }
}
