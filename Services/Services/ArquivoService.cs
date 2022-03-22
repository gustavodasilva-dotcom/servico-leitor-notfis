using System;
using System.IO;
using System.Linq;
using Domain.Entities;
using Services.Interfaces;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

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

        private readonly IConfiguration _configuration;

        private readonly ILogger<ArquivoService> _logger;

        public ArquivoService(ILogger<ArquivoService> logger, IConfiguration configuration)
        {
            _logger = logger;

            Dia = DateTime.Now.Day;
            Mes = DateTime.Now.Month;
            Ano = DateTime.Now.Year;

            _configuration = configuration;

            Extensao = _configuration["Leitor:Configuracoes:Extensao"];
        }

        public void VerificarArquivos(PastasClientes pastaCliente, List<Clientes_Tags> tags, List<InformacoesLinhas> informacoesLinhas)
        {
            try
            {
                SePastasNaoExistemCrie(pastaCliente);

                var arquivos = ObterArquivos();

                if (arquivos.Length > 0)
                {
                    _logger.LogInformation($"{DateTimeOffset.Now} - {arquivos.Length} arquivo(s) para processamento.");

                    foreach (var arquivo in arquivos)
                    {
                        var conteudo = LerArquivo(arquivo, tags);

                        if (conteudo.Length > 0)
                        {
                            _logger.LogInformation($"{DateTimeOffset.Now} - Processando arquivo {arquivo}.");

                            ProcessarConteudoArquivo(informacoesLinhas, conteudo, arquivo, tags);
                        }
                        else
                            _logger.LogInformation($"{DateTimeOffset.Now} - O arquivo {arquivo} está vazio.");
                    }
                }
                else
                    _logger.LogInformation($"{DateTimeOffset.Now} - Não há arquivo(s) para processamento.");
            }
            catch (Exception) { throw; }
        }

        #region Processos

        private void SePastasNaoExistemCrie(PastasClientes pastasClientes)
        {
            if (!Directory.Exists(pastasClientes.CaminhoNovosArquivos))
                Directory.CreateDirectory(pastasClientes.CaminhoNovosArquivos);

            if (!Directory.Exists(pastasClientes.CaminhoProcessados))
                Directory.CreateDirectory(pastasClientes.CaminhoProcessados);

            if (!Directory.Exists(pastasClientes.CaminhoErro))
                Directory.CreateDirectory(pastasClientes.CaminhoErro);

            SetDiretoriosDia(pastasClientes);
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

        private string[] ObterArquivos()
        {
            return Directory.GetFiles(CaminhoNovo, Extensao);
        }

        private string[] LerArquivo(string arquivo, List<Clientes_Tags> tags)
        {
            try
            {
                return File.ReadAllLines(arquivo); 
            }
            catch (Exception e)
            {
                _logger.LogError($"{DateTimeOffset.Now} - O seguinte erro ocorreu: {e.Message}");

                MoverArquivo(arquivo, CaminhoErro);

                throw;
            }
        }

        private void MoverArquivo(string origem, string destino)
        {
            var arquivo = origem.Split("\\");

            SePastasNaoExistemCrie(destino);

            File.Move(origem, destino + arquivo[4]);
        }

        public void ProcessarConteudoArquivo(List<InformacoesLinhas> informacoesLinhas, string[] conteudo, string arquivo, List<Clientes_Tags> tags)
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
                    if (informacoesLinhas.Where(i => i.Descricao.Equals(informacoesLinhas[0].Descricao)).ToList()[0].Descricao == informacoesLinhas[0].Descricao)
                    {
                        if (dado.Substring(0, 3).Contains(informacoesLinhas[0].Linha.ToString()))
                        {
                            if (tags[0].Obrigatoria) remetente.Identificacao = dado.Substring(tags[0].IndiceInicial, tags[0].Tamanho).Trim();
                            if (tags[1].Obrigatoria) remetente.InscricaoEstadual = dado.Substring(tags[1].IndiceInicial, tags[1].Tamanho).Trim();
                            if (tags[2].Obrigatoria) remetente.RazaoSocial = dado.Substring(tags[2].IndiceInicial, tags[2].Tamanho).Trim();

                            if (tags[6].Obrigatoria) enderecoRemetente.Cep = dado.Substring(tags[6].IndiceInicial, tags[6].Tamanho).Trim();
                            if (tags[7].Obrigatoria) enderecoRemetente.Logradouro = dado.Substring(tags[7].IndiceInicial, tags[7].Tamanho).Trim();
                            if (tags[8].Obrigatoria) enderecoRemetente.Numero = dado.Substring(tags[8].IndiceInicial, tags[8].Tamanho).Trim();
                            if (tags[9].Obrigatoria) enderecoRemetente.Complemento = dado.Substring(tags[9].IndiceInicial, tags[9].Tamanho).Trim();
                            if (tags[10].Obrigatoria) enderecoRemetente.Bairro = dado.Substring(tags[10].IndiceInicial, tags[10].Tamanho).Trim();
                            if (tags[11].Obrigatoria) enderecoRemetente.Cidade = dado.Substring(tags[11].IndiceInicial, tags[11].Tamanho).Trim();
                            if (tags[12].Obrigatoria) enderecoRemetente.Estado = dado.Substring(tags[12].IndiceInicial, tags[12].Tamanho).Trim();

                            if (conteudoArquivo.Remetente == null) conteudoArquivo.Remetente = new Remetente();
                            conteudoArquivo.Remetente = remetente;

                            if (conteudoArquivo.Remetente.Endereco == null) conteudoArquivo.Remetente.Endereco = new Endereco();
                            conteudoArquivo.Remetente.Endereco = enderecoRemetente;
                        }
                    }
                    
                    if (informacoesLinhas.Where(i => i.Descricao.Equals(informacoesLinhas[1].Descricao)).FirstOrDefault().Descricao == informacoesLinhas[1].Descricao)
                    {
                        if (dado.Substring(0, 3).Contains(informacoesLinhas[1].Linha.ToString()))
                        {
                            if (tags[3].Obrigatoria) destinatario.Nome = dado.Substring(tags[3].IndiceInicial, tags[3].Tamanho).Trim();
                            if (tags[4].Obrigatoria) destinatario.Identificacao = dado.Substring(tags[4].IndiceInicial, tags[4].Tamanho).Trim();
                            if (tags[5].Obrigatoria) destinatario.InscricaoEstadual = dado.Substring(tags[5].IndiceInicial, tags[5].Tamanho).Trim();

                            if (tags[13].Obrigatoria) enderecoDestinatario.Cep = dado.Substring(tags[13].IndiceInicial, tags[13].Tamanho).Trim();
                            if (tags[14].Obrigatoria) enderecoDestinatario.Logradouro = dado.Substring(tags[14].IndiceInicial, tags[14].Tamanho).Trim();
                            if (tags[15].Obrigatoria) enderecoDestinatario.Numero = dado.Substring(tags[15].IndiceInicial, tags[15].Tamanho).Trim();
                            if (tags[16].Obrigatoria) enderecoDestinatario.Complemento = dado.Substring(tags[16].IndiceInicial, tags[16].Tamanho).Trim();
                            if (tags[17].Obrigatoria) enderecoDestinatario.Bairro = dado.Substring(tags[17].IndiceInicial, tags[17].Tamanho).Trim();
                            if (tags[18].Obrigatoria) enderecoDestinatario.Cidade = dado.Substring(tags[18].IndiceInicial, tags[18].Tamanho).Trim();
                            if (tags[19].Obrigatoria) enderecoDestinatario.Estado = dado.Substring(tags[19].IndiceInicial, tags[19].Tamanho).Trim();

                            if (conteudoArquivo.Destinatario == null)
                                conteudoArquivo.Destinatario = new Destinatario();

                            conteudoArquivo.Destinatario = destinatario;

                            if (conteudoArquivo.Destinatario.Endereco == null)
                                conteudoArquivo.Destinatario.Endereco = new Endereco();

                            conteudoArquivo.Destinatario.Endereco = enderecoDestinatario;
                        }
                    }

                    if (informacoesLinhas.Where(i => i.Descricao.Equals(informacoesLinhas[2].Descricao)).FirstOrDefault().Descricao == informacoesLinhas[2].Descricao)
                    {
                        if (dado.Substring(0, 3).Contains(informacoesLinhas[2].Linha.ToString()))
                            if (tags[20].Obrigatoria) ordem.NumeroOrdem = dado.Substring(tags[20].IndiceInicial, tags[20].Tamanho).Trim();
                    }

                    if (informacoesLinhas.Where(i => i.Descricao.Equals(informacoesLinhas[3].Descricao)).FirstOrDefault().Descricao == informacoesLinhas[3].Descricao)
                    {
                        if (dado.Substring(0, 3).Contains(informacoesLinhas[3].Linha.ToString()))
                        {
                            if (tags[21].Obrigatoria) ordem.Preco = dado.Substring(tags[21].IndiceInicial, tags[21].Tamanho).Trim();
                            if (tags[22].Obrigatoria) ordem.ChaveNFe = dado.Substring(tags[22].IndiceInicial, tags[22].Tamanho).Trim();

                            if (conteudoArquivo.Ordem == null) conteudoArquivo.Ordem = new Ordem();
                            conteudoArquivo.Ordem = ordem;
                        }
                    }

                    if (informacoesLinhas.Where(i => i.Descricao.Equals(informacoesLinhas[4].Descricao)).FirstOrDefault().Descricao == informacoesLinhas[4].Descricao)
                    {
                        if (dado.Substring(0, 3).Contains(informacoesLinhas[4].Linha.ToString()))
                        {
                            var item = new Item();
                            if (tags[23].Obrigatoria) item.Quantidade = dado.Substring(tags[23].IndiceInicial, tags[23].Tamanho).Trim();
                            if (tags[24].Obrigatoria) item.Descricao = dado.Substring(tags[24].IndiceInicial, tags[24].Tamanho).Trim();

                            if (conteudoArquivo.Ordem.Items == null) conteudoArquivo.Ordem.Items = new List<Item>();
                            conteudoArquivo.Ordem.Items.Add(item);
                        }
                    }

                    if (informacoesLinhas.Where(i => i.Descricao.Equals(informacoesLinhas[5].Descricao)).FirstOrDefault().Descricao == informacoesLinhas[5].Descricao)
                    {
                        if (dado.Substring(0, 3).Contains(informacoesLinhas[5].Linha.ToString()))
                            conteudosArquivos.Add(conteudoArquivo);
                    }
                }

                MoverArquivo(arquivo, CaminhoProcessado);
            }
            catch (Exception e)
            {
                _logger.LogError($"{DateTimeOffset.Now} - O seguinte erro ocorreu: {e.Message}");

                MoverArquivo(arquivo, CaminhoErro);
            }
        }

        #endregion
    }
}
