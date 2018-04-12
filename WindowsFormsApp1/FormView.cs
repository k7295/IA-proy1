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
    public partial class FormView : Form
	{

		static Mapa matriz;
        int size = 10;
        int m = 20;
        int n = 10;
        int x = 0;
        int y = 0;
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
        SpeechSynthesizer  sSynth = new SpeechSynthesizer();
        PromptBuilder pBuild = new PromptBuilder();
        Thread agent;
     
        System.Windows.Forms.Timer timRun = new System.Windows.Forms.Timer();

        public FormView()
		{
			InitializeComponent();
            
            matriz = new Mapa(m, n, 100);
            agent = new Thread(IniAgente);

            matriz = new Mapa(m, n, 100);

            matriz.setElementoPos(0, 0, 2);
            matriz.setElementoPos(19, 9, 4);
            matriz.colocarInicio(0, 0);
            matriz.colocarFinal(19, 9);

            matriz.colocarObstaculos(20);

           
            agent.Start();

        }

		

        public void IniAgente()
        {


            Choices commands = new Choices();
            commands.Add(new string[] { "n", "s","w","east","hello" });
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

        public void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs er)
        {
            switch (er.Result.Text)
            {
                case "n":
                    if (matriz.moverNorte())
                    {
                        y -= size;
                    }
                    System.Console.WriteLine("up");
                    break;
                case "s":
                    if (matriz.moverSur())
                    {
                        y += size;
                    }
                    System.Console.WriteLine("down");
                    break;
                case "w":
                    if (matriz.moverOeste())
                    {
                        x -= size;
                    }
                    System.Console.WriteLine("left");
                    break;
                case "east":
                    if (matriz.moverEste())
                    {
                        x += size;
                    }
                    System.Console.WriteLine("right");
                    break;
                case "hello":
                    
                    Invalidate();
                    System.Console.WriteLine("hello");
                    break;
            }
            matriz.imprimirArreglo();
        }

        

		

       

		private void timer1_Tick(object sender, EventArgs e)
		{
            
            Invalidate();
        }

        private void FormView_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, 900, 900));

            e.Graphics.FillRectangle(Brushes.LightGreen, x, y, size, size);
            for (int i = 0; i < m; i++)
            {
                for (int b = 0; b < n; b++)
                {

                    if (matriz.getElementoPos(i, b) == 1)
                    {
                        e.Graphics.FillRectangle(Brushes.Black, i * size, b * size, size, size);
                    }
                    if (matriz.getElementoPos(i, b) == 5)
                    {
                        x = i * size;
                        y = i * size;
                        e.Graphics.FillRectangle(Brushes.LightGreen,x, y , size, size);
                        
                    }
                    if (matriz.getElementoPos(i, b) == 3)
                    {
                        e.Graphics.FillRectangle(Brushes.Cyan, i * size, b * size, size, size);
                    }
                    if (matriz.getElementoPos(i, b) == 4)
                    {
                        e.Graphics.FillRectangle(Brushes.Red, i * size, b * size, size, size);
                    }
                }

            }
        }

        private void FormView_Load(object sender, EventArgs e)
        {

        }
    }
}
