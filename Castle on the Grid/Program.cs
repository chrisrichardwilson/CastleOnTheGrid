using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Solution to https://www.hackerrank.com/challenges/castle-on-the-grid
/// </summary>
namespace Castle_on_the_Grid
{     

    class Solution
    {        
        static void Main(string[] args)
        {
            CastleSolver castleSolver = new CastleSolver();
            castleSolver.Solve();
        }
    }

    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;
    }

    public struct Line
    {
        public Line(int x1, int y1, int x2, int y2)
        {
            Start = new Point(x1, y1);
            End = new Point(x2, y2);
        }

        public Point Start;
        public Point End;
    }

    class CastleSolver
    {
        const char forbiddenCellChar = 'X';
        int sizeOfGrid;
        List<List<bool>> forbiddenCells = new List<List<bool>>();
        Point start;
        Point end;

        public void Solve()
        {
            readInput();
            getInterestingLines();
        }

        /// <summary>
        /// read console inputs and populate sizeOfGrid, forbiddenCells, start and end
        /// </summary>
        private void readInput()
        {
            sizeOfGrid = int.Parse(Console.ReadLine());

            for (int i = 0; i < sizeOfGrid; i++)
            {
                string row = Console.ReadLine();
                List<bool> rowForbiddenCells = new List<bool>();
                foreach (char cell in row)
                    rowForbiddenCells.Add(cell == 'X');
                forbiddenCells.Add(rowForbiddenCells);
            }

            int[] positionNumbers = Array.ConvertAll(Console.ReadLine().Split(' '), Int32.Parse);
            start.X = positionNumbers[0];
            start.Y = positionNumbers[1];
            end.X = positionNumbers[2];
            end.Y = positionNumbers[3];
        }

        /// <summary>
        /// get lines from start and end point and all lines that touch a forbidden cell
        /// </summary>
        /// <returns></returns>
        private List<Line> getInterestingLines()
        {
            List<Line> returnValue = new List<Line>();

            returnValue.Add(new Line
            (
                Enumerable.Range(0, sizeOfGrid).Where(i => i < start.X && (i == 0 ||forbiddenCells[start.Y][i-1])).Max(),
                start.Y,
                Enumerable.Range(0, sizeOfGrid).Where(i => i > start.X && (i == sizeOfGrid - 1 || forbiddenCells[start.Y][i+1])).Min(),
                start.Y
            ));

            return returnValue;
        }

    }
}
