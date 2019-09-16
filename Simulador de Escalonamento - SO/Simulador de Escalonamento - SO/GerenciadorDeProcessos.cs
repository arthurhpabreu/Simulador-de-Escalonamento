using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace Simulador_de_Escalonamento___SO
{
    class GerenciadorDeProcessos
    {
        public int Quantum { get; set; } //Número para o cálculo de quanto tempo o processo ficará em execução
        public Processo ProcessoEmExecucao { get; set; } //O processo que está em execução no momento
        public int ProcessosConcluidos { get; set; }
        public int TempoMaximoDeExecucao { get; set; } //Tempo máximo que um processo pode ficar executando

        public GerenciadorDeProcessos(int Quantum, int TempoMaximoDeExecucao) 
        {
            this.Quantum = Quantum;
            this.TempoMaximoDeExecucao = TempoMaximoDeExecucao;
        }

        //Verifica se todas as filas estão vazias
        public static bool FilaDeProcessosVazia(FilaDeProcessos[] FilaP)
        {
            for (int i = 0; i < FilaP.Length; i++)
            {
                if (!FilaP[i].FilaVazia()) //se existe uma fila que NÃO esta vazia
                {
                    return false; //existem processos a serem executados - fila não está vazia
                }
            }

            return true; //se o loop executou e não retornou nada, significa que todas as filas estão vazias
        }


        public void ExecutarProcesso(FilaDeProcessos[] filaDeProcessos)
        {
            int i; //prioridade do processo executado

            do
            {
                //i é o indice do vetor de Filas
                if (!filaDeProcessos[4].FilaVazia()) //se a fila de prioridade 5 não está vazia
                    i = 4;

                else if (!filaDeProcessos[3].FilaVazia())//se a fila de prioridade 4 nao está vazia
                    i = 3;

                else if (!filaDeProcessos[2].FilaVazia()) //se a fila de prioridade 3 nao está vazia
                    i = 2;

                else if (!filaDeProcessos[1].FilaVazia()) //se a fila de prioridade 2 nao está vazia
                    i = 1;

                else if (!filaDeProcessos[0].FilaVazia()) //se a fila de prioridade 1 nao está vazia
                    i = 0;

                else//quando todas as filas estao vazias
                    i = -1;//condição de loop quebrada e fim do método

                if (i != -1)//i é diferente de -1
                    SimularExecucao(filaDeProcessos, i);
            } while (i != -1);
        }

        private void SimularExecucao(FilaDeProcessos[] F, int prioridadeDaFila)
        {
            if (!F[prioridadeDaFila].FilaVazia()) //Se a fila não está vazia
            {
                ProcessoEmExecucao = F[prioridadeDaFila].DesenfileirarProcesso();  //captura o processo a ser executado

                if (ProcessoEmExecucao == null)// se o processo é null, houve erro na execução e finaliza por aqui
                    return;

                double tempoReservadoProcessadorD = (ProcessoEmExecucao.OcupacaoCPU / 100) *  Quantum;
                int tempoReservadoProcessador = Convert.ToInt32(tempoReservadoProcessadorD); //calcula o tempo a thread irá dormir para simular a execução do processo
                int tempoJaUsado = 0;

                ProcessoEmExecucao.IniciaCiclo();  //inicia a contagem de tempo do processo

                //Ciclos podem ser partidos
                if (ProcessoEmExecucao.TempoSobra > 0) //se o tempo de sobra(processo não conseguiu completar o ciclo anteriormente, o que gera tempo de sobra de um ciclo anterior)
                {
                    if (ProcessoEmExecucao.TempoSobra > TempoMaximoDeExecucao)
                    {
                        Thread.Sleep(TempoMaximoDeExecucao);
                        ProcessoEmExecucao.TempoSobra -= TempoMaximoDeExecucao;
                        tempoReservadoProcessador = 0;
                    }

                    else //executa o resto do ciclo do processo
                    {
                        Thread.Sleep(ProcessoEmExecucao.TempoSobra);
                        tempoReservadoProcessador -= ProcessoEmExecucao.TempoSobra;
                        tempoJaUsado = ProcessoEmExecucao.TempoSobra;
                        ProcessoEmExecucao.NumeroDeCiclos--; //Ciclo completo
                        ProcessoEmExecucao.TempoSobra = 0;                       
                    }
                }

                if (tempoReservadoProcessador >= TempoMaximoDeExecucao) //se o tempo de execução for superior ao máximo tempo de execução
                {
                    Thread.Sleep(TempoMaximoDeExecucao); //Coloca a thread para dormir
                    prioridadeDaFila = ProcessoEmExecucao.ReduzirPrioridade() - 1; //reduz a prioridade do processo e seta o índice da fila de prioridade desejada  
                    ProcessoEmExecucao.TempoSobra = tempoReservadoProcessador - TempoMaximoDeExecucao;
                }

                else //executa o tempo reservado de Quantum reservado pelo processo
                {
                    int tempoRestante = tempoReservadoProcessador - tempoJaUsado;

                    if (tempoRestante > 0)
                    {
                        Thread.Sleep(tempoRestante);
                        ProcessoEmExecucao.TempoSobra = tempoReservadoProcessador;
                    }
                }

                ProcessoEmExecucao.CicloCompleto(); //Finaliza a contagem de tempo do processo em execução

                if (ProcessoEmExecucao.NumeroDeCiclos > 0) //Existem ciclos a serem executados?
                {
                    F[prioridadeDaFila].EnfileirarProcesso(ProcessoEmExecucao);//Se existem, ele insere o processo de volta na fila de espera, na ultima posição
                }

                else
                {
                    ProcessosConcluidos++;
                }
            }
        }

        public static void ControlePrioridadeEspera(FilaDeProcessos[] filaProcessos, int TempoMaximoDeEspera)
        {
            while (true)
            {
                Thread.Sleep(TempoMaximoDeEspera); //Thread dorme para não verificar a todo instante

                FilaDeProcessos [] FilasAux = new FilaDeProcessos[4]; //fila auxiliar

                for (int p = 0; p < FilasAux.Length; p++) //instânciando as filas
                    FilasAux[p] = new FilaDeProcessos();

                Monitor.Enter(filaProcessos);

                for (int x = 0; x < filaProcessos.Length - 1; x++) //percorre a fila prioridade 1 até a 4
                {
                    int nProcs = filaProcessos[x].ContadorDeProcesso;  //captura o número de processos na fila
                    int prioridadeFila = x; //define a prioridade

                    for (int u = 0; u < nProcs; u++)
                    {
                        Processo processoEmAnalise = filaProcessos[prioridadeFila].DesenfileirarProcesso(); //retira o processo da fila

                        if (processoEmAnalise.TempoEmEspera.ElapsedMilliseconds > TempoMaximoDeEspera) //verifica necessidade de subir prioridade
                        {
                            prioridadeFila = processoEmAnalise.ElevarPrioridade() - 1; //eleva a prioridade do processo e altera o valor da variavel prioridade
                            FilasAux[prioridadeFila - 1].EnfileirarProcesso(processoEmAnalise); //coloca o processo na fila auxiliar
                        }

                        else
                        {
                            filaProcessos[prioridadeFila].EnfileirarProcesso(processoEmAnalise); //coloca o processo de volta na mesma fila
                        }

                        prioridadeFila = x; //altera o valor da prioridade para evitar bugs
                    }
                }

                for (int i = 1; i < filaProcessos.Length; i++) //percorre as filas para adicionar os processos em suas devidas filas
                {
                    while (!FilasAux[i - 1].FilaVazia())
                        filaProcessos[i].EnfileirarProcesso(FilasAux[i - 1].DesenfileirarProcesso()); 
                }

                Monitor.Exit(filaProcessos);

            }
        }
    }


}