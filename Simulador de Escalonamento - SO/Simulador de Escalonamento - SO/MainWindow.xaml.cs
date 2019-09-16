using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;

namespace Simulador_de_Escalonamento___SO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*Dispatcher: Propriedade de uma Thread
         *Invoke: Método do Dispatcher que chama um delegate Action
         *Delegate: São referencias para funções. Semelhante a uma váriavel, porém ao invés de armazenar um valor, armazena um método
         */

        GerenciadorDeProcessos[] gerenciadorDeProcessos; //vetor de gerenciadores que executam os processos
        //cada vetor mostra qual processo está sendo executado

        FilaDeProcessos[] FilaProcessos { get; set; } = new FilaDeProcessos[5]; //vetor de filas
        int totalProcesso;//processos a serem executados
        int nThreadExibindo;//numero da thread a ser exibida os dados
        int tempoInterface; //tempo de atualização dos dados na tela

        Thread[] ThreadDeExecucao; //Vetor de threads que determina quantas threads executaram os processos
        Thread ControlePrioridadeEspera;
        Thread ThreadInterface;

        GeradorDeProcessos produtor = new GeradorDeProcessos();//objeto que preenche as filas


        public MainWindow()
        {
            
            InstanciarFilas();
            InitializeComponent();
        }

        //método que muda a interface de tempos em tempos
        private void AtualizaInterface()
        {
            while (!GerenciadorDeProcessos.FilaDeProcessosVazia(FilaProcessos)) //Enquanto houver processos a serem executados
            {
                try
                {
                    Dispatcher.Invoke(
                    new Action(() =>
                    {
                        nThreadExibindo = ComboBoxThreadExibicao.SelectedIndex;
                    }));

                    AtualizarValores(nThreadExibindo);//Tabela é atualizada
                    Thread.Sleep(tempoInterface);//Thread dorme por tempo determinado pelo usuario
                }

                catch (TaskCanceledException) //exceção disparada quando tentam finalizar o programa antes das tarefas serem concluidas
                {
                    return; //finaliza a thread
                }
            }

            Dispatcher.Invoke(
                new Action(() =>
                {
                    DataGridGerenciador.Items.Clear();//Limpa a tela para remover os processos que restaram no DataGrid
                    MessageBox.Show("Todos os processos foram concluídos ",TotalProcessosConcluidos().ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                    AtualizarControles(true);//libera e limpa todos os controles

                    LabelNomeProcesso.Content = "";
                    LabelPID.Content = "";
                    LabelPrioridade.Content = "";
                    LabelTempoExec.Content = "";

                    InterromperThreads();
                }));
        }

        //Método que instancia todas as filas de todas as prioridades
        private void InstanciarFilas()
        {
            for (int i = 0; i < FilaProcessos.Length; i++)
                FilaProcessos[i] = new FilaDeProcessos();
        }

        //Evento de click do botão de iniciar a simulação
        private void BtnIniciaSimulacao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //captura os valores dados pelo usuario
                int q = int.Parse(TxtQuantum.Text);
                int threads = (Convert.ToInt32(ComboBoxNumeroThreads.SelectedIndex + 1)); //número de threads
                tempoInterface = int.Parse(TxtTempoInterface.Text); //tempo de atualização da interface
                int tempoVerificacao = int.Parse(TxtTempoVerif.Text); //tempo de verificação de execução
                int tempoEspera = int.Parse(TxtTempoEspera.Text); //tempo máximo de espera antes de subir a prioridade
                totalProcesso = 0;

                //Tratamento de erros
                if (q <= 0)
                {
                    MessageBox.Show("Quantum inválido. O tempo deve ser superior a 0.");
                }

                else if (tempoInterface <= 0)
                {
                    MessageBox.Show("Tempo de atualização de interface inválido. O tempo deve ser superior a 0.");
                }

                else if (tempoVerificacao <= 0)
                {
                    MessageBox.Show("Tempo de verificação de prioridades inválido. O tempo deve ser superior a 0.");
                }

                else if (threads <=0)
                {
                    MessageBox.Show("Indique um número de threads.");
                }


                //bloco executado quando não há erros
                else //sem erros de números inferiores a 0
                {
                    //declarando tamanho dos vetores de threads e gerenciadores de processos
                    gerenciadorDeProcessos = new GerenciadorDeProcessos[threads];
                    ThreadDeExecucao = new Thread[threads];

                    //preenche as filas com os processos necessários
                    produtor.PreencherFilaDeProcessos(FilaProcessos);

                    //loop para descobrir quantos processos necessitam de serem executados
                    foreach (FilaDeProcessos fila in FilaProcessos)
                        totalProcesso += fila.ContadorDeProcesso;

                    //instanciando todos os gerenciadores de processo
                    for (int ax = 0; ax < threads; ax++)
                    {
                        gerenciadorDeProcessos[ax] = new GerenciadorDeProcessos(q, tempoVerificacao);
                    }

                    int a = 0;

                    //inicia as threads de execução
                    foreach (GerenciadorDeProcessos g in gerenciadorDeProcessos)
                    {
                        ThreadDeExecucao[a] = new Thread(() => g.ExecutarProcesso(FilaProcessos));
                        ThreadDeExecucao[a].Start();
                        a++;
                    }

                    ControlePrioridadeEspera = new Thread(() => GerenciadorDeProcessos.ControlePrioridadeEspera(FilaProcessos, tempoEspera));
                    ControlePrioridadeEspera.Start();

                    ThreadInterface = new Thread(AtualizaInterface);

                    //Adiciona items no ComboBox de Threads, de acordo com o numero de Threads que o usuario solicitar
                    for (int ab = 0; ab < threads; ab++)
                        ComboBoxThreadExibicao.Items.Add(ab + 1);

                    ComboBoxThreadExibicao.SelectedIndex = 0;
                    ThreadInterface.Start();
                    AtualizarControles(false);
                }
            }

            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Atualiza os controles de acordo com a necessidade
        //Desabilita botões e caixas de textos, para impedir que o usuário use comandos em momentos inapropriados
        private void AtualizarControles(bool valor)
        {
            BtnIniciaSimulacao.IsEnabled = valor;
            TxtQuantum.IsEnabled = valor;
            TxtTempoInterface.IsEnabled = valor;
            TxtTempoVerif.IsEnabled = valor;
            ComboBoxNumeroThreads.IsEnabled = valor;
            TxtTempoEspera.IsEnabled = valor;
            ComboBoxThreadExibicao.IsEnabled = !valor;
            BtnPausar.IsEnabled = !valor;
            Btn_Interromper.IsEnabled = !valor;

            //Se o boleano for verdadeiro, significa que os botões estão sendo ligados novamente
            //Portanto, as caixas precisam ser limpas
            if (valor)
            {
                ComboBoxThreadExibicao.Items.Clear();
            }
        }

        private int TotalProcessosConcluidos()
        {
            int total = 0;
            foreach (GerenciadorDeProcessos g in gerenciadorDeProcessos)
                total += g.ProcessosConcluidos;

            return total;
        }

        //Atualiza os valores da tela
        private void AtualizarValores(int indice)
        {
            Dispatcher.Invoke(
                new Action(() =>
                {
                    DataGridGerenciador.Items.Clear(); //limpa o grid
                    for (int i = (FilaProcessos.Length - 1); i >= 0; i--)
                    {
                        Monitor.Enter(FilaProcessos);
                        for (int u = 0; u < FilaProcessos[i].ContadorDeProcesso; u++)
                        {
                            DataGridGerenciador.Items.Add(FilaProcessos[i].BuscarProcesso(u));
                        }
                        Monitor.Exit(FilaProcessos);

                        Processo processoExecutando = gerenciadorDeProcessos[indice].ProcessoEmExecucao;

                        LabelNomeProcesso.Content = processoExecutando.NomeDoProcesso;
                        LabelPID.Content = processoExecutando.PID;
                        LabelPrioridade.Content = processoExecutando.Prioridade;
                        LabelTempoExec.Content = processoExecutando.TempoEmExecucao.ElapsedMilliseconds + "ms";
                        LabelCiclco.Content = processoExecutando.NumeroDeCiclos;
                    }
                }));

        }


        private void InterromperThreads()
        {
            //Finaliza todas as threads para permitir a finalização do programa sem problemas
            if (ThreadDeExecucao != null)
            {
                foreach (Thread t in ThreadDeExecucao)
                {
                    try
                    {
                        if (t.ThreadState.ToString() == "Suspended") //se a thread estiver suspensa
                            t.Resume();//resume a thread

                        if (t != null)
                            t.Abort();//aborta a thread
                    }

                    catch (ThreadStateException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            if (ThreadInterface != null)
            {
                try
                {
                    ThreadInterface.Abort();
                }

                catch (ThreadStateException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (ControlePrioridadeEspera != null)
            {
                try
                {
                    ControlePrioridadeEspera.Abort();
                }

                catch (ThreadStateException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //Interrompe as threads assim de sair
            InterromperThreads();
        }

        private void BtnPausar_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < gerenciadorDeProcessos.Length; i++)
            {
                ThreadDeExecucao[i].Suspend();//suspende a thread de execução
                gerenciadorDeProcessos[i].ProcessoEmExecucao.TempoEmExecucao.Stop();
            }

            foreach (FilaDeProcessos f in FilaProcessos)
            {
                for (int i = 0; i < f.ContadorDeProcesso; i++)
                    f.BuscarProcesso(i).TempoEmEspera.Stop();
            }

            ControlePrioridadeEspera.Suspend();

            BtnPausar.IsEnabled = false;
            BtnResumir.IsEnabled = true;
        }

        private void BtnResumir_Click(object sender, RoutedEventArgs e)
        {
            Monitor.Enter(FilaProcessos);
            for (int i = 0; i < gerenciadorDeProcessos.Length; i++)
            {
                ThreadDeExecucao[i].Resume();
                gerenciadorDeProcessos[i].ProcessoEmExecucao.TempoEmExecucao.Start();
            }

            foreach (FilaDeProcessos f in FilaProcessos)
            {
                for (int i = 0; i < f.ContadorDeProcesso; i++)
                    f.BuscarProcesso(i).TempoEmEspera.Start();
            }

            ControlePrioridadeEspera.Resume();

            BtnPausar.IsEnabled = true;
            BtnResumir.IsEnabled = false;
            Btn_Interromper.IsEnabled = false;
            Monitor.Exit(FilaProcessos);
        }

        private void Btn_Interromper_Click(object sender, RoutedEventArgs e)
        {
            InterromperThreads();//interrompe todas as threads
            DataGridGerenciador.Items.Clear(); //limpa o grid de processos
            AtualizarControles(true);//atualiza os controles
            InstanciarFilas();//reseta todas as filas
            //limpa todos os labels
            LabelNomeProcesso.Content = "";
            LabelPID.Content = "";
            LabelPrioridade.Content = "";
            LabelTempoExec.Content = "";
        }
    }
}
