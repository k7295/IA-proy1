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
        int m = 30;
        int n = 20;
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
            sSynth.SelectVoiceByHints(VoiceGender.Female);
            matriz = new Mapa(m, n, 100);
            agent = new Thread(IniAgente);
            
            matriz.setElementoPos(0, 0, 2);
            matriz.colocarInicio(0, 0);

            Random rnd = new Random();
            int randX = rnd.Next(m-10, m);
            int randY = rnd.Next(n-10, n);
            matriz.setElementoPos(randX, randY, 4);
            matriz.colocarFinal(randX, randY);
            

            matriz.colocarObstaculos((m*n)/7);
            
            

            
         
            matriz.imprimirArreglo();
            agent.Start();

        }

		

        public void IniAgente()
        {


            Choices commands = new Choices();
            commands.Add(new string[] { "n","north","up",
                                        "s","south","down",
                                        "w","west","left",
                                        "e","east","right",
                                        "hello",
                                        "show route" ,
                                        "enable diagonal",
                                        "disable diagonal",
                                        "new"});
            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(commands);

            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);

            recEngine.SetInputToDefaultAudioDevice();
            recEngine.SpeechRecognized += recEngine_SpeechRecognized;
            pBuild.ClearContent();
            pBuild.AppendText("What to do now?");

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
                case "north":
                    if (matriz.moverNorte())
                    {
                        y -= size;
                    }
                    System.Console.WriteLine("up");
                    break;
                case "up":
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
                case "south":
                    if (matriz.moverSur())
                    {
                        y += size;
                    }
                    System.Console.WriteLine("down");
                    break;
                case "down":
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
                case "west":
                    if (matriz.moverOeste())
                    {
                        x -= size;
                    }
                    System.Console.WriteLine("left");
                    break;
                case "left":
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
                case "e":
                    if (matriz.moverEste())
                    {
                        x += size;
                    }
                    System.Console.WriteLine("right");
                    break;
                case "right":
                    if (matriz.moverEste())
                    {
                        x += size;
                    }
                    System.Console.WriteLine("right");
                    break;
                case "hello1":
                    
                    Invalidate();
                    System.Console.WriteLine("hello");
                    break;
                case "show route":
                    matriz.limpiarRuta();
                    matriz.crearRuta();
                    System.Console.WriteLine("show route");
                    break;
                case "enable diagonal":
                    matriz.enableDiagonal();
                    System.Console.WriteLine("enable Diagonal");
                    break;
                case "disable diagonal":
                    matriz.disableDiagonal();
                    System.Console.WriteLine("disable diagonal");
                    break;
                case "hello":
                    matriz = new Mapa(m, n, 100);
                    matriz.setElementoPos(0, 0, 2);
                    matriz.colocarInicio(0, 0);
             
                    Random rnd = new Random();
                    int randX = rnd.Next(m - 10, m);
                    int randY = rnd.Next(n - 10, n);
                    matriz.setElementoPos(randX, randY, 4);
                    matriz.colocarFinal(randX, randY);
                    matriz.colocarObstaculos((m * n) / 7);
                    break;
            }
            pBuild.ClearContent();
            pBuild.AppendText(er.Result.Text);
            sSynth.Speak(pBuild);

            pBuild.ClearContent();
            pBuild.AppendText("What to do now?");
            //matriz.imprimirArreglo();
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
                        e.Graphics.FillRectangle(Brushes.Black, b * size, i * size, size, size);
                    }
                    if (matriz.getElementoPos(i, b) == 5)
                    {
                        x = i * size;
                        y = i * size;
                        e.Graphics.FillRectangle(Brushes.LightGreen,x, y , size, size);
                        
                    }
                    if (matriz.getElementoPos(i, b) == 3)
                    {
                        e.Graphics.FillRectangle(Brushes.Cyan, b * size, i * size, size, size);
                    }
                    if (matriz.getElementoPos(i, b) == 4)
                    {
                        e.Graphics.FillRectangle(Brushes.Red, b * size, i * size, size, size);
                    }
                }

            }
        }

        private void FormView_Load(object sender, EventArgs e)
        {

        }
    }
}
