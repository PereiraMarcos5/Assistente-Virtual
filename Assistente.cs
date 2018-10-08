using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using System.Speech.Synthesis;
using System.Globalization;
using System.Diagnostics;

namespace Jarvis

{
    public partial class Assistente : Form
    {
        static CultureInfo ci = new CultureInfo("pt-BR");
        static SpeechRecognitionEngine reconhecedor;
        SpeechSynthesizer resposta = new SpeechSynthesizer();

        public string[] listaPalavras = {"qual o seu nome", "como você está", " que dia é hoje" , "que horas são", "horas" , "Tchau Jarvis", "abra o facebook", "atualize","abra o youtube","parar","continue",
        "olá Jarvis","Bom dia","Boa Tarde","Boa Noite","eu vou bem", "maximizar", "minimizar", "facebook", "área de trabalho", "meu computador", "Desligar computador", "reiniciar computador", "close",
        "Abrir gerenciador de tarefas", "Abrir OneDrive", "Abrir Calculadora", "Me mostre as Noticias", "Quais são as Noticias","pausar","pause","quem é você", "me conte uma historia","trocar voz","trocar pessoa"}; //preencha com os comandos de voz, ex. tudo bem?, como é o seu nome?.

        public Assistente()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            //resposta.SelectVoice("Microsoft Anna");
            resposta.Volume = 100; // controla volume de saida
            resposta.Rate = 2; // velocidade de fala

            Gramatica(); // inicialização da gramatica
        }

