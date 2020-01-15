using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterpreterPlugin
{
    public partial class TreeForm : Form
    {
        private Graphics _graph;
        private Pen _pen;
        private Plugin.Node _stNode;
        private Size _sizeOfRect;
        private SolidBrush _brush;
        private Font _font;
        private int _distX = 50;
        private int _distY = 50;
        private Dictionary<Point, bool> _usedRect;
        private Form pForm;
        private Point _maxPoint;

        public TreeForm(Plugin.Node node, Form pForm)
        {
            InitializeComponent();
            _stNode = node;
            this.pForm = pForm;
            pForm.FormClosing += clForm;
        }

        private void TreeForm_Shown(object sender, EventArgs e)
        {
            _pen = new Pen(Color.Black);
            _graph = this.CreateGraphics();
            _sizeOfRect = new Size(100, 70);
            _brush = new SolidBrush(Color.Black);
            _font = new Font("Arial", 10);
            _usedRect = new Dictionary<Point, bool>();

            Point p = new Point(0, 0);
            _maxPoint = new Point(0, 0);

            paint(_stNode, p, p);

            _pen.Dispose();
            _graph.Dispose();
            _brush.Dispose();
        }

        private void test(object sender, EventArgs e)
        {
            _pen = new Pen(Color.Black);
            _graph = this.CreateGraphics();
            _sizeOfRect = new Size(50, 30);
            _brush = new SolidBrush(Color.Black);
            _font = new Font("Arial", 14);

            Point p = new Point(0, 0);

            paint(_stNode, p, p);

            _pen.Dispose();
            _graph.Dispose();
            _brush.Dispose();
        }

        private void paint(Plugin.Node n, Point parPoint, Point paintPoint)
        {
            drawRectangle(paintPoint);
            drawString(new Point(paintPoint.X + 5, paintPoint.Y + 5), n.tn.ToString());
            if (n.val != null)
                drawString(new Point(paintPoint.X + 5, paintPoint.Y + 30), "Value: " + n.val);
            if (parPoint != paintPoint)
            {
                drawLine(new Point(parPoint.X + _sizeOfRect.Width / 2, parPoint.Y + _sizeOfRect.Height), new Point(paintPoint.X + _sizeOfRect.Width / 2, paintPoint.Y));
            }

            for (int i = 0, j = 0; j < n.childs.Count; ++i)
            {
                int distX = (_sizeOfRect.Width + _distX) * i;
                int distY = _sizeOfRect.Height + _distY;
                Point nPoint = new Point(paintPoint.X + distX, paintPoint.Y + distY);
                if (_usedRect.ContainsKey(nPoint))
                    continue;
                else
                    _usedRect.Add(nPoint, true);
                paint(n.childs[j], paintPoint, nPoint);
                ++j;
            }
        }

        private int max(int a, int b)
        {
            if (a > b)
                return a;
            else
                return b;
        }

        private void drawRectangle(Point p)
        {
            _maxPoint = new Point(max(_maxPoint.X, p.X), max(_maxPoint.Y, p.Y));
            _graph.DrawRectangle(_pen, new Rectangle(p, _sizeOfRect));
        }

        private void drawLine(Point st, Point en)
        {
            _graph.DrawLine(_pen, st, en);
        }

        private void drawString(Point p, string str)
        {
            StringFormat drawFormat = new StringFormat();
            _graph.DrawString(str, _font, _brush, p, drawFormat);
        }

        private void clForm(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
