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

        public void VerificarArquivos(PastasClientes pastaCliente, Extensoes extensao, List<Clientes_Layouts_Tag> tags)
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
                                ProcessarConteudoArquivoNOTFIS(conteudo, arquivo, tags);

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
            CaminhoProcessado = $@"{pastasClientes.CaminhoProcessados}{Ano}\{Mes}\{Dia}\";
            CaminhoErro = $@"{pastasClientes.CaminhoErro}{Ano}\{Mes}\{Dia}\";
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
            var arquivo = origem.Split("\\");

            SePastasNaoExistemCrie(destino);

            File.Move(origem, destino + arquivo[4]);
        }

        private void ProcessarConteudoArquivoNOTFIS(string[] conteudo, string arquivo, List<Clientes_Layouts_Tag> tags)
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
                        if (tags[0].Obrigatoria) remetente.Identificacao = dado.Substring(tags[0].InicioIndice, tags[0].Tamanho).Trim();
                        if (tags[1].Obrigatoria) remetente.InscricaoEstadual = dado.Substring(tags[1].InicioIndice, tags[1].Tamanho).Trim();
                        if (tags[2].Obrigatoria) remetente.RazaoSocial = dado.Substring(tags[2].InicioIndice, tags[2].Tamanho).Trim();

                        if (tags[6].Obrigatoria) enderecoRemetente.Cep = dado.Substring(tags[6].InicioIndice, tags[6].Tamanho).Trim();
                        if (tags[7].Obrigatoria) enderecoRemetente.Logradouro = dado.Substring(tags[7].InicioIndice, tags[7].Tamanho).Trim();
                        if (tags[8].Obrigatoria) enderecoRemetente.Numero = dado.Substring(tags[8].InicioIndice, tags[8].Tamanho).Trim();
                        if (tags[9].Obrigatoria) enderecoRemetente.Complemento = dado.Substring(tags[9].InicioIndice, tags[9].Tamanho).Trim();
                        if (tags[10].Obrigatoria) enderecoRemetente.Bairro = dado.Substring(tags[10].InicioIndice, tags[10].Tamanho).Trim();
                        if (tags[11].Obrigatoria) enderecoRemetente.Cidade = dado.Substring(tags[11].InicioIndice, tags[11].Tamanho).Trim();
                        if (tags[12].Obrigatoria) enderecoRemetente.Estado = dado.Substring(tags[12].InicioIndice, tags[12].Tamanho).Trim();

                        if (conteudoArquivo.Remetente == null) conteudoArquivo.Remetente = new Remetente();
                        conteudoArquivo.Remetente = remetente;

                        if (conteudoArquivo.Remetente.Endereco == null) conteudoArquivo.Remetente.Endereco = new Endereco();
                        conteudoArquivo.Remetente.Endereco = enderecoRemetente;
                    }

                    if (dado.Substring(0, 3).Contains("312"))
                    {
                        if (tags[3].Obrigatoria) destinatario.Nome = dado.Substring(tags[3].InicioIndice, tags[3].Tamanho).Trim();
                        if (tags[4].Obrigatoria) destinatario.Identificacao = dado.Substring(tags[4].InicioIndice, tags[4].Tamanho).Trim();
                        if (tags[5].Obrigatoria) destinatario.InscricaoEstadual = dado.Substring(tags[5].InicioIndice, tags[5].Tamanho).Trim();

                        if (tags[13].Obrigatoria) enderecoDestinatario.Cep = dado.Substring(tags[13].InicioIndice, tags[13].Tamanho).Trim();
                        if (tags[14].Obrigatoria) enderecoDestinatario.Logradouro = dado.Substring(tags[14].InicioIndice, tags[14].Tamanho).Trim();
                        if (tags[15].Obrigatoria) enderecoDestinatario.Numero = dado.Substring(tags[15].InicioIndice, tags[15].Tamanho).Trim();
                        if (tags[16].Obrigatoria) enderecoDestinatario.Complemento = dado.Substring(tags[16].InicioIndice, tags[16].Tamanho).Trim();
                        if (tags[17].Obrigatoria) enderecoDestinatario.Bairro = dado.Substring(tags[17].InicioIndice, tags[17].Tamanho).Trim();
                        if (tags[18].Obrigatoria) enderecoDestinatario.Cidade = dado.Substring(tags[18].InicioIndice, tags[18].Tamanho).Trim();
                        if (tags[19].Obrigatoria) enderecoDestinatario.Estado = dado.Substring(tags[19].InicioIndice, tags[19].Tamanho).Trim();

                        if (conteudoArquivo.Destinatario == null)
                            conteudoArquivo.Destinatario = new Destinatario();

                        conteudoArquivo.Destinatario = destinatario;

                        if (conteudoArquivo.Destinatario.Endereco == null)
                            conteudoArquivo.Destinatario.Endereco = new Endereco();

                        conteudoArquivo.Destinatario.Endereco = enderecoDestinatario;
                    }

                    if (dado.Substring(0, 3).Contains("307"))
                        if (tags[20].Obrigatoria) ordem.NumeroOrdem = dado.Substring(tags[20].InicioIndice, tags[20].Tamanho).Trim();

                    if (dado.Substring(0, 3).Contains("313"))
                    {
                        if (tags[21].Obrigatoria) ordem.Preco = dado.Substring(tags[21].InicioIndice, tags[21].Tamanho).Trim();
                        if (tags[22].Obrigatoria) ordem.ChaveNFe = dado.Substring(tags[22].InicioIndice, tags[22].Tamanho).Trim();

                        if (conteudoArquivo.Ordem == null) conteudoArquivo.Ordem = new Ordem();
                        conteudoArquivo.Ordem = ordem;
                    }

                    if (dado.Substring(0, 3).Contains("314"))
                    {
                        var item = new Item();
                        if (tags[23].Obrigatoria) item.Quantidade = dado.Substring(tags[23].InicioIndice, tags[23].Tamanho).Trim();
                        if (tags[24].Obrigatoria) item.Descricao = dado.Substring(tags[24].InicioIndice, tags[24].Tamanho).Trim();

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
