using System;
using System.Collections;
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
			int n = 10;
			matriz = new Mapa(m, n, 100);
			
			matriz.setElementoPos(0, 0, 2);
            matriz.setElementoPos(19, 9, 4);
			matriz.colocarInicio(0, 0);
			matriz.colocarFinal(19, 9);
			
			matriz.colocarObstaculos(20);
			matriz.imprimirArreglo();

            matriz.dijkstra();

            if (matriz.existeSolucion())
            {
                ArrayList lista = matriz.demeSolucion();
                System.Console.WriteLine("El tamaño de la solucion es: " + lista.Count);

                System.Console.WriteLine();
                System.Console.WriteLine("-----------------------------------------------------------------");
                System.Console.WriteLine("La solucion es:");
                System.Console.WriteLine("-----------------------------------------------------------------");
                System.Console.WriteLine("X Y");

                foreach (int[] d in lista)
                {
      
                    if (matriz.getElementoPos(d[0], d[1]) == 0)
                    {
                        System.Console.WriteLine(d[0] + " " + d[1]);
                        matriz.setElementoPos(d[0], d[1], 3);
                    }
                }
            }
            else
            {
                System.Console.WriteLine("No existe solucion.");
            }
            
            //matriz.setElementoPos(0, 0, 1);
            for (int i = 0; i < m; i++)
			{
				for (int b = 0; b < n; b++)
				{
					
					if (matriz.getElementoPos(i, b) == 1)
					{
						e.Graphics.FillRectangle(Brushes.Black, new Rectangle(i*10, b*10, 10, 10));
					}
                    if (matriz.getElementoPos(i, b) == 2)
                    {
                        e.Graphics.FillRectangle(Brushes.LightGreen, new Rectangle(i * 10, b * 10, 10, 10));
                    }
                    if (matriz.getElementoPos(i, b) == 3)
                    {
                        e.Graphics.FillRectangle(Brushes.Cyan, new Rectangle(i * 10, b * 10, 10, 10));
                    }
                    if (matriz.getElementoPos(i, b) == 4)
                    {
                        e.Graphics.FillRectangle(Brushes.Red, new Rectangle(i * 10, b * 10, 10, 10));
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
