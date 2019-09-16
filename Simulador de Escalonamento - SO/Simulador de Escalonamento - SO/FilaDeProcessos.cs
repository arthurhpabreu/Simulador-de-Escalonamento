using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace Simulador_de_Escalonamento___SO
{
    class FilaDeProcessos
    {
        public ItemProcesso Frente { get; set; }
        public ItemProcesso Tras { get; set; }
        public int ContadorDeProcesso { get; set; }

        Mutex mutex = new Mutex();

        //Construtor da fila
        public FilaDeProcessos()
        {
            ItemProcesso sentinela = new ItemProcesso();

            Frente = sentinela;
            Tras = sentinela;
            ContadorDeProcesso = 0;
        }


        //Verifica se a fila esta vazia
        public bool FilaVazia()
        {
            mutex.WaitOne();

            if (Frente == Tras)
            {
                mutex.ReleaseMutex();
                return true;
            }

            else
            {
                mutex.ReleaseMutex();
                return false;
            }
        }


        //Procura um processo por índice
        public Processo BuscarProcesso(int indice)
        {
            if (!FilaVazia())
            {
                mutex.WaitOne();
                if (indice >= ContadorDeProcesso) //indice maior do que o limite
                {
                    mutex.ReleaseMutex();
                    return null;
                }

                ItemProcesso aux = Frente.Proximo;

                for (int contador = 0; contador < indice; contador++) //passa o ponteiro, até chegar no indice desejado
                    aux = aux.Proximo;

                mutex.ReleaseMutex();
                return aux._Processo;
            }


            return null;
        }

        //Enfileira um processo, colocando-o na ultima posição
        public void EnfileirarProcesso(Processo p)
        {
            mutex.WaitOne();
            ItemProcesso novoProcesso = new ItemProcesso(p);

            if (!novoProcesso._Processo.TempoEmEspera.IsRunning)
                novoProcesso._Processo.TempoEmEspera.Start();

            if (FilaVazia())
                Frente.Proximo = novoProcesso;

            Tras.Proximo = novoProcesso;
            Tras = novoProcesso;

            ContadorDeProcesso++;
            mutex.ReleaseMutex();
        }

        //Remove um processo, sendo sempre o primeiro da fila a ser removido
        public Processo DesenfileirarProcesso()
        {
            mutex.WaitOne();
            if (!FilaVazia())
            {
                ItemProcesso aux = Frente.Proximo;

                Frente.Proximo = aux.Proximo;

                aux.Proximo = null;

                if (Frente.Proximo == null)
                    Tras = Frente;

                ContadorDeProcesso--;

                mutex.ReleaseMutex();
                return aux._Processo;
            }

            else
            {
                mutex.ReleaseMutex();
                return null;
            }
        }
    }
}
