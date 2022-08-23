using System;
namespace Mancala
{
    class Board
    {
        public bool IsGameOver
        {
            get
            {
                return (Spaces[0].Stones == 0
                    && Spaces[1].Stones == 0
                    && Spaces[2].Stones == 0
                    && Spaces[3].Stones == 0
                    && Spaces[4].Stones == 0
                    && Spaces[5].Stones == 0)
                    ||
                    (Spaces[7].Stones == 0
                    && Spaces[8].Stones == 0
                    && Spaces[9].Stones == 0
                    && Spaces[10].Stones == 0
                    && Spaces[11].Stones == 0
                    && Spaces[12].Stones == 0);
            }
        }

        BoardSpace[] Spaces;

        public Board()
        {
            Spaces = new BoardSpace[14];
            for (int i = 0; i < 14; i++)
            {
                Spaces[i] = new BoardSpace(i);
                if (i != 13)
                {
                    Spaces[i].Next = i + 1;
                }
                else
                {
                    Spaces[i].Next = 0;
                }
            }
        }

        public static void Play()
        {

            var gameBoard = new Board();

            while (!gameBoard.IsGameOver)
            {
                bool myTurn = true;
                while (!gameBoard.IsGameOver && myTurn)
                {
                    Board.PrintBoard(gameBoard);
                    Console.WriteLine();
                    Console.Write("Enter your move: ");
                    var move = Console.ReadKey();

                    //TODO: Clean this up probably
                    switch (move.Key)
                    {
                        case ConsoleKey.D1:
                            myTurn = Board.Move(gameBoard, 0);
                            break;
                        case ConsoleKey.D2:
                            myTurn = Board.Move(gameBoard, 1);
                            break;
                        case ConsoleKey.D3:
                            myTurn = Board.Move(gameBoard, 2);
                            break;
                        case ConsoleKey.D4:
                            myTurn = Board.Move(gameBoard, 3);
                            break;
                        case ConsoleKey.D5:
                            myTurn = Board.Move(gameBoard, 4);
                            break;
                        case ConsoleKey.D6:
                            myTurn = Board.Move(gameBoard, 5);
                            break;
                    }
                }

                Random R = new Random();
                while (!gameBoard.IsGameOver && Board.Move(gameBoard, R.Next(7, 14)))
                {
                    Board.PrintBoard(gameBoard);
                    Console.WriteLine();
                };
            }

            Console.WriteLine("\n\nGame Over!!");
            Board.PrintBoard(gameBoard);
            Board.PrintBoardShort(gameBoard);

        }

        public static int GradeBoard(Board board, Player player)
        {
            return player switch
            {
                Player.one => board.Spaces[0].Stones + board.Spaces[1].Stones + board.Spaces[2].Stones + board.Spaces[3].Stones + board.Spaces[4].Stones + board.Spaces[5].Stones,
                Player.two => board.Spaces[7].Stones + board.Spaces[8].Stones + board.Spaces[9].Stones + board.Spaces[10].Stones + board.Spaces[11].Stones + board.Spaces[12].Stones,
            };
        }

        public static void PrintBoard(Board board)
        {
            Console.WriteLine();
            Console.WriteLine($"({board.Spaces.Where(x => x.Index > 6).Sum(x => x.Stones):00})                                    ");
            Console.WriteLine(" -----------------------------------");
            Console.WriteLine($"|   [{board.Spaces[12].Stones:00}] " +
                $"[{board.Spaces[11].Stones:00}] " +
                $"[{board.Spaces[10].Stones:00}] " +
                $"[{board.Spaces[9].Stones:00}] " +
                $"[{board.Spaces[8].Stones:00}] " +
                $"[{board.Spaces[7].Stones:00}]   | ");
            Console.WriteLine($"{board.Spaces[13].Stones:00}\t\t\t            {board.Spaces[6].Stones:00} ");

            Console.WriteLine($"|   [{board.Spaces[0].Stones:00}] " +
                $"[{board.Spaces[1].Stones:00}] " +
                $"[{board.Spaces[2].Stones:00}] " +
                $"[{board.Spaces[3].Stones:00}] " +
                $"[{board.Spaces[4].Stones:00}] " +
                $"[{board.Spaces[5].Stones:00}]   | ");
            Console.WriteLine(" -----------------------------------");
            Console.WriteLine($"                                 ({board.Spaces.Where(x => x.Index < 7).Sum(x => x.Stones):00})");
            Console.WriteLine();
        }

