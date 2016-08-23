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
        List<Point> forbiddenCellPoints = new List<Point>();
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

            int x = 0;
            int y = 0;
            for (int i = 0; i < sizeOfGrid; i++)
            {
                string row = Console.ReadLine();
                List<bool> rowForbiddenCells = new List<bool>();
                foreach (char cell in row)
                {
                    rowForbiddenCells.Add(cell == forbiddenCellChar);
                    if ((cell == forbiddenCellChar))
                        forbiddenCellPoints.Add(new Point(x, y));
                    x++;
                }
                forbiddenCells.Add(rowForbiddenCells);
                y++;
                x = 0;
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

            foreach (Point p in new List<Point>() { start, end })
            {
                returnValue.Add(new Line
                (
                    Enumerable.Range(0, sizeOfGrid) //try all integers up to the size of the grid
                        .Where(i => i <= p.X //cells left of the p
                        && (i == 0 || forbiddenCells[p.Y][i - 1])) //get 0 (edge of grid) or any forbidden cells
                        .Max(), //set left most position to most restrictive cell
                    p.Y,
                    Enumerable.Range(0, sizeOfGrid).Where(i => i >= p.X && (i == sizeOfGrid - 1 || forbiddenCells[p.Y][i + 1])).Min(),
                    p.Y
                ));

                //add vertical line going through the p/end, bounded by edges of grid or forbidden cells
                returnValue.Add(new Line
                (
                    p.X,
                    Enumerable.Range(0, sizeOfGrid).Where(i => i <= p.Y && (i == 0 || forbiddenCells[i - 1][p.X])).Max(),
                    p.X,
                    Enumerable.Range(0, sizeOfGrid).Where(i => i >= p.Y && (i == sizeOfGrid - 1 || forbiddenCells[i + 1][p.X])).Min()
                ));
            }

            foreach (Point fp in forbiddenCellPoints)
            {    
                if (!((forbiddenCellPoints.Exists(p => p.X == fp.X-1 && p.Y == fp.Y) && //forbiddenCells left and right of fp
                    forbiddenCellPoints.Exists(p => p.X == fp.X+1 && p.Y == fp.Y)) ||
                    (forbiddenCellPoints.Exists(p => p.X == fp.X && p.Y == fp.Y-1) && //forbiddenCells above and below fp
                    forbiddenCellPoints.Exists(p => p.X == fp.X && p.Y == fp.Y+1)) ||
                    (fp.X == 0 && forbiddenCellPoints.Exists(p => p.X == 1 && p.Y == fp.Y)) || //fp in left most column and forbiddenCell to right
                    (fp.X == sizeOfGrid-1 && forbiddenCellPoints.Exists(p => p.X == sizeOfGrid-2 && p.Y == fp.Y)) || //fp in right most column and forbiddenCell to left
                    (fp.Y == 0 && forbiddenCellPoints.Exists(p => p.X == fp.X && p.Y == 1)) || //fp in top most row and forbiddenCell below
                    (fp.Y == sizeOfGrid - 1 && forbiddenCellPoints.Exists(p => p.X == fp.X && p.Y == sizeOfGrid - 2)))) //fp in bottom most row and forbiddenCell above
                {

                }
            }

            return returnValue;
        }

    }
}
