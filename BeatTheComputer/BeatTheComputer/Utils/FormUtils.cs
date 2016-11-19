using System.Drawing;
using System.Windows.Forms;

namespace BeatTheComputer.Utils
{
    class FormUtils
    {
        public delegate Control ControlFactory(Point drawPos, Position gridPos);

        private FormUtils() { }

        public static void createControlGrid(ControlFactory factory, ContainerControl container, Control[,] grid, int padding = 0)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            Control template = factory.Invoke(new Point(0, 0), new Position(0, 0));
            container.ClientSize = new Size(cols * template.Width + 2 * padding, rows * template.Height + 2 * padding);

            for (int row = 0; row < rows; row++) {
                for (int col = 0; col < cols; col++) {
                    Point drawPos = new Point(padding + col * template.Width, padding + (rows - row - 1) * template.Height);
                    Position gridPos = new Position(row, col);
                    grid[row, col] = factory(drawPos, gridPos);
                    container.Controls.Add(grid[row, col]);
                }
            }
        }
    }
}
