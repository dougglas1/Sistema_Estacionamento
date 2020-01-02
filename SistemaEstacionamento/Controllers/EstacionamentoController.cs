using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBM.Data.DB2.Core;
using Microsoft.AspNetCore.Mvc;
using SistemaEstacionamento.Dao;
using SistemaEstacionamento.Dao.Connections;
using SistemaEstacionamento.Models;

namespace SistemaEstacionamento.Controllers
{
    public class EstacionamentoController : Controller
    {
        DB2Connection conexao;
        DB2Transaction transacao;

        /// <summary>
        /// Construtor - utilizado após instânciar a classe
        /// </summary>
        /// <param name="conexao"></param>
        public EstacionamentoController()
        {
            this.conexao = Conexao.Conn();
            if (!conexao.IsOpen)
                conexao.Open();
        }

        /// <summary>
        /// Chamar a Tela Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Buscar Veículo no Estacionamento
        /// </summary>
        /// <returns></returns>
        public List<EstacionamentoModel> BuscarVeiculosEstacionados()
        {
            try
            {
                if (!conexao.IsOpen)
                    conexao.Open();

                List<EstacionamentoModel> veiculos = new EstacionamentoDao(conexao).BuscarVeiculosEstacionados();
                return veiculos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conexao.IsOpen)
                    conexao.Close();
            }
        }

        /// <summary>
        /// Inserir Veículo no Estacionamento
        /// </summary>
        public void EntrarVeiculo([FromBody] EstacionamentoModel obj)
        {
            if (!conexao.IsOpen)
                conexao.Open();

            DB2Transaction trans = conexao.BeginTransaction();
            try
            {
                new EstacionamentoDao(conexao, trans).EntrarVeiculo(obj);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conexao.IsOpen)
                    conexao.Close();
            }
        }

        /// <summary>
        /// Retirar Veículo do Estacionamento
        /// </summary>
        public double SairVeiculo(int codigo)
        {
            try
            {
                if (!conexao.IsOpen)
                    conexao.Open();
                if (transacao == null)
                    transacao = conexao.BeginTransaction();

                DateTime dataEntrada = new EstacionamentoDao(conexao, transacao).BuscarDataEntradaVeiculo(codigo);
                DateTime dataSaida = DateTime.Now;

                TimeSpan duracao = new TimeSpan(dataSaida.Ticks - dataEntrada.Ticks);

                new EstacionamentoDao(conexao, transacao).SairVeiculo(codigo);

                transacao.Commit();

                return 0;
            }
            catch (Exception ex)
            {
                transacao.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conexao.IsOpen)
                    conexao.Close();
            }
        }
    }
}