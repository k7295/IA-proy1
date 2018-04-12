using System;
using System.Drawing;
using System.Windows.Forms;
using Proyecto_IA1.Mapa;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Globalization;
using System.Threading;
using Google.Apis.Speech;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
	{

		static Mapa matriz;
        int m = 20;
        int n = 10;
        System.Windows.Forms.PaintEventArgs e;
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
        SpeechSynthesizer  sSynth = new SpeechSynthesizer();
        PromptBuilder pBuild = new PromptBuilder();
        Thread y;
        Thread mapa;

        public Form1()
		{
			InitializeComponent();
            
            matriz = new Mapa(m, n, 100);
            y = new Thread(IniAgente);

            mapa = new Thread(mostrarMapaAux);
            
        }

		private void Form1_Load(object sender, EventArgs e)
		{
			this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(form1_Paint);

            


        }

        public void IniAgente()
        {
            Choices commands = new Choices();
            commands.Add(new string[] { "up", "down","left","right","hello" });
            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(commands);

            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);
            recEngine.SetInputToDefaultAudioDevice();
            recEngine.SpeechRecognized += recEngine_SpeechRecognized;
            pBuild.ClearContent();
            pBuild.AppendText("Where to move?");

            Agente();
        }

        public void Agente()
        {
            
            sSynth.Speak(pBuild);
            DateTime localDate = DateTime.Now;
            recEngine.RecognizeAsync();
            while (true)
            {
                DateTime localDatenew = DateTime.Now;
                if ((localDatenew - localDate).TotalSeconds > 5)
                {
                    break;
                }
            }
            recEngine.RecognizeAsyncStop();
            recEngine.SpeechRecognized += recEngine_SpeechRecognized;
            Agente();
        }

        public void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "up":
                    matriz.moverNorte();
                    System.Console.WriteLine("up");
                    break;
                case "down":
                    matriz.moverSur();
                    System.Console.WriteLine("down");
                    break;
                case "left":
                    matriz.moverOeste();
                    System.Console.WriteLine("left");
                    break;
                case "right":
                    matriz.moverEste();
                    System.Console.WriteLine("right");
                    break;
                case "hello":
                    matriz.moverEste();
                    System.Console.WriteLine("hello");
                    break;
            }
            matriz.imprimirArreglo();
        }

        void mostrarMapaAux()
        {
            mostrarMapa();
            DateTime localDate = DateTime.Now; 
            while (true)
            {
                DateTime localDatenew = DateTime.Now;
                if ((localDatenew - localDate).TotalSeconds > 10)
                {
                    break;
                }
            }
            
            mostrarMapaAux();
        }

		private void form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{

            
		

			matriz = new Mapa(m, n, 100);
            this.e = e;
			matriz.setElementoPos(0, 0, 2);
            matriz.setElementoPos(19, 9, 4);
			matriz.colocarInicio(0, 0);
			matriz.colocarFinal(19, 9);
			
			matriz.colocarObstaculos(20);
			

            //mostrarMapa();

            matriz.moverEste();
            
            matriz.moverSur(); 
            matriz.imprimirArreglo();
            mostrarMapa();
            y.Start();
            //mapa.Start();

            //matriz.dijkstra();
            /*
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
            }*/

            //matriz.setElementoPos(0, 0, 1);

        }

        public void mostrarMapa()
        {
            this.e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, 900, 900));
            for (int i = 0; i < m; i++)
            {
                for (int b = 0; b < n; b++)
                {

                    if (matriz.getElementoPos(i, b) == 1)
                    {
                        e.Graphics.FillRectangle(Brushes.Black, new Rectangle(i * 10, b * 10, 10, 10));
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
            mostrarMapa();
		}
	}
}
