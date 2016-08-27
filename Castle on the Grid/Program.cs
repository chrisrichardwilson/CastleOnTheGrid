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
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }        

        public override string ToString()
        {
            return string.Format("{0}, {1}", X, Y);
        }
    }

    public struct Line
    {
        public Point Start;
        public Point End;

        public Line(int x1, int y1, int x2, int y2)
        {
            Start = new Point(x1, y1);
            End = new Point(x2, y2);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1} to {2}, {3}", Start.X, Start.Y, End.X, End.Y);
        }
    }

    //public struct LinkedPoint
    //{
    //    public Point P;
    //    public List<Point> LinkedPoints;

    //    public LinkedPoint(Point p, List<Point> linkedPoints)
    //    {
    //        P = p;
    //        LinkedPoints = linkedPoints;
    //    }
    //}

    //public struct PointScore
    //{
    //    public Point P;
    //    public int Score;

    //    public PointScore(int x, int y, int score = 0)
    //    {
    //        P.X = x;
    //        P.Y = y;
    //        Score = score;
    //    }
    //}

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
            List<Line> interestingLines = getInterestingLines();
            List<Point> intersections = getIntersections(interestingLines);
            Dictionary<Point, List<Point>> tree = getTree(intersections);
            showGridWithIntersections(intersections);
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
                //add horizontal line going through the start/end, bounded by edges of grid or forbidden cells
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

                //add vertical line going through the start/end, bounded by edges of grid or forbidden cells
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
                //ignore forbidden cells that have 2 other forbidden cells in the same direction
                if (!((forbiddenCellPoints.Exists(p => p.X == fp.X-1 && p.Y == fp.Y) && //forbiddenCells left and right of fp
                    forbiddenCellPoints.Exists(p => p.X == fp.X+1 && p.Y == fp.Y)) ||
                    (forbiddenCellPoints.Exists(p => p.X == fp.X && p.Y == fp.Y-1) && //forbiddenCells above and below fp
                    forbiddenCellPoints.Exists(p => p.X == fp.X && p.Y == fp.Y+1)) ||
                    (fp.X == 0 && forbiddenCellPoints.Exists(p => p.X == 1 && p.Y == fp.Y)) || //fp in left most column and forbiddenCell to right
                    (fp.X == sizeOfGrid-1 && forbiddenCellPoints.Exists(p => p.X == sizeOfGrid-2 && p.Y == fp.Y)) || //fp in right most column and forbiddenCell to left
                    (fp.Y == 0 && forbiddenCellPoints.Exists(p => p.X == fp.X && p.Y == 1)) || //fp in top most row and forbiddenCell below
                    (fp.Y == sizeOfGrid - 1 && forbiddenCellPoints.Exists(p => p.X == fp.X && p.Y == sizeOfGrid - 2)))) //fp in bottom most row and forbiddenCell above
                {
                    //if there isn't a forbidden cell directly above this one, add horizontal line above
                    if (fp.Y > 0 && !forbiddenCellPoints.Any(fc => fc.X == fp.X && fc.Y == fp.Y - 1))
                        returnValue.Add(new Line
                        (
                            Enumerable.Range(0, sizeOfGrid).Where(i => i <= fp.X && (i == 0 || forbiddenCells[fp.Y-1][i - 1])).Max(),
                            fp.Y - 1,
                            Enumerable.Range(0, sizeOfGrid).Where(i => i >= fp.X && (i == sizeOfGrid - 1 || forbiddenCells[fp.Y-1][i + 1])).Min(),
                            fp.Y - 1
                        ));
                    //if there isn't a forbidden cell directly below this one, add horizontal line below
                    if (fp.Y < sizeOfGrid-1 && !forbiddenCellPoints.Any(fc => fc.X == fp.X && fc.Y == fp.Y + 1))
                        returnValue.Add(new Line
                        (
                            Enumerable.Range(0, sizeOfGrid).Where(i => i <= fp.X && (i == 0 || forbiddenCells[fp.Y + 1][i - 1])).Max(),
                            fp.Y + 1,
                            Enumerable.Range(0, sizeOfGrid).Where(i => i >= fp.X && (i == sizeOfGrid - 1 || forbiddenCells[fp.Y + 1][i + 1])).Min(),
                            fp.Y + 1
                        ));
                    //if there isn't a forbidden cell directly right of this one, add vertical line to the right
                    if (fp.X < sizeOfGrid-1 && !forbiddenCellPoints.Any(fc => fc.Y == fp.Y && fc.X == fp.X + 1))
                        returnValue.Add(new Line
                        (
                            fp.X+1,
                            Enumerable.Range(0, sizeOfGrid).Where(i => i <= fp.Y && (i == 0 || forbiddenCells[i - 1][fp.X+1])).Max(),
                            fp.X+1,
                            Enumerable.Range(0, sizeOfGrid).Where(i => i >= fp.Y && (i == sizeOfGrid - 1 || forbiddenCells[i + 1][fp.X+1])).Min()
                        ));
                    //if there isn't a forbidden cell directly left of this one, add vertical line to the left
                    if (fp.X > 0 && !forbiddenCellPoints.Any(fc => fc.Y == fp.Y && fc.X == fp.X + 1))
                        returnValue.Add(new Line
                        (
                            fp.X-1,
                            Enumerable.Range(0, sizeOfGrid).Where(i => i <= fp.Y && (i == 0 || forbiddenCells[i - 1][fp.X - 1])).Max(),
                            fp.X-1,
                            Enumerable.Range(0, sizeOfGrid).Where(i => i >= fp.Y && (i == sizeOfGrid - 1 || forbiddenCells[i + 1][fp.X - 1])).Min()
                        ));
                }
            }

            return returnValue;
        }

        private List<Point> getIntersections(List<Line> interestingLines)
        {
            List<Point> returnValue = new List<Point>();

            foreach (Line l in interestingLines)
            {
                //for each crossing line where l vertical and crossingLine horizontal
                foreach (Line crossingLine in
                    interestingLines.Where(l2 => l.Start.X == l.End.X && l2.Start.X != l2.End.X &&
                    l.Start.X >= l2.Start.X && l.Start.X <= l2.End.X &&
                    l.Start.Y <= l2.Start.Y && l.End.Y >= l2.Start.Y))
                    returnValue.Add(new Point(l.Start.X, crossingLine.Start.Y));
                //for each crossing line where l horizonal and crossingLine vertical
                foreach (Line crossingLine in
                    interestingLines.Where(l2 => l.Start.Y == l.End.Y && l2.Start.Y != l2.End.Y &&
                    l.Start.Y >= l2.Start.Y && l.Start.Y <= l2.End.Y &&
                    l.Start.X <= l2.Start.X && l.End.X >= l2.Start.X))
                    returnValue.Add(new Point(crossingLine.Start.X, l.Start.Y));
            }

            returnValue = returnValue.Distinct().ToList();
            return returnValue;
        }

        /// <summary>
        /// Gets the search tree, all the intersection points and which other points they are directly connected to
        /// </summary>
        /// <param name="intersections"></param>
        /// <returns></returns>
        private Dictionary<Point, List<Point>> getTree(List<Point> intersections)
        {
            Dictionary<Point, List<Point>> returnValue = new Dictionary<Point, List<Point>>();

            foreach (Point p1 in intersections)
            {
                foreach (Point p2 in intersections.Where(p => p.X != p1.X || p.Y != p1.Y))
                {
                       //points in the same column and no forbiddenCells in between
                    if ((p1.X == p2.X && !forbiddenCellPoints.Any
                        (p => p.X == p1.X && p.Y >= Math.Min(p1.Y, p2.Y) && p.Y <= Math.Max(p1.Y, p2.Y))) ||
                        //points in the same row and no forbiddenCells in between
                        (p1.Y == p2.Y && !forbiddenCellPoints.Any
                        (p => p.Y == p1.Y && p.X >= Math.Min(p1.X, p2.X) && p.X <= Math.Max(p1.X, p2.X))))
                    {
                        if (!returnValue.ContainsKey(p1))
                            returnValue.Add(p1, new List<Point>() { p2 });
                        else
                            returnValue[p1].Add(p2);
                    }                                        
                }
            }

            return returnValue;
        }

        private void showGridWithIntersections(List<Point> intersections)
        {
            for (int y = 0; y < sizeOfGrid; y++)
            {
                for (int x = 0; x < sizeOfGrid; x++)
                {                    
                    if (start.X == x && start.Y == y)
                        Console.Write('S');
                    else if (end.X == x && end.Y == y)
                        Console.Write('E');
                    else if (forbiddenCells[y][x])
                        Console.Write('X');
                    else if (intersections.Contains(new Point(x, y)))
                        Console.Write('+');
                    else
                        Console.Write('.');                    
                }
                Console.WriteLine();

            }
        }

    }
}
