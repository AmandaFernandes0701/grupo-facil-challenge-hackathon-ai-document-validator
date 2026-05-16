using HackathonValidador.Models;
using Spectre.Console;

namespace HackathonValidador.UI;

/// <summary>
/// Classe responsável EXCLUSIVAMENTE por interagir com o terminal.
/// Mantém a lógica de apresentação isolada da lógica de negócio.
/// </summary>
public static class TerminalUI
{
    public static void ExibirBanner()
    {
        Console.Clear();
        AnsiConsole.Write(new FigletText("Hackathon Validator").LeftJustified().Color(Color.Cyan1));
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold white]criado por:[/] [white]Amanda Fernandes[/]\n");
    }

    public static void ExibirErroChaveApi()
    {
        AnsiConsole.MarkupLine("\n[bold red]ERRO CRÍTICO: Chave da API do Gemini não encontrada![/]");
        AnsiConsole.MarkupLine("[yellow]Crie um arquivo chamado '.env' na raiz do projeto e adicione a linha:[/]");
        AnsiConsole.MarkupLine("[white]GEMINI_API_KEY=sua_chave_aqui[/]\n");
    }

    public static string SolicitarTipoDocumento()
    {
        var tipo = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n[bold yellow]Selecione o tipo de documento que deseja validar:[/]")
                .PageSize(5)
                .HighlightStyle(new Style(foreground: Color.Black, background: Color.Green))
                .AddChoices(new[] { "RG", "CNH", "PASSAPORTE" }));

        AnsiConsole.MarkupLine($"Você selecionou: [bold green]{tipo}[/]\n");
        return tipo;
    }

    public static string? SolicitarArquivo(string pastaImagens)
    {
        if (!Directory.Exists(pastaImagens))
        {
            Directory.CreateDirectory(pastaImagens);
            AnsiConsole.MarkupLine($"[bold red]A pasta '{pastaImagens}' não existia e foi criada agora.[/]");
            AnsiConsole.MarkupLine("[yellow]Por favor, coloque as imagens dos documentos lá dentro e rode o programa novamente![/]");
            return null;
        }

        string[] caminhosCompletos = Directory.GetFiles(pastaImagens);

        if (caminhosCompletos.Length == 0)
        {
            AnsiConsole.MarkupLine($"[bold red]Nenhuma imagem encontrada na pasta '{pastaImagens}'.[/]");
            return null;
        }

        string[] nomesArquivos = caminhosCompletos.Select(Path.GetFileName).ToArray()!;

        string nomeArquivoEscolhido = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n[bold yellow]Selecione a imagem que deseja validar:[/]")
                .PageSize(10)
                .HighlightStyle(new Style(foreground: Color.Black, background: Color.Cyan1))
                .AddChoices(nomesArquivos));

        AnsiConsole.MarkupLine($"Arquivo selecionado: [bold cyan]{nomeArquivoEscolhido}[/]\n");

        return Path.Combine(pastaImagens, nomeArquivoEscolhido);
    }

    // Cria uma animação de "Carregando..." no terminal enquanto executa a tarefa
    public static async Task<T> ExecutarComSpinner<T>(string mensagem, Func<Task<T>> acao)
    {
        return await AnsiConsole.Status()
            .StartAsync(mensagem, async ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);
                ctx.SpinnerStyle(Style.Parse("green"));
                return await acao();
            });
    }

    public static void ExibirTabelaDadosIA(DocumentoExtraido dadosExtraidos)
    {
        var tabelaDados = new Table();
        tabelaDados.Border(TableBorder.Rounded);
        tabelaDados.Title("[bold green]DADOS LIDOS PELA INTELIGÊNCIA ARTIFICIAL[/]");

        tabelaDados.AddColumn(new TableColumn("[green]CAMPO[/]"));
        tabelaDados.AddColumn(new TableColumn("[green]VALOR IDENTIFICADO[/]"));

        tabelaDados.AddRow("Tipo Identificado", dadosExtraidos.TipoIdentificado ?? "[red]Não identificado[/]");
        tabelaDados.AddRow("Nome Completo", dadosExtraidos.NomeCompleto ?? "[red]Não identificado[/]");
        tabelaDados.AddRow("Número", dadosExtraidos.NumeroDocumento ?? "[red]Não identificado[/]");
        tabelaDados.AddRow("Data Nasc.", dadosExtraidos.DataNascimento ?? "[red]Não identificado[/]");

        foreach (var campo in dadosExtraidos.ObterCamposEspecificos())
        {
            tabelaDados.AddRow(campo.Key, campo.Value ?? "[red]Não identificado[/]");
        }

        Console.WriteLine();
        AnsiConsole.Write(tabelaDados);
    }

    public static void ExibirVeredito(string tipoSolicitado, string resultadoFinal)
    {
        string[] partesResultado = resultadoFinal.Split(":", 2);
        string status = partesResultado[0].Trim();
        string detalhe = partesResultado.Length > 1 ? partesResultado[1].Trim() : "Sem detalhes adicionais.";

        var tabela = new Table();
        tabela.Border(TableBorder.Rounded);
        tabela.Title("[bold cyan]RELATÓRIO DE VALIDAÇÃO DE DOCUMENTO[/]");

        tabela.AddColumn(new TableColumn("[cyan]CAMPO[/]"));
        tabela.AddColumn(new TableColumn("[cyan]INFORMAÇÃO[/]"));

        tabela.AddRow("[bold]TIPO SOLICITADO[/]", tipoSolicitado);

        string statusFormatado = status.Contains("APROVADO")
            ? $"[black on green] ✔ {status} [/]"
            : $"[white on red] ✖ {status} [/]";

        tabela.AddRow("[bold]VEREDITO FINAL[/]", statusFormatado);
        tabela.AddRow("[bold]PARECER TÉCNICO[/]", detalhe);

        Console.WriteLine();
        AnsiConsole.Write(tabela);
        Console.WriteLine();
    }

    public static void ExibirErroCritico(Exception ex)
    {
        AnsiConsole.MarkupLine("\n[bold red]OCORREU UM ERRO INESPERADO![/]");
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    }
}