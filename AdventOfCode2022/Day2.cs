using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day2 : AoCDay
    {
        public enum Move
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }
        public record Round(Move Move1, Move Move2);
        public record PlayedRound(Move Move1, Move Move2, int Player1Score, int Player2Score);

        static Dictionary<string, Move> TranslateToMove = new()
        { 
            {"A", Move.Rock },
            {"B", Move.Paper },
            {"C", Move.Scissors },
            {"X", Move.Rock },
            {"Y", Move.Paper },
            {"Z", Move.Scissors },
        };

        

        public static PlayedRound PlayRound(Round round)
        {
            int LossScore = 0;
            int DrawScore = 3;
            int WinScore = 6;
            int p1Score = 0;
            int p2Score = 0;
            // Draw
            if(round.Move1 == round.Move2)
            {
                p1Score = DrawScore;
                p2Score = DrawScore;
            }
            else if(((int)round.Move1 % 3) + 1 == (int)round.Move2) // Player 2 victory
            {
                p1Score = LossScore;
                p2Score = WinScore;
            }
            else // Player 1 victory
            {
                p1Score = WinScore;
                p2Score = LossScore;
            }
            return new PlayedRound(round.Move1, round.Move2, (int)round.Move1 + p1Score, (int)round.Move2 + p2Score);
        }

        public static string ExecutePart1(List<string> input)
        {
            int score = 0;
            foreach (var line in input)
            {
                var moves = line.Split(" ");
                var round = new Round(TranslateToMove[moves[0]], TranslateToMove[moves[1]]);
                var playedRound = PlayRound(round);
                score += playedRound.Player2Score;
            }
            return score.ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            throw new NotImplementedException();
        }
    }
}
