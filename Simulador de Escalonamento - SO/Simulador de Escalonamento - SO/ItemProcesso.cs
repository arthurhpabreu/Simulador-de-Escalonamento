using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulador_de_Escalonamento___SO
{
    class ItemProcesso
    {
        public Processo _Processo { get; set; }
        public ItemProcesso Proximo { get; set; }

        public ItemProcesso(Processo p)
        {
            _Processo = p;
        }

        public ItemProcesso()
        {

        }
    }
}
