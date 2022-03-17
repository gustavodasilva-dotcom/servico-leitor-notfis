using System;
using System.IO;
using Domain.Entities;
using Services.Interfaces;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Services.Services
{
    public class ArquivoService : IArquivoService
    {
        private int Dia { get; set; }

        private int Mes { get; set; }

        private int Ano { get; set; }

        private string CaminhoNovo { get; set; }

        private string CaminhoProcessado { get; set; }

        private string CaminhoErro { get; set; }

        private string Extensao { get; set; }

        private readonly ILogger<ArquivoService> _logger;

        public ArquivoService(ILogger<ArquivoService> logger)
        {
            _logger = logger;

            Dia = DateTime.Now.Day;
            Mes = DateTime.Now.Month;
            Ano = DateTime.Now.Year;
        }

        public void VerificarArquivos(PastasClientes pastaCliente, Extensoes extensao)
        {
            try
            {
                SePastasNaoExistemCrie(pastaCliente, extensao);

                var arquivos = ObterArquivos();

                if (arquivos.Length > 0)
                {
                    _logger.LogInformation($"{DateTimeOffset.Now} - {arquivos.Length} arquivo(s) para processamento.");

                    foreach (var arquivo in arquivos)
                    {
                        var conteudo = LerArquivo(arquivo);

                        if (conteudo.Length > 0)
                        {
                            if (extensao.Extensao.Equals("*.txt"))
                                ProcessarConteudoArquivoNOTFIS(conteudo, arquivo);

                            if (extensao.Extensao.Equals("*.csv"))
                                ProcessarConteudoArquivoCSV();

                            if (extensao.Extensao.Equals("*.xml"))
                                ProcessarConteudoArquivoXML();
                        }
                    }
                }
                else
                {
                    _logger.LogInformation($"{DateTimeOffset.Now} - Não há arquivo(s) para processamento.");
                }
            }
            catch (Exception) { throw; }
        }

        #region Processos

        private void SePastasNaoExistemCrie(PastasClientes pastasClientes, Extensoes extensao)
        {
            if (!Directory.Exists(pastasClientes.CaminhoNovosArquivos))
                Directory.CreateDirectory(pastasClientes.CaminhoNovosArquivos);

            if (!Directory.Exists(pastasClientes.CaminhoProcessados))
                Directory.CreateDirectory(pastasClientes.CaminhoProcessados);

            if (!Directory.Exists(pastasClientes.CaminhoErro))
                Directory.CreateDirectory(pastasClientes.CaminhoErro);

            SetDiretoriosDia(pastasClientes);

            SetExtensao(extensao);
        }

        private void SePastasNaoExistemCrie(string diretorio)
        {
            if (!Directory.Exists(diretorio))
                Directory.CreateDirectory(diretorio);
        }

        private void SetDiretoriosDia(PastasClientes pastasClientes)
        {
            CaminhoNovo = $@"{pastasClientes.CaminhoNovosArquivos}";
            CaminhoProcessado = $@"{pastasClientes.CaminhoProcessados}\{Ano}\{Mes}\{Dia}\";
            CaminhoErro = $@"{pastasClientes.CaminhoErro}\{Ano}\{Mes}\{Dia}\";
        }

        private void SetExtensao(Extensoes extensao)
        {
            Extensao = extensao.Extensao;
        }

        private string[] ObterArquivos()
        {
            return Directory.GetFiles(CaminhoNovo, Extensao);
        }

        private string[] LerArquivo(string caminho)
        {
            try
            {
                return File.ReadAllLines(caminho);
            }
            catch (Exception e)
            {
                _logger.LogError($"{DateTimeOffset.Now} - O seguinte erro ocorreu: {e.Message}");

                MoverArquivo(caminho, CaminhoErro);

                throw;
            }
        }

        private void MoverArquivo(string origem, string destino)
        {
            SePastasNaoExistemCrie(destino);

            File.Move(origem, destino);
        }

        private void ProcessarConteudoArquivoNOTFIS(string[] conteudo, string arquivo)
        {
            try
            {
                var conteudosArquivos = new List<ConteudoArquivo>();

                var conteudoArquivo = new ConteudoArquivo();

                var remetente = new Remetente();
                var destinatario = new Destinatario();
                var enderecoRemetente = new Endereco();
                var enderecoDestinatario = new Endereco();

                var ordem = new Ordem();

                foreach (var dado in conteudo)
                {
                    if (dado.Substring(0, 3).Contains("311"))
                    {
                        remetente.Cnpj = dado.Substring(3, 14).Trim();
                        remetente.RazaoSocial = dado.Substring(133, 35).Trim();

                        enderecoRemetente.Cep = dado.Substring(107, 8).Trim();
                        enderecoRemetente.Logradouro = dado.Substring(32, 24).Trim();
                        enderecoRemetente.Numero = dado.Substring(173, 3).Trim();
                        enderecoRemetente.Cidade = dado.Substring(72, 9).Trim();
                        enderecoRemetente.Estado = dado.Substring(116, 2).Trim();

                        if (conteudoArquivo.Remetente == null) conteudoArquivo.Remetente = new Remetente();
                        conteudoArquivo.Remetente = remetente;

                        if (conteudoArquivo.Remetente.Endereco == null) conteudoArquivo.Remetente.Endereco = new Endereco();
                        conteudoArquivo.Remetente.Endereco = enderecoRemetente;
                    }

                    if (dado.Substring(0, 3).Contains("312"))
                    {
                        destinatario.Nome = dado.Substring(3, 34).Trim();
                        destinatario.Identificacao = dado.Substring(43, 11).Trim();
                        destinatario.InscricaoEstadual = dado.Substring(57, 6).Trim();

                        enderecoDestinatario.Cep = dado.Substring(167, 8).Trim();
                        enderecoDestinatario.Logradouro = dado.Substring(72, 19).Trim();
                        enderecoDestinatario.Numero = dado.Substring(294, 4).Trim();
                        enderecoDestinatario.Complemento = dado.Substring(240, 12).Trim();
                        enderecoDestinatario.Bairro = dado.Substring(112, 11).Trim();
                        enderecoDestinatario.Cidade = dado.Substring(132, 8).Trim();
                        enderecoDestinatario.Estado = dado.Substring(185, 2).Trim();

                        if (conteudoArquivo.Destinatario == null)
                            conteudoArquivo.Destinatario = new Destinatario();

                        conteudoArquivo.Destinatario = destinatario;

                        if (conteudoArquivo.Destinatario.Endereco == null)
                            conteudoArquivo.Destinatario.Endereco = new Endereco();

                        conteudoArquivo.Destinatario.Endereco = enderecoDestinatario;
                    }

                    if (dado.Substring(0, 3).Contains("307"))
                        ordem.NumeroOrdem = dado.Substring(108, 11).Trim();

                    if (dado.Substring(0, 3).Contains("313"))
                    {
                        ordem.Preco = dado.Substring(78, 34).Trim();
                        ordem.ChaveNFe = dado.Substring(258, 44).Trim();

                        if (conteudoArquivo.Ordem == null) conteudoArquivo.Ordem = new Ordem();
                        conteudoArquivo.Ordem = ordem;
                    }

                    if (dado.Substring(0, 3).Contains("314"))
                    {
                        var item = new Item();
                        item.Quantidade = dado.Substring(3, 7).Trim();
                        item.Descricao = dado.Substring(25, 37).Trim();

                        if (conteudoArquivo.Ordem.Items == null) conteudoArquivo.Ordem.Items = new List<Item>();
                        conteudoArquivo.Ordem.Items.Add(item);
                    }

                    if (dado.Substring(0, 3).Contains("315"))
                        conteudosArquivos.Add(conteudoArquivo);
                }

                MoverArquivo(arquivo, CaminhoProcessado);
            }
            catch (Exception e)
            {
                _logger.LogError($"{DateTimeOffset.Now} - O seguinte erro ocorreu: {e.Message}");

                MoverArquivo(arquivo, CaminhoErro);
            }
        }

        private void ProcessarConteudoArquivoCSV()
        {
            // TODO: a ser implementado.
        }

        private void ProcessarConteudoArquivoXML()
        {
            // TODO: a ser implementado.
        }

        #endregion
    }
}
