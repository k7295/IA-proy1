using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace TextToSpeech
{

    public partial class MainWindow : Window
    {
        
        SpeechSynthesizer _synthesizer = new SpeechSynthesizer();
        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
        List<VoiceInfo> vocesInfo = new List<VoiceInfo>(); // obtengo las informacion de las diferentes voces 

        public MainWindow()
        {
            InitializeComponent();
            foreach (InstalledVoice voice in _synthesizer.GetInstalledVoices())
            {
                vocesInfo.Add(voice.VoiceInfo); 
                comboBoxVoices.Items.Add(voice.VoiceInfo.Name);
            }
    
        }

        private void buttonSpeak_Click(object sender, RoutedEventArgs e)
        {
            int indice;


            indice = comboBoxVoices.SelectedIndex;
            String nombre = vocesInfo.ElementAt(indice).Name; // voces instaladas en mi pc que agrego al comboBox
            _synthesizer.SelectVoice("Microsoft Sabina Desktop");

            if (textBoxInput.Text == "arriba")
            {
                _synthesizer.Speak("funca");
            }
            
        }

        private void buttonHear_Click(object sender, RoutedEventArgs e)
        {
            //Inicia la escucha con el dispositivo de entrada de audio predeterminado 
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.LoadGrammar(MiGramatica()); // carga la gramatica 
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(_recognizer_SpeechRecognized);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);

        }

        void _recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            foreach (RecognizedWordUnit word in e.Result.Words)
            {
                textBoxInput.AppendText(word.Text); // convierte las palabras a texto 
            }
        }

        private static Grammar MiGramatica()
        {
            Grammar miGramatica = new Grammar(AppDomain.CurrentDomain.BaseDirectory + "//direcciones.xml");
            return miGramatica;
        }



    }
}
