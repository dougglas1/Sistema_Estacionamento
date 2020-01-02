using IBM.Data.DB2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SistemaEstacionamento.Models;

namespace SistemaEstacionamento.Dao
{
    public class EstacionamentoDao
    {
        DB2Connection conexao;
        DB2Transaction transacao;

        public EstacionamentoDao(DB2Connection conexao, DB2Transaction transacao = null)
        {
            this.conexao = conexao;
            this.transacao = transacao;
        }

        public List<EstacionamentoModel> BuscarVeiculosEstacionados()
        {
            string sql = @"SELECT NUMERO_REGISTRO, MODELO_VEICULO, PLACA_VEICULO, DATA_ENTRADA, 
                           DATA_SAIDA, OBSERVACAO_VEICULO FROM KARSTEN.TST001_CONTROLE_ENTRADA_VEICULO";

            return conexao.Query<EstacionamentoModel>(sql).ToList();
        }

        public void EntrarVeiculo(EstacionamentoModel controle)
        {
            string sql = @"INSERT INTO KARSTEN.TST001_CONTROLE_ENTRADA_VEICULO 
                           (MODELO_VEICULO, PLACA_VEICULO, DATA_ENTRADA, OBSERVACAO_VEICULO)
                          VALUES (@Modelo_veiculo, @Placa_veiculo, CURRENT TIMESTAMP, @Observacao_veiculo)";

            conexao.ExecuteScalar(sql, param: controle, transaction: transacao);
        }

        public void SairVeiculo(int codigo)
        {
            string sql = @"UPDATE KARSTEN.TST001_CONTROLE_ENTRADA_VEICULO SET
                           DATA_SAIDA = CURRENT TIMESTAMP
                           WHERE NUMERO_REGISTRO = @Codigo";

            conexao.ExecuteScalar(sql, param: new { Codigo = codigo }, transaction: transacao);
        }

        public DateTime BuscarDataEntradaVeiculo(int codigo)
        {
            string sql = @"SELECT DATA_ENTRADA, 
                           DATA_SAIDA FROM KARSTEN.TST001_CONTROLE_ENTRADA_VEICULO
                           WHERE NUMERO_REGISTRO = @Codigo";

            return conexao.Query<DateTime>(sql, param: new { Codigo = codigo }, transaction: transacao).FirstOrDefault();
        }

    }
}
