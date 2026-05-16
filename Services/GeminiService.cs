using System.Text;
using System.Text.Json;
using HackathonValidador.Models;

namespace HackathonValidador.Services;

public class GeminiService
{
    private readonly string _apiKey;

    public GeminiService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<DocumentoExtraido> ExtrairDadosAsync(string tipoDocumento, string caminhoArquivo)
    {
        string promptMestre = MontarPromptDinamico(tipoDocumento);

        byte[] bytesImagem = await File.ReadAllBytesAsync(caminhoArquivo);
        string base64Image = Convert.ToBase64String(bytesImagem);

        var payload = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new { text = promptMestre },
                        new { inline_data = new { mime_type = "image/png", data = base64Image } }
                    }
                }
            }
        };

        string jsonPayload = JsonSerializer.Serialize(payload);

        using var client = new HttpClient();
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // Dispara a requisição
        var response = await client.PostAsync(url, content);
        string responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"\n[ERRO NA API DO GOOGLE] Falha na comunicação: {responseString}");
            return new DocumentoNaoReconhecido(); // Retorna objeto de erro para o sistema não "explodir"
        }

        // Navega no JSON de resposta do Google para extrair o texto da IA
        string jsonExtraido = ExtrairTextoDaResposta(responseString);
        string jsonPuro = LimparRespostaJson(jsonExtraido);

        // Converte o JSON para o objeto C#
        var opcoesSerializer = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        DocumentoExtraido? documento;
        switch (tipoDocumento.ToUpper())
        {
            case "RG":
                documento = JsonSerializer.Deserialize<RgExtraido>(jsonPuro, opcoesSerializer);
                break;
            case "CNH":
                documento = JsonSerializer.Deserialize<CnhExtraido>(jsonPuro, opcoesSerializer);
                break;
            case "PASSAPORTE":
                documento = JsonSerializer.Deserialize<PassaporteExtraido>(jsonPuro, opcoesSerializer);
                break;
            default:
                documento = null; // Tipo não suportado, não tentamos deserializar
                break;
        }
        return documento ?? new DocumentoNaoReconhecido();
    }

    private string ExtrairTextoDaResposta(string jsonRespostaGoogle)
    {
        try
        {
            using JsonDocument doc = JsonDocument.Parse(jsonRespostaGoogle);
            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "";
        }
        catch
        {
            return "";
        }
    }

    private string MontarPromptDinamico(string tipoDocumento)
    {
        // 1. CAMPOS BASE (Obrigatórios para qualquer documento)
        string camposBase = "nome_completo, numero_documento, data_nascimento";

        // 2. CAMPOS COMPLEMENTARES (Apenas o que é exclusivo de cada tipo)
        string camposComplementares = tipoDocumento.ToUpper() switch
        {
            "RG" => "data_emissao",
            "CNH" => "categoria",
            "PASSAPORTE" => "data_validade, pais_emissor",
            _ => "" // Se for um tipo desconhecido, não pede nada extra
        };

        // 3. JUNTA TUDO NUMA INSTRUÇÃO CLARA E DIRETA PARA A IA
        string todosOsCampos = string.IsNullOrWhiteSpace(camposComplementares)
            ? camposBase
            : $"{camposBase}, {camposComplementares}";

        return $@"
            Você é um assistente especialista em análise de documentos.
            Analise a imagem enviada e verifique se ela corresponde a um(a) {tipoDocumento}.
            
            REGRAS ESTRITAS:
            1. Retorne 'is_legivel' como true se o texto estiver nítido, ou false se estiver borrado.
            2. Identifique o 'tipo_documento' real encontrado na imagem.
            3. Extraia os seguintes campos: {todosOsCampos}.
            4. Se um campo não existir ou for impossível ler, retorne null para ele.
            5. Responda APENAS com um JSON estrito, sem formatação markdown (como ```json) ou textos explicativos.";
    }

    private string LimparRespostaJson(string respostaIa)
    {
        return respostaIa
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();
    }
}