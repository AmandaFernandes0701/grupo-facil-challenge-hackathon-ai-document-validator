using System.Text.Json.Serialization;

namespace HackathonValidador.Models;

// 1. A CLASSE BASE (Só tem o que TODO documento tem)
public abstract class DocumentoExtraido
{
    [JsonPropertyName("is_legivel")]
    public bool IsLegivel { get; set; }

    [JsonPropertyName("tipo_documento")]
    public string? TipoIdentificado { get; set; }

    [JsonPropertyName("nome_completo")]
    public string? NomeCompleto { get; set; }

    [JsonPropertyName("numero_documento")]
    public string? NumeroDocumento { get; set; }

    [JsonPropertyName("data_nascimento")]
    public string? DataNascimento { get; set; }

    public abstract Dictionary<string, string?> ObterCamposEspecificos();
}

// 2. EXTENSÃO DO RG
public class RgExtraido : DocumentoExtraido
{
    [JsonPropertyName("data_emissao")]
    public string? DataEmissao { get; set; }

    public override Dictionary<string, string?> ObterCamposEspecificos() => new()
    {
        { "Data de Emissão", DataEmissao }
    };
}

// 3. EXTENSÃO DA CNH
public class CnhExtraido : DocumentoExtraido
{
    [JsonPropertyName("categoria")]
    public string? Categoria { get; set; }

    public override Dictionary<string, string?> ObterCamposEspecificos() => new()
    {
        { "Categoria (Ex: AB)", Categoria }
    };
}

// 4. EXTENSÃO DO PASSAPORTE
public class PassaporteExtraido : DocumentoExtraido
{
    [JsonPropertyName("data_validade")]
    public string? DataValidade { get; set; }

    [JsonPropertyName("pais_emissor")]
    public string? PaisEmissor { get; set; }

    public override Dictionary<string, string?> ObterCamposEspecificos() => new()
    {
        { "Data de Validade", DataValidade },
        { "País Emissor", PaisEmissor }
    };
}

// 5. CLASSE PARA ERROS/FALHAS na extração ou deserialização
public class DocumentoNaoReconhecido : DocumentoExtraido
{
    public DocumentoNaoReconhecido()
    {
        IsLegivel = false; // Já define como ilegível por padrão
        TipoIdentificado = "Não Reconhecido";
    }

    public override Dictionary<string, string?> ObterCamposEspecificos() => new();
}