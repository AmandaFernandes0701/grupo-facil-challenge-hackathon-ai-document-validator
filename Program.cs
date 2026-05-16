using HackathonValidador.Models;
using HackathonValidador.Services;
using HackathonValidador.Validators;
using HackathonValidador.UI;
using DotNetEnv;

// 1. Configuração e Segurança
Env.Load();
string? apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

if (string.IsNullOrWhiteSpace(apiKey))
{
    TerminalUI.ExibirErroChaveApi();
    return;
}

// 2. Interação com o Usuário (Input)
TerminalUI.ExibirBanner();
string tipoDoc = TerminalUI.SolicitarTipoDocumento();
string? caminhoArquivo = TerminalUI.SolicitarArquivo("DocsTeste");

if (caminhoArquivo == null)
{
    return; // Encerra se a pasta estiver vazia ou não houver arquivo
}

// 3. Orquestração do Pipeline (Processamento Central)
try
{
    var geminiService = new GeminiService(apiKey);

    // Passamos a função da IA para dentro da UI apenas para exibir a animação de carregamento
    DocumentoExtraido dados = await TerminalUI.ExecutarComSpinner(
        "[yellow]Enviando imagem para a IA e processando dados...[/]",
        async () => await geminiService.ExtrairDadosAsync(tipoDoc, caminhoArquivo)
    );

    // Exibe os dados crus lidos pela IA
    TerminalUI.ExibirTabelaDadosIA(dados);

    // Instancia o validador correto usando Factory
    var validador = ValidadorFactory.ObterValidador(tipoDoc);

    if (validador == null)
    {
        Console.WriteLine($"\n[ERRO] Não há validador implementado para o tipo: {tipoDoc}");
        return;
    }

    // Aplica as Regras de Negócio e obtém a decisão
    string resultadoFinal = validador.Validar(dados);

    // 4. Exibição do Resultado (Output)
    TerminalUI.ExibirVeredito(tipoDoc, resultadoFinal);
}
catch (Exception ex)
{
    // Captura qualquer erro de internet, API ou código e exibe o erro
    TerminalUI.ExibirErroCritico(ex);
}