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
        int size = 14;
        int m = 50;
        int n = 50;
        bool sizeFlag = false;
        bool nFlag = false;
        bool mFlag = false;
        bool newGameFlag = true;
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
            

            
            
            agent = new Thread(IniAgente);
            
            pBuild.ClearContent();
            pBuild.AppendText("Hello, my name is Sebastian, Welcome to the maze game,");
            sSynth.Speak(pBuild);

            pBuild.ClearContent();
            pBuild.AppendText("Please follow all the instructions, Use the command help to find all the instructions available, Player is green square and destination is red square");
            sSynth.Speak(pBuild);

            newGame();
            /*startGame();
            newGameFlag = false;*/
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
                pBuild.AppendText("What is the value for row m, Can be between five to twenty. thirty,forty,or fifty");

            }
            if (nFlag)
            {
                pBuild.ClearContent();
                pBuild.AppendText("What is the value for column n, Can be between five to twenty. thirty,forty,or fifty");
            }
            if (!newGameFlag)
            {
                pBuild.ClearContent();
                pBuild.AppendText("What to do now?");
            }

            if (sizeFlag)
            {
                pBuild.ClearContent();
                pBuild.AppendText("What is the size value, have to be between five to fourteen?");
            }
            if (matriz.inicioY == matriz.finalY && matriz.inicioX == matriz.finalX && !newGameFlag)
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

        public void commands(SpeechRecognizedEventArgs er)
        {
            switch (er.Result.Text)
            {

                case "north":
                    if (matriz.moverNorte())
                    {
                        pBuild.AppendText("moving up");
                    }
                    else
                    {
                        pBuild.AppendText("can't move up");
                    }
                    System.Console.WriteLine("up");
                    break;
                case "up":
                    if (matriz.moverNorte())
                    {
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
                    }
                    else
                    {
                        pBuild.AppendText("can not move right");
                    }
                    System.Console.WriteLine("right");
                    break;
                case "show route":
                    matriz.limpiarRuta();
                    if (matriz.crearRuta())
                    {
                        pBuild.AppendText("Showing route");
                    }
                    else
                    {
                        pBuild.AppendText("No route is available");
                    }

                    System.Console.WriteLine("show route");
                    break;
                case "clean":
                    matriz.limpiarRuta();
                    pBuild.AppendText("route cleaned");
                    System.Console.WriteLine("clean route");
                    break;
                case "sebastian":
                    pBuild.AppendText("yes?");
                    System.Console.WriteLine("sebastian");
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
                    pBuild.AppendText("Please set a size, Five to fourteen");
                    System.Console.WriteLine("size");
                    break;
                case "north east":
                    if (matriz.modoDiagonal)
                    {
                        if (matriz.moverNorEste())
                        {
                            pBuild.AppendText("moving north east");
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
                    
                    break;
                case "start game":
                    if (nFlag == false && mFlag == false && newGameFlag == true)
                    {
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "new end":
                    randEnd();
                    pBuild.AppendText("Destination changed");
                    break;
                case "new start":
                    randStart();
                    pBuild.AppendText("Start changed");
                    break;
                case "help":
                    pBuild.AppendText("To move upwards, use up or north," +
                        " to move downwards, use down or south, to move to " +
                        "the left, use left or west, to move to the right, " +
                        "use right or east, To show the route to the destination," +
                        " use show route, to clean the route, use clean, to " +
                        "enable diagonals, use enable diagonal, " +
                        "to disable diagonals, use disable diagonal, to change the " +
                        "size, use size, to change the destination, use new end," +
                        " to change the start, use new start, for a new game, use new game,");
                    break;
            }
        }

        public void numSelect(SpeechRecognizedEventArgs er)
        {
            switch (er.Result.Text)
            {
                case "five":
                    if (sizeFlag)
                    {
                        size = 5;
                        pBuild.AppendText("Size is set to five");
                        sizeFlag = false;
                        break;
                    }
                    if (mFlag)
                    {
                        m = 5;
                        mFlag = false;
                        nFlag = true;
                        pBuild.AppendText("m is set to five");
                        break;
                    }
                    if (nFlag)
                    {
                        n = 5;
                        nFlag = false;
                        newGameFlag = false;

                        pBuild.AppendText("n is set to five");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();

                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "six":
                    if (sizeFlag)
                    {
                        size = 6;
                        pBuild.AppendText("size is set to six");
                        sizeFlag = false;
                        break;
                    }
                    if (mFlag)
                    {
                        m = 6;
                        pBuild.AppendText("m is set to six");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 6;
                        nFlag = false;
                        newGameFlag = false;
                        pBuild.AppendText("n is set to six");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "seven":
                    if (sizeFlag)
                    {
                        size = 7;
                        pBuild.AppendText("size is set to seven");
                        sizeFlag = false;
                        break;
                    }
                    if (mFlag)
                    {
                        m = 7;
                        pBuild.AppendText("m is set to seven");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 7;
                        pBuild.AppendText("n is set to seven");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
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
                        pBuild.AppendText("size is set to eight");
                        sizeFlag = false;
                        break;
                    }
                    if (mFlag)
                    {
                        m = 8;
                        pBuild.AppendText("m is set to eight");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 8;
                        pBuild.AppendText("n is set to eight");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
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
                        pBuild.AppendText("size is set to nine");
                        sizeFlag = false;
                        break;
                    }

                    if (mFlag)
                    {
                        m = 9;
                        pBuild.AppendText("m is set to nine");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 9;
                        pBuild.AppendText("n is set to nine");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                        break;
                    }
                    break;
                case "ten":
                    if (sizeFlag)
                    {
                        size = 10;
                        pBuild.AppendText("size is set to ten");
                        sizeFlag = false;
                        break;
                    }
                    if (mFlag)
                    {
                        m = 10;
                        pBuild.AppendText("m is set to ten");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 10;
                        pBuild.AppendText("n is set to ten");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
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
                        pBuild.AppendText("size is set to eleven");
                        sizeFlag = false;
                        break;
                    }
                    if (mFlag)
                    {
                        m = 11;
                        pBuild.AppendText("m is set to eleven");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 11;
                        pBuild.AppendText("n is set to eleven");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
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
                        pBuild.AppendText("size is set to twelve");
                        sizeFlag = false;
                        break;
                    }
                    if (mFlag)
                    {
                        m = 12;
                        pBuild.AppendText("m is set to twelve");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 12;
                        pBuild.AppendText("n is set to twelve");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
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
                        pBuild.AppendText("size is set to thirteen");
                        sizeFlag = false;
                        break;
                    }
                    if (mFlag)
                    {
                        m = 13;
                        pBuild.AppendText("m is set to thirteen");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 13;
                        pBuild.AppendText("n is set to thirteen");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
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
                        pBuild.AppendText("size is set to fourteen");
                        sizeFlag = false;
                        break;
                    }
                    if (mFlag)
                    {
                        m = 14;
                        pBuild.AppendText("m is set to fourteen");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 14;
                        pBuild.AppendText("n is set to fourteen");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "fifth":

                    if (mFlag)
                    {
                        m = 15;
                        pBuild.AppendText("m is set to fifth");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 15;
                        pBuild.AppendText("n is set to fifth");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "sixteen":

                    if (mFlag)
                    {
                        m = 16;
                        pBuild.AppendText("m is set to sixteen");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 16;
                        pBuild.AppendText("n is set to sixteen");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "seventeen":

                    if (mFlag)
                    {
                        m = 17;
                        pBuild.AppendText("m is set to seventeen");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 17;
                        pBuild.AppendText("n is set to seventeen");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "eighteen":

                    if (mFlag)
                    {
                        m = 18;
                        pBuild.AppendText("m is set to eighteen");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 18;
                        pBuild.AppendText("n is set to eighteen");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "nineteen":

                    if (mFlag)
                    {
                        m = 19;
                        pBuild.AppendText("m is set to nineteen");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 19;
                        pBuild.AppendText("n is set to nineteen");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "twenty":

                    if (mFlag)
                    {
                        m = 20;
                        pBuild.AppendText("m is set to twenty");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 20;
                        nFlag = false;
                        newGameFlag = false;
                        pBuild.AppendText("n is set to twenty");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
                case "thirty":

                    if (mFlag)
                    {
                        m = 30;
                        pBuild.AppendText("m is set to thirty");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 30;
                        pBuild.AppendText("n is set to thirty");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
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
                        pBuild.AppendText("m is set to forty");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {
                        n = 40;
                        pBuild.AppendText("n is set to forty");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
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
                        pBuild.AppendText("m is set to fifty");
                        mFlag = false;
                        nFlag = true;
                        break;
                    }
                    if (nFlag)
                    {

                        n = 50;
                        pBuild.AppendText("n is set to fifty");
                        sSynth.Speak(pBuild);
                        pBuild.ClearContent();
                        nFlag = false;
                        newGameFlag = false;
                        startGame();
                        pBuild.AppendText("Game starting");
                    }
                    break;
            }
        }

        public void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs er)
        {
            pBuild.ClearContent();
            if (!sizeFlag && !mFlag && !nFlag)  
            {
                commands(er);
            }
            else
            {
                numSelect(er);
            }
            
            sSynth.Speak(pBuild);
            
            
        }
        public void randEnd()
        {
            Random rnd = new Random();
            matriz.setElementoPos(matriz.finalX, matriz.finalY, 0);
            int randX = rnd.Next(0, m);
            int randY = rnd.Next(0, n);
            matriz.setElementoPos(randX, randY, 4);
            matriz.colocarFinal(randX, randY);
        }

        public void randStart()
        {
            
            Random rnd = new Random();
            matriz.setElementoPos(matriz.inicioX, matriz.inicioY, 0);
            int tempM = rnd.Next(1, m);
            int tempN = rnd.Next(1, n);
            matriz.setElementoPos(tempM, tempN, 2);
            matriz.colocarInicio(tempM, tempN);
        }


        public void newGame()
        {
            pBuild.ClearContent();
            pBuild.AppendText("making new game");
            sSynth.Speak(pBuild);

            Choices commands = new Choices();
            commands.Add(new string[] { "start game",
                                        "five","six","seven","eight","nine","ten",
                                        "eleven","twelve","thirteen","fourteen","fifteen",
                                        "sixteen","seventeen","eighteen","nineteen","twenty","thirty","forty","fifty","help"
            });
            gBuilder = new GrammarBuilder();
            gBuilder.Append(commands);
            m = 0;
            n = 0;
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
                                        "fuck you","you suck","new end","new start","help","sebastian"
            });
            gBuilder = new GrammarBuilder();
            gBuilder.Append(commands);

            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);

            matriz = new Mapa(m, n, size);
            Random rnd = new Random();
            int tempM = rnd.Next(0, m-1);
            int tempN = rnd.Next(0, n-1);
            matriz.setElementoPos(tempM, tempN, 2);
            matriz.colocarInicio(tempM, tempN);

           
            int randX = rnd.Next(0, m-1);

            int randY = rnd.Next(0, n-1);
            matriz.setElementoPos(randX, randY, 4);
            matriz.colocarFinal(randX, randY);
            matriz.colocarObstaculos((m * n) / 6);

        }

       

		private void timer1_Tick(object sender, EventArgs e)
		{
            
            Invalidate();
        }

        private void FormView_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < m; i++)
            {
                for (int b = 0; b < n; b++)
                {
                    if (matriz.getElementoPos(i, b) == 1)
                    {
                        e.Graphics.FillRectangle(Brushes.Black, b * size, i * size, size, size);
                    }
                    if (matriz.getElementoPos(i, b) == 2)
                    {
                        e.Graphics.FillRectangle(Brushes.LightGreen, b * size, i * size, size, size);
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