        public static void PrintBoardShort(Board board)
        {
            Console.WriteLine($"({board.Spaces.Where(x => x.Index < 7).Sum(x => x.Stones):00}),({board.Spaces.Where(x => x.Index > 6).Sum(x => x.Stones):00})");
        }

        private static Player GetPlayerFromWellIndex(int Index)
        {
            if (Index < 7)
            {
                return Player.one;
            }
            else
            {
                return Player.two;
            }
        }

        public static int GetWellOnOppositeSide(int index)
        {
            return 12 - index;
        }

        public static int GetScoreWellForPlayer(Player player)
        {
            return player switch
            {
                Player.one => 6,
                Player.two => 13
            };
        }

        public static bool Move(Board board, int index)
        {
            //NOTE: You can't move score wells, so just return 'true' that we get to go again.
            if (index == 13 || index == 6)
            {
#if DEBUG
                Console.WriteLine("\tPlayer must go again, cannot move score wells");
#endif
                return true;
            }

#if DEBUG
            Console.WriteLine();
#endif
            var movePlayer = GetPlayerFromWellIndex(index);

#if DEBUG
            Console.WriteLine($"Beginning move: well {index}, player {movePlayer}");
            Console.WriteLine($"\tMove made by player {movePlayer}");
#endif

            var stones = board.Spaces[index].Stones;

            //NOTE: If the well specified has 0 stones, just ask the player to go again.
            if (stones == 0)
            {
#if DEBUG
                Console.WriteLine("\tSpecified well has 0 stones, cannot make move");
#endif
                return true;
            }

#if DEBUG
            Console.WriteLine($"\tWell has {stones} stones");
#endif
            board.Spaces[index].Stones = 0;
            var currentSpace = board.Spaces[board.Spaces[index].Next];
            while (stones != 0)
            {
                currentSpace.Stones += 1;
#if DEBUG
                Console.WriteLine($"\tPlacing a stone in {currentSpace.Index}");
#endif
                stones--;

                //TODO: Maybe not another check for stones != 0 here?
                if (stones != 0)
                {
                    currentSpace = board.Spaces[board.Spaces[currentSpace.Index].Next];
                }
            }

#if DEBUG
            Console.WriteLine($"\tEnded on well {currentSpace.Index}, which belongs to player {GetPlayerFromWellIndex(currentSpace.Index)}");
#endif

            if ((currentSpace.Index == 6 && movePlayer == Player.one)
                || (currentSpace.Index == 13 && movePlayer == Player.two))
            {
#if DEBUG
                Console.WriteLine("\tPlayer goes again");
#endif
                return true;
            }

            if(currentSpace.Stones == 1
                && GetPlayerFromWellIndex(currentSpace.Index) == movePlayer)
            {
                board.Spaces[GetScoreWellForPlayer(movePlayer)].Stones += board.Spaces[GetWellOnOppositeSide(currentSpace.Index)].Stones;
                board.Spaces[GetWellOnOppositeSide(currentSpace.Index)].Stones = 0;
#if DEBUG
                Console.WriteLine($"\tStealing from {GetWellOnOppositeSide(currentSpace.Index)}");
#endif
            }

#if DEBUG
            Console.WriteLine();
#endif
            return false;
        }

        public static int GetBestMove(Board board, Player player, int Depth)
        {
            int bestMove = 0;
            int bestMoveScore = 0;
            if (player == Player.one)
            {
                for (int i = 0; i < 6; i++)
                {
                    Board testBoard = board;
                    Board.Move(testBoard, i);
                    int thisMoveScore = Board.GradeBoard(testBoard, player);
                    if (thisMoveScore > bestMoveScore) bestMove = i; 
                }
            }
            else
            {
                for (int i = 7; i < 13; i++)
                {
                    Board testBoard = board;
                    Board.Move(testBoard, i);
                    int thisMoveScore = Board.GradeBoard(testBoard, player);
                    if (thisMoveScore > bestMoveScore) bestMove = i;
                }
            }

            return bestMove;
        }
    }
}