        public void Gramatica()
        {
            try
            {
                //reconhecedor = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-us"));
                reconhecedor = new SpeechRecognitionEngine(ci);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO ao integrar lingua escolhida:" + ex.Message);
            }

            // criacao da gramatica simples que o programa vai entender
            // usando um objeto Choices
            var gramatica = new Choices();
            gramatica.Add(listaPalavras); // inclui a gramatica criada

            // cria o construtor gramatical
            // e passa o objeto criado com as palavras
            var gb = new GrammarBuilder();
            gb.Append(gramatica);

            // cria a instancia e carrega a engine de reconhecimento
            // passando a gramatica construida anteriomente
            try
            {
                var g = new Grammar(gb);

                try
                {
                    // carrega o arquivo de gramatica
                    reconhecedor.RequestRecognizerUpdate();
                    reconhecedor.LoadGrammarAsync(g);

                    // registra a voz como mecanismo de entrada para o evento de reconhecimento
                    reconhecedor.SpeechRecognized += Sre_Reconhecimento;

                    reconhecedor.SetInputToDefaultAudioDevice(); // microfone padrao
                    resposta.SetOutputToDefaultAudioDevice(); // auto falante padrao
                    reconhecedor.RecognizeAsync(RecognizeMode.Multiple); // multiplo reconhecimento
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERRO ao criar reconhecedor: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO ao criar a gramática: " + ex.Message);
            }
        }

        private void Sre_Reconhecimento(object sender, SpeechRecognizedEventArgs e) //Respostas das perguntas contidas no array listaPalavras.
        {
            string frase = e.Result.Text;


            if (frase.Equals("olá") || frase.Equals("Bom dia") || frase.Equals("Boa Tarde") || frase.Equals("Boa Noite"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Olá senhor, como você está");
            }
            if (frase.Equals("eu vou bem"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("fico feliz de ouvir isso senhor");
            }
            if (frase.Equals("qual o seu nome") || frase.Equals("quem é você"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("eu  sou uma  inteligência  artificial  criada  pelo Senhor");
            }
            if (frase.Equals("me conte uma historia"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync(@"Era uma vez, três porquinhos que saíram da casa de sua mãe. Cada um construiria a sua própria casa. Seguiram caminhos diferentes.
O primeiro porquinho construiu a sua casa com palha.Logo ficou pronta e ele foi dormir.Chegou um lobo que queria comer o porquinho e disse: --Abra a porta ou derrubarei esta casa com um sopro só!
O porquinho não abriu.O lobo soprou e derrubou a casa.O porquinho fugiu.
O segundo porquinho fez a sua casa com galhos de árvore.Logo ficou pronta e ele foi dormir.Outra vez veio o lobo.
Porquinho, abra a porta ou vou assoprar e derrubar tudo.O porquinho não abriu, o lobo assoprou e derrubou a casa.Mas o porquinho fugiu e se escondeu, e o lobo queria saber:
Onde se meteu este porquinho ?
O terceiro porquinho construiu a sua casa com tijolos.Para lá, foram os seus irmãos e o lobo também.Mas, desta vez, o lobo soprou até cansar e não derrubou a casa.
O lobo resolveu descer pela chaminé, mas a lareira estava acesa e ele saiu pegando fogo.O lobo foi embora e os porquinhos ficaram muito felizes, morando na casinha de tijolos.");
            }
            if (frase.Equals("como você está"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("eu estou muito bem, obrigado");
            }
            if (frase.Equals("que dia é hoje"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Hoje é " + DateTime.Now.DayOfWeek.ToString() + "do dia" + DateTime.Now.Day.ToString() + " do Mês " + DateTime.Now.Month.ToString() + " Do ano " + DateTime.Now.Year.ToString());
            }
            if (frase.Equals("hora"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync(DateTime.Now.Hour.ToString() + "horas e" + DateTime.Now.Minute.ToString() + "Minutos");
            }
            if (frase.Equals("que horas são"))
            {
                resposta.SpeakAsync(DateTime.Now.Hour.ToString() + "horas e" + DateTime.Now.Minute.ToString() + "Minutos");
            }
            if (frase.Equals("Tchau") || frase.Equals("close"))
            {
                if (resposta.SpeakAsync("Até logo").IsCompleted == false)
                {

                }
                Close();
            }
            if (frase.Equals("abra o facebook") || frase.Equals("facebook"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Sim, claro");
                System.Diagnostics.Process.Start("chrome.exe", "facebook.com");
            }
            if (frase.Equals("abra o youtube"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Sim, claro");
                System.Diagnostics.Process.Start("chrome.exe", "youtube.com");
            }
            if (frase.Equals("atualize"))
            {
                Close();
                System.Diagnostics.Process.Start("ComandoPorVoz.exe");
            }
            if (frase.Equals("parar"))
            {
                resposta.SpeakAsyncCancelAll();
            }
            if (frase.Equals("pausar") || frase.Equals("pause"))
            {
                resposta.Pause();
            }
            if (frase.Equals("continue"))
            {
                resposta.Resume();
            }
            if (frase.Equals("maximizar"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Sim, claro");
                WindowState = FormWindowState.Maximized;
            }
            if (frase.Equals("minimizar"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Sim, claro");
                WindowState = FormWindowState.Minimized;
            }
            if (frase.Equals("área de trabalho"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Sim, claro");
                Process.Start("Área de Trabalho");
            }
            if (frase.Equals("meu computador"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Sim, senhor");
                System.Diagnostics.Process.Start("Computador");
            }
            if (frase.Equals("Desligar computador"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Sim, senhor");
                System.Diagnostics.Process.Start("shutdown", "/s /t 0");
            }
            if (frase.Equals("Reiniciar computador"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Sim, senhor");
                System.Diagnostics.Process.Start("shutdown", "/r /t 0");
            }
            if (frase.Equals("Abrir gerenciador de tarefas"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Sim, senhor");
                System.Diagnostics.Process.Start("taskmgr.exe");
            }
            if (frase.Equals("Abrir OneDrive"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Sim, senhor");
                System.Diagnostics.Process.Start("C:/Users/Marcos Vini/OneDrive");
            }
            if (frase.Equals("Abrir Calculadora"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Sim, senhor");
                System.Diagnostics.Process.Start("calc");
            }
            if (frase.Equals("Me mostre as Noticias") || frase.Equals("Quais são as Noticias"))
            {
                resposta.SpeakAsyncCancelAll();
                resposta.SpeakAsync("Sim, claro senhor");
                System.Diagnostics.Process.Start("chrome", "globo.com");
            }
            if (frase.Equals("trocar voz") || frase.Equals("trocar pessoa"))
            {
                resposta.SelectVoice("ScanSoft Raquel_Full_22kHz");
            }
            
        }

        private void Jarvis_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}

