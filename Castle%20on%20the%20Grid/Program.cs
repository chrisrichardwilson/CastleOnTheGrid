using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

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
            end.X = positionNumbers[3];
        }

    }
}
