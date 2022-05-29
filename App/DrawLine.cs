using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Math;

namespace mod_task10_ufo
{
    public partial class Form1 : Form
    {
        static int factorial(int x)
        {
            if (x <= 0)
            {
                return 1;
            }
            else
            {
                return x * factorial(x - 1);
            }
        }
        int line_x1 = 0;
        int line_y1 = 0;
        int line_x2= 400;
        int line_y2 = 200;
        int point_size = 1;
        public Form1()
        {
            InitializeComponent();
            this.Paint +=  new PaintEventHandler(paint_approx_line);
        }
        void paint_ideal_line(object sender, PaintEventArgs e){
            Graphics g =  e.Graphics;
            Pen pen  = new Pen(Color.Black, 2);
            GraphicsState gs;
            gs = g.Save();
            g.ScaleTransform(0.5f, 0.5f);
            g.DrawLine(pen, line_x1, line_y1, line_x2, line_y2);
            g.Restore(gs);
        }
        double taylor_series_sin(double x, int degree=5){
            double res = 0;
            for(int i = 1; i < degree + 1; i++){
                res += Math.Pow(-1, i - 1) * Math.Pow(x, 2 * i - 1) / factorial(2 * i - 1);
            }
            return res;
        }
        double taylor_series_cos(double x, int degree=5){
            double res = 0;
            for(int i = 1; i < degree + 1; i++){
                res += Math.Pow(-1, i - 1) * Math.Pow(x, 2 * i - 2) / factorial(2 * i - 2);
            }
            return res;
        }
        double taylor_series_arctan(double x, int degree=5){
            double res = 0;
            if( -1 <= x &&  x <= 1){
                for(int i = 1; i < degree + 1; i++){
                    res += Math.Pow(-1, i - 1) * Math.Pow(x, 2 * i - 1) / (2 * i - 1);
                }
            } 
            else{
                if( x >= 1){
                    res += PI / 2;
                    for(int i = 0; i < degree; i++){
                        res -= Pow(-1, i) / ((2 * i + 1) * Pow(x, 2 * i + 1));
                    }
                } 
                else {
                    res -= PI / 2;
                    for(int i = 0; i < degree; i++){
                        res -= Pow(-1, i) / ((2 * i + 1) * Pow(x, 2 * i + 1));
                    }
                }
            }
            return res;
        }
        void paint_approx_line(object sender, PaintEventArgs e){
            double x = line_x1;
            double y = line_y1;
            double step = 1;
            int approx_degree = 2;
            Graphics g =  e.Graphics;
            Pen pen  = new Pen(Color.Black, 2);
            GraphicsState gs;
            gs = g.Save();
            g.ScaleTransform(0.5f, 0.5f);
            double min_distance = 1000000;
            double angle = taylor_series_arctan((double)(line_y2 - line_y1) / (line_x1 - line_x2), approx_degree);
            while(true){
                y -= taylor_series_sin(angle, approx_degree) * step;
                x += taylor_series_cos(angle, approx_degree) * step;
                g.DrawEllipse(pen, (int)x, (int)y, point_size, point_size);
                if (l1_distance(x, y, line_x2, line_y2) > min_distance){
                    g.DrawString(min_distance.ToString(), new Font("Arial", 20), new SolidBrush(Color.Black), 100, 0);
                    break;
                }
                else{
                    min_distance = l1_distance(x, y, line_x2, line_y2);
                }
            }
            g.Restore(gs);
        }
        double l2_distance(double x1, double y1, double x2, double y2){
            return Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
        double l1_distance(double x1, double y1, double x2, double y2){
            return Abs((x1 - x2)) + Abs((y1 - y2));
        }

    }
}
