using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace Simulador_de_Escalonamento___SO
{
    class GeradorDeProcessos
    {
        StreamReader reader;
        string fileName = "lista_de_processos.txt";
        
        //Construtor
        public GeradorDeProcessos()
        {
            if (File.Exists(fileName))
            {
               reader = new StreamReader(fileName);
            }
        }

        //Metódo que monta um novo processo e retorna esse processo
        private Processo MontaNovoProcesso()
        {
            string linha = reader.ReadLine();

            string[] buffer = linha.Split(';', ',');

            int PID = 0, prioridade = 0, numeroDeCiclos = 0;
            int ocupacaoCPU = 0;
            string nome="VAZIO";

            if (buffer.Length == 6)
            {
                PID = int.Parse(buffer[0]);
                prioridade = int.Parse(buffer[2]);
                ocupacaoCPU = int.Parse(buffer[4]);
                numeroDeCiclos = int.Parse(buffer[5]);
                nome = buffer[1];
            }

            else if (buffer.Length == 5)
            {
                PID = int.Parse(buffer[0]);
                prioridade = int.Parse(buffer[2]);
                ocupacaoCPU = int.Parse(buffer[3]);
                numeroDeCiclos = int.Parse(buffer[4]);
                nome = buffer[1];
            }

            Processo p = new Processo(PID, nome, prioridade, ocupacaoCPU, numeroDeCiclos);

            return p;
        }

        public void PreencherFilaDeProcessos(FilaDeProcessos [] f)
        {
            if (File.Exists(fileName)) //verifica se o arquivo existe
            {
                reader = new StreamReader(fileName); //instância o objeto

                while (!reader.EndOfStream) //enquanto o arquivo não tiver sido lido completamente
                {
                    int prioridade = -1;
                    Processo processo = MontaNovoProcesso(); //no método MontaNovoProcesso ocorre a leitura do arquivo e montagem do processo

                    switch (processo.Prioridade) //switch para definir em qual fila de prioridade o processo deve ser inserido
                    {
                        case 1:
                            prioridade = 0;
                            break;

                        case 2:
                            prioridade = 1;
                            break;

                        case 3:
                            prioridade = 2;
                            break;

                        case 4:
                            prioridade = 3;
                            break;

                        case 5:
                            prioridade = 4;
                            break;
                    }

                    try
                    {
                        f[prioridade].EnfileirarProcesso(processo);
                    }

                    catch (IndexOutOfRangeException)
                    {
                        MessageBox.Show("Erro inesperado: Prioridade com valor incorreto.");
                    }
                }
                reader.Close();
            }
        }

    }
}
