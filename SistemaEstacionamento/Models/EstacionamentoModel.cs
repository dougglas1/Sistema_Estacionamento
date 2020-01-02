using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaEstacionamento.Models
{
    public class EstacionamentoModel
    {
        /// <summary>NUMERO REGISTRO ENTRADA</summary>
        public int Numero_registro { get; set; }

        /// <summary>MODELO VEÍCULO</summary>
        public string Modelo_veiculo { get; set; }

        /// <summary>PLACA VEÍCULO</summary>
        public string Placa_veiculo { get; set; }

        /// <summary>DATA ENTRADA</summary>
        public string Data_entrada { get; set; }

        /// <summary>DATA SAÍDA</summary>
        public string Data_saida { get; set; }

        /// <summary>OBSEVAÇÃO</summary>
        public string Observacao_veiculo { get; set; }
    }
}
