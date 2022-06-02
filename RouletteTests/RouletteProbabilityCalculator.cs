using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouletteTests
{
    public static class RouletteProbabilityCalculator
    {
        public static void PerformCalculation()
        {
            CalculatePotentialProfit(1, 14);
        }

        static void CalculateChances(out float chanceToWin, out float chanceToLose)
        {
            int successfulBets = Player.possibleSpinResults.Where(a => a == Player.SpinResult.Red).ToList().Count;
            int unsuccessfulBets = Player.possibleSpinResults.Length - successfulBets;

            chanceToWin = successfulBets / (float) Player.possibleSpinResults.Length;
            chanceToLose = unsuccessfulBets / (float) Player.possibleSpinResults.Length;
        }

        public static void CalculatePotentialProfit(int baseBet ,int lossesInARow)
        {
            CalculateChances(out float chanceToWin, out float chanceToLose);
            Console.WriteLine($"One time chance to win: {chanceToWin}");
            Console.WriteLine($"One time chance to lose: {chanceToLose}");

            float accumulatedChanceToLose = 1f;
            int accumulatedLoss = 0;
            for (int i = 0; i < lossesInARow; i++)
            {
                int nextBetSize = accumulatedLoss > 0 ? accumulatedLoss : baseBet;
                //Console.WriteLine($"accumulatedLoss({accumulatedLoss}) + baseBet({baseBet}) * {i + 1} = {accumulatedLoss + (int)Math.Pow(baseBet * 2, i)}");
                accumulatedLoss += nextBetSize;
                accumulatedChanceToLose *= chanceToLose;
                Console.WriteLine($"Accumulated loss at {i+1} loses is {accumulatedLoss}");
            }

            float gamesForAverageOneLoss = 1 / accumulatedChanceToLose;

            int possibleIncome = (int)(gamesForAverageOneLoss / 2);

            Console.WriteLine($"Chance to lose {lossesInARow} times in a row is {accumulatedChanceToLose}");
            Console.WriteLine($"Need on average {gamesForAverageOneLoss} games to lose");
            Console.WriteLine($"Accumulated loss would be: {accumulatedLoss}");
            Console.WriteLine($"Possible income: {possibleIncome}");

            Console.ReadLine();
        }
    }
}
