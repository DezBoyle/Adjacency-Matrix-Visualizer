using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace AdjacencyMatrix
{
    public partial class Form1 : Form
    {
        private static bool[,] matrix;
        public Form1()
        {
            InitializeComponent();

            //credit: https://stackoverflow.com/questions/32923098/how-can-i-generate-dinamically-a-button-grid-in-a-winform-application-which-fits
            var rowCount = 9;
            var columnCount = 9;

            this.tableLayoutPanel1.ColumnCount = columnCount;
            this.tableLayoutPanel1.RowCount = rowCount;

            this.tableLayoutPanel1.ColumnStyles.Clear();
            this.tableLayoutPanel1.RowStyles.Clear();

            for (int i = 0; i < columnCount; i++)
            {
                this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / columnCount));
            }
            for (int i = 0; i < rowCount; i++)
            {
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 / rowCount));
            }

            matrix = new bool[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if(i == j)
                    { continue;  }
                    var button = new CheckBox();
                    button.Click += OnButtonClickedCallback;
                    button.Name = $"button_{i}{j}";
                    button.Dock = DockStyle.Fill;
                    this.tableLayoutPanel1.Controls.Add(button, j, i);
                }
            }
        }

        private void OnButtonClickedCallback(object sender, EventArgs e)
        {
            if(sender == null)
            { return; }

            CheckBox checkBox = (CheckBox)sender;
            int x = int.Parse(checkBox.Name.Substring(7, 1));
            int y = int.Parse(checkBox.Name.Substring(8, 1));

            System.Diagnostics.Debug.WriteLine($"{x}, {y}");
            matrix[x, y] = checkBox.Checked;

            /*for(int i = 0; i < 9; i++)
            {
                for(int j= 0; j < 9; j++)
                {
                    System.Diagnostics.Debug.Write(matrix[i,j] ? "1" : "0");
                }
                System.Diagnostics.Debug.WriteLine("");
            }*/

            panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Paint");
            Pen pen = new Pen(Color.Black, 4);
            Graphics g = e.Graphics;
            Point origin = new Point(128, 32);
            List<int> vertices = new List<int>();

            //get vertices
            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    if(i == j)
                    { continue; }

                    if(matrix[i, j])
                    {
                        if(!vertices.Contains(i))
                        { vertices.Add(i); }
                        if (!vertices.Contains(j))
                        { vertices.Add(j); }
                    }
                }
            }

            if (vertices.Count == 0)
            { return; }

            //draw vertices
            Point point = origin;
            float angle = (MathF.PI * 2) / vertices.Count;
            Dictionary<int, Point> vertexPositions = new Dictionary<int, Point>();
            for (int i = 0; i < vertices.Count; i++)
            {
                vertexPositions.Add(vertices[i], point);
                g.DrawEllipse(pen, new Rectangle(point.X - 2, point.Y - 2, 4, 4));
                point.X += (int)(MathF.Cos(angle * i) * 48);
                point.Y += (int)(MathF.Sin(angle * i) * 48);
            }

            //draw lines
            foreach(int v in vertexPositions.Keys)
            {
                //check columns for adjacency
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (matrix[v, j])
                    {
                        g.DrawLine(pen, vertexPositions[v], vertexPositions[j]);
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        
    }
}