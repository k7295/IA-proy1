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
        int m = 10;
        int n = 30;
        int x = 0;
        int y = 0;
        bool sizeFlag = false;
        bool nFlag = false;
        bool mFlag = false;
        bool newGameFlag = true;
        bool gameOverFlag = false;
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
        SpeechSynthesizer  sSynth = new SpeechSynthesizer();

        GrammarBuilder gBuilder; 
        PromptBuilder pBuild = new PromptBuilder();
        Thread agent;
     
        System.Windows.Forms.Timer timRun = new System.Windows.Forms.Timer();

        public FormView()
		{
			InitializeComponent();
            matriz = new Mapa(m, n, size);
            

            //newGame();
            startGame();
            newGameFlag = false;

            agent = new Thread(IniAgente);

            pBuild.ClearContent();
            pBuild.AppendText("Welcome to the maze game,");
            sSynth.Speak(pBuild);
            pBuild.ClearContent();
            pBuild.AppendText("Please follow all the instructions");
            sSynth.Speak(pBuild);

            agent.Start();

        }

		

        public void IniAgente()
        {

            recEngine.SetInputToDefaultAudioDevice();
            recEngine.SpeechRecognized += recEngine_SpeechRecognized;
            pBuild.ClearContent();
            pBuild.AppendText("What to do now?");

            Agente();
        }

        public void Agente()
        {
            if (mFlag)
            {
                pBuild.ClearContent();
                pBuild.AppendText("What is the value for m, Can be between five to twenty. thirty,forty,or fifty");

            }
            if (nFlag)
            {
                pBuild.ClearContent();
                pBuild.AppendText("What is the value for n, Can be between five to twenty. thirty,forty,or fifty");
            }
            if (!newGameFlag)
            {
                pBuild.ClearContent();
                pBuild.AppendText("What to do now?");
            }

            if (sizeFlag)
            {
                pBuild.ClearContent();
                pBuild.AppendText("What is the size value, have to be between five to twenty?");
            }
            if (matriz.inicioY == matriz.finalY && matriz.inicioX == matriz.finalX)
            {
                pBuild.ClearContent();
                pBuild.AppendText("Congratulations, you finished the game");
                sSynth.Speak(pBuild);
                newGameFlag = true;
                newGame();
            }


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
            pBuild.ClearContent();
            switch (er.Result.Text)
            {
       
                case "north":
                    if (matriz.moverNorte())
                    {
                        y -= size;
                        pBuild.AppendText("moving up");
                    }
                    else
                    {
                        pBuild.AppendText("can't move up");
                    }
                    System.Console.WriteLine("up");
                    break;
                case "up":
                    if(matriz.moverNorte())
                    {
                        y -= size;
                        pBuild.AppendText("moving up");
                    }
                    else
                    {
                        pBuild.AppendText("can't move up");
                    }
                    System.Console.WriteLine("up");
                    break;
           
                case "south":
                    if (matriz.moverSur())
                    {
                        y += size;
                        pBuild.AppendText("moving down");
                    }
                    else
                    {
                        pBuild.AppendText("can not move down");
                    }
                    System.Console.WriteLine("down");
                    break;
                case "down":
                    if (matriz.moverSur())
                    {
                        y += size;
                        pBuild.AppendText("moving down");
                    }
                    else
                    {
                        pBuild.AppendText("can not move down");
                    }
                    System.Console.WriteLine("down");
                    break;
                
                case "west":
                    if (matriz.moverOeste())
                    {
                        x -= size;
                        pBuild.AppendText("moving left");
                    }
                    else
                    {
                        pBuild.AppendText("can not move left");
                    }
                    System.Console.WriteLine("left");
                    break;
                case "left":
                    if (matriz.moverOeste())
                    {
                        x -= size;
                        pBuild.AppendText("moving left");
                    }
                    else
                    {
                        pBuild.AppendText("can not move left");
                    }
                    System.Console.WriteLine("left");
                    break;
                case "east":
                    if (matriz.moverEste())
                    {
                        pBuild.AppendText("moving right");
                        x += size;
                    }
                    else
                    {
                        pBuild.AppendText("can not move right");
                    }
                    System.Console.WriteLine("right");
                    break;
                
                case "right":
                    if (matriz.moverEste())
                    {
                        pBuild.AppendText("moving right");
                        x += size;
                    }
                    else
                    {
                        pBuild.AppendText("can not move right");
                    }
                    System.Console.WriteLine("right");
                    break;
                case "show route":
                    matriz.limpiarRuta();
                    matriz.crearRuta();
                    pBuild.AppendText("Showing route");
                    System.Console.WriteLine("show route");
                    break;
                case "clean":
                    matriz.limpiarRuta();
                    pBuild.AppendText("route cleaned");
                    System.Console.WriteLine("clean route");
                    break;
                case "enable diagonal":
                    matriz.enableDiagonal();
                    pBuild.AppendText("Diagonal enabled");
                    System.Console.WriteLine("enable Diagonal");
                    break;
                case "disable diagonal":
                    matriz.disableDiagonal();
                    pBuild.AppendText("Diagonal disabled");
                    System.Console.WriteLine("disable diagonal");
                    break;
                case "size":
                    sizeFlag = true;
                    pBuild.AppendText("Please set a size, Five to twenty");
                    System.Console.WriteLine("size");
                    break;
                case "north east":
                    if (matriz.modoDiagonal)
                    {
                        if (matriz.moverNorEste())
                        {
                            pBuild.AppendText("moving north east");
                            y -= size;
                            x += size;
                        }
                        else
                        {
                            pBuild.AppendText("can not move north east");
                        }
                    }
                    else
                    {
                        pBuild.AppendText("Diagonal is disabled, Use, Enable diagonal, to move north east");
                    }
                    break;
                case "north west":
                    if (matriz.modoDiagonal)
                    {
                        if (matriz.moverNorOeste())
                        {
                            pBuild.AppendText("moving north west");
                            y -= size;
                            x -= size;
                        }
                        else
                        {
                            pBuild.AppendText("can not move north west");
                        }
                    }
                    else
                    {
                        pBuild.AppendText("Diagonal is disabled, Use, Enable diagonal, to move norht west");
                    }
                    break;
                case "south east":
                    if (matriz.modoDiagonal)
                    {
                        if (matriz.moverSurEste())
                        {
                            pBuild.AppendText("moving south east");
                            y += size;
                            x += size;
                        }
                        else
                        {
                            pBuild.AppendText("can not move south east");
                        }
                    }
                    else
                    {
                        pBuild.AppendText("Diagonal is disabled, Use, Enable diagonal, to move south east");
                    }
                    break;
                case "south west":
                    if (matriz.modoDiagonal)
                    {
                        if (matriz.moverSurOeste())
                        {
                            pBuild.AppendText("moving south west");
                            y += size;
                            x -= size;
                        }
                        else
                        {
                            pBuild.AppendText("can not move south west");
                        }
                    }
                    else
                    {
                        pBuild.AppendText("Diagonal is disabled, Use, Enable diagonal, to move south west");
                    }
                    break;
                case "hello":
                    pBuild.AppendText("Hi, how are u doing");
                    break;
                case "new game":
                    newGame();
                    newGameFlag = true;
                    pBuild.AppendText("making new game");
                    break;
                case "start game":
                    if(nFlag == false && mFlag == false && newGameFlag == true)
                    {
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "five":
                    if (sizeFlag)
                    {
                        size = 5;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 5;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 5;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "six":
                    if (sizeFlag)
                    {
                        size = 6;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 6;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 6;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "seven":
                    if (sizeFlag)
                    {
                        size = 7;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 7;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 7;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "eight":
                    if (sizeFlag)
                    {
                        size = 8;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 8;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 8;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }

                    break;
                case "nine":
                    if (sizeFlag)
                    {
                        size = 9;
                        sizeFlag = false;
                    }
                    
                    if (mFlag)
                    {
                        m =9;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 9;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "ten":
                    if (sizeFlag)
                    {
                        size = 10;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 10;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 10;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "eleven":
                    if (sizeFlag)
                    {
                        size = 11;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 11;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 11;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "twelve":
                    if (sizeFlag)
                    {
                        size = 12;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 12;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 12;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "thirteen":
                    if (sizeFlag)
                    {
                        size = 13;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 13;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 13;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "fourteen":
                    if (sizeFlag)
                    {
                        size = 14;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 14;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 14;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "fifth":
                    if (sizeFlag)
                    {
                        size = 15;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 15;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 15;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "sixteen":
                    if (sizeFlag)
                    {
                        size = 16;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 16;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 16;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "seventeen":
                    if (sizeFlag)
                    {
                        size = 17;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 17;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 17;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "eighteen":
                    if (sizeFlag)
                    {
                        size = 18;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 18;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 18;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "nineteen":
                    if (sizeFlag)
                    {
                        size = 19;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 19;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 19;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "twenty":
                    if (sizeFlag)
                    {
                        size = 20;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 20;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 20;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "thirty":
                    if (sizeFlag)
                    {
                        size = 30;
                        sizeFlag = false;
                    }
                    if (mFlag)
                    {
                        m = 30;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 30;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "forty":
                    if (mFlag)
                    {
                        m = 40;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 40;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    
                    break;
                case "fifty":
                    if (mFlag)
                    {
                        m = 50;
                        mFlag = false;
                        nFlag = true;
                    }
                    if (nFlag)
                    {
                        n = 50;
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "fuck you":
                    pBuild.AppendText("No, fuck you");
                    break;
            }
            
            sSynth.Speak(pBuild);
            matriz.imprimirArreglo();
            pBuild.ClearContent();
            pBuild.AppendText("What to do now?");
            
        }

        

		public void newGame()
        {
            Choices commands = new Choices();
            commands.Add(new string[] { "start game",
                                        "five","six","seven","eight","nine","ten",
                                        "eleven","twelve","thirteen","fourteen","fifteen",
                                        "sixteen","seventeen","eighteen","nineteen","twenty","thirty","forty","fifty"
            });
            gBuilder = new GrammarBuilder();
            gBuilder.Append(commands);

            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);
            mFlag = true;
            
        }

        public void startGame()
        {
            Choices commands = new Choices();
            commands.Add(new string[] { "north","up",
                                        "south","down",
                                        "west","left",
                                        "east","right",
                                        "hello",
                                        "show route" ,
                                        "enable diagonal",
                                        "disable diagonal",
                                        "new game",
                                        "clean",
                                        "size",
                                        "north east",
                                        "north west",
                                        "south east",
                                        "south west",
                                        "five","six","seven","eight","nine","ten",
                                        "eleven","twelve","thirteen","fourteen","fifteen",
                                        "sixteen","seventeen","eighteen","nineteen","twenty","thirty","forty","fifty",
                                        "fuck you"
            });
            gBuilder = new GrammarBuilder();
            gBuilder.Append(commands);

            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);

            matriz = new Mapa(m, n, size);
            Random rnd = new Random();
            int tempM = rnd.Next(1, m);
            int tempN = rnd.Next(1, n);
            y += (size * (tempM));
            x += (size * (tempN));
            matriz.setElementoPos(tempM, tempN, 2);
            matriz.colocarInicio(tempM, tempN);

           
            int randX = rnd.Next(0, m);

            int randY = rnd.Next(0, n);
            matriz.setElementoPos(randX, randY, 4);
            matriz.colocarFinal(randX, randY);
            matriz.colocarObstaculos((m * n) / 7);
            matriz.modoDiagonal = true;
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
