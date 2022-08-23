using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Mancala
{
    public class MancalaRunner
    { 
        public static void Main(string[] args)
        {
#if DEBUG
            Board.Play();
#else
            var summary = BenchmarkRunner.Run(typeof(MancalaRunner).Assembly);
#endif
        }

        [Benchmark]
        public void Bench_First_Move_Well_1()
        {
            Board.Move(new Board(), 0);
        }

        [Benchmark]
        public void Bench_First_Move_Well_2()
        {
            Board.Move(new Board(), 1);
        }

        [Benchmark]
        public void Bench_First_Move_Well_3()
        {
            Board.Move(new Board(), 2);
        }

        [Benchmark]
        public void Bench_First_Move_Well_4()
        {
            Board.Move(new Board(), 3);
        }

        [Benchmark]
        public void Bench_First_Move_Well_5()
        {
            Board.Move(new Board(), 4);
        }

        [Benchmark]
        public void Bench_First_Move_Well_6()
        {
            Board.Move(new Board(), 5);
        }

        [Benchmark]
        public void Grade_Board_PlayerOne()
        {
            Board.GradeBoard(new Board(), Player.one);
        }

        [Benchmark]
        public void Grade_Board_PlayerTwo()
        {
            Board.GradeBoard(new Board(), Player.two);
        }

        [Benchmark]
        public void GetBestMove_P1()
        {
            Board.GetBestMove(new Board(), Player.one, 1);
        }

        [Benchmark]
        public void GetBestMove_P2()
        {
            Board.GetBestMove(new Board(), Player.one, 1);
        }
    }
}
