using System;
using System.IO;
using Domain.Entities;
using Services.Interfaces;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Services.Services
{
    public class ArquivoService : IArquivoService
    {
        private readonly ILogger<ArquivoService> _logger;

        private readonly IConfiguration _configuration;

        public ArquivoService(ILogger<ArquivoService> logger, IConfiguration configuration)
        {
            _logger = logger;

            _configuration = configuration;
        }

        public void VerificarArquivos()
        {
            try
            {
                var arquivos = ObterArquivos();

                if (arquivos.Length > 0)
                {
                    _logger.LogInformation($"{DateTimeOffset.Now} - {arquivos.Length} arquivo(s) para processamento.");

                    foreach (var arquivo in arquivos)
                    {
                        var conteudo = LerArquivo(arquivo);

                        if (conteudo.Length > 0)
                            ProcessarConteudoArquivo(conteudo);
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

        private string[] ObterArquivos()
        {
            return Directory.GetFiles(_configuration["Arquivo:Notfis:Caminho"], _configuration["Arquivo:Notfis:Extensao"]); ;
        }

        private static string[] LerArquivo(string caminho)
        {
            return File.ReadAllLines(caminho);
        }

        private static void ProcessarConteudoArquivo(string[] conteudo)
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
        }

        #endregion
    }
}
