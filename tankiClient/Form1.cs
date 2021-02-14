using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tankiClient
{
    public partial class Form1 : Form
    {
        Bitmap Gamer;

        public class Tank
        {
            private int id;
            private string name;
            private int speed;
            public Image tankImg;
            public PictureBox tank = new PictureBox();

            public Tank(int id, string name) 
            {
                tank.Name = "tank" + id;
                tankImg = Image.FromFile(name);
                tank.Image = tankImg;
                tank.Location = new Point(50, 50); ;
                tank.Size = new Size(125, 75);
                tank.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            public Tank(int id, string name, Point point)
            {
                tank.Name = "tank" + id;
                tankImg = Image.FromFile(name);
                tank.Image = tankImg;
                tank.Location = point;
                tank.Size = new Size(125, 75);
                tank.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            public void AddTank(int id, string name)
            {
                tank.Name = "tank" + id;
                tankImg = Image.FromFile(name);
                tank.Image = tankImg;
                tank.Location = new Point(50, 50); ;
                tank.Size = new Size(125, 75);
                tank.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            public void AddTank(int id, string name, Point point)
            {
                tank.Name = "tank" + id;
                tankImg = Image.FromFile(name);
                tank.Image = tankImg;
                tank.Location = point;
                tank.Size = new Size(125, 75);
                tank.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }


        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(600, 400);
            this.FormBorderStyle = FormBorderStyle.None;

            Point pA = new Point(100, 100);
            Tank A = new Tank(1, "tankRed.png", pA);
            Tank B = new Tank(2, "tankBlue.png");
            this.Controls.Add(A.tank);
            this.Controls.Add(B.tank);

        }
    }
}

