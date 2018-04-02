using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_IA1.Mapa;

namespace WindowsFormsApp1
{
	public partial class Form1 : Form
	{

		static Mapa matriz;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			this.DoubleBuffered = true;

			this.Paint += new PaintEventHandler(form1_Paint);

		}

		private void form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			
			e.Graphics.FillRectangle(Brushes.White,new Rectangle(0, 0, 900, 900));
			
			int m = 20;
			int n = 30;
			matriz = new Mapa(m, n, 100);
			
			
			matriz.colocarInicio(0, 0);
			matriz.colocarFinal(4, 4);
			
			matriz.colocarObstaculos(100);
			matriz.imprimirArreglo();
			//matriz.setElementoPos(0, 0, 1);
			for (int i = 0; i < m; i++)
			{
				for (int b = 0; b < n; b++)
				{
					
					if (matriz.getElementoPos(i, b) == 1)
					{
						e.Graphics.FillRectangle(Brushes.Black, new Rectangle(i*10, b*10, 10, 10));
					}

				}

			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			this.Refresh();
		}
	}
}
