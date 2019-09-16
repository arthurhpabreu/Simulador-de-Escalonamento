using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Simulador_de_Escalonamento___SO
{
    class Processo
    {
        public int PID { get; set; }
        public string NomeDoProcesso { get; set; }
        public int Prioridade { get; set; }
        public double OcupacaoCPU { get; set; }
        public Stopwatch TempoEmExecucao { get; set; }
        public int NumeroDeCiclos { get; set; }
        public Stopwatch TempoEmEspera { get; set; }
        public int TempoSobra { get; set; }
        public string Estado { get; set; }

        //construtor real
        public Processo(int PID, string NomeDoProcesso, int Prioridade, double OcupacaoCPU, int NumeroDeCiclos)
        {
            this.PID = PID;
            this.NomeDoProcesso = NomeDoProcesso;
            this.Prioridade = Prioridade;
            this.OcupacaoCPU = OcupacaoCPU;
            TempoEmExecucao = new Stopwatch();
            this.NumeroDeCiclos = NumeroDeCiclos;
            TempoEmEspera = new Stopwatch();
            Estado = "PRONTO";
        }

        //Quando um ciclo é completado, o numero de ciclos é reduzido em 1
        //O tempo de execução é resetado e o tempo e
        public void CicloCompleto()
        {
            Estado = "ESPERA";
            TempoEmExecucao.Reset();
            TempoEmExecucao.Stop();

            if (TempoSobra <= 0)
            {
                TempoSobra = 0;
                if (NumeroDeCiclos > 0)
                    NumeroDeCiclos--;
                TempoEmEspera.Start();
            }

            Estado = "PRONTO";
        }

        public void IniciaCiclo()
        {
            Estado = "EXECUTANDO";
            TempoEmEspera.Reset();
            TempoEmEspera.Stop();
            TempoEmExecucao.Start();
        }

        public int ElevarPrioridade()//eleva a prioridade, com a condição de que ela seja menor do que 5
        {
            if (Prioridade < 5)
            {
                Prioridade++;
                return Prioridade;
            }
            return Prioridade;
        }

        public int ReduzirPrioridade()//reduz a prioridade, com a condição de que ela seja maior que 1
        {
            if (Prioridade > 1)
            {
                Prioridade--;
                return Prioridade;
            }
            return Prioridade;
        }

    }
}
