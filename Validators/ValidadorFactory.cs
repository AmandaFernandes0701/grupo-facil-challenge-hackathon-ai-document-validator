namespace HackathonValidador.Validators;

/// <summary>
/// Implementação do padrão Factory (Fábrica).
/// Responsável por instanciar o validador correto baseado no tipo de documento.
/// </summary>
public static class ValidadorFactory
{
    /// <summary>
    /// Retorna uma instância de DocumentoValidator compatível com o tipo informado.
    /// </summary>
    /// <param name="tipo">O tipo de documento selecionado pelo usuário (ex: "RG", "CNH").</param>
    /// <returns>Uma subclasse de DocumentoValidator ou null caso o tipo não seja suportado.</returns>
    public static DocumentoValidator? ObterValidador(string tipo)
    {
        if (string.IsNullOrWhiteSpace(tipo)) return null;

        // O switch expression decide qual classe instanciar.
        return tipo.ToUpper() switch
        {
            "RG" => new RgValidator(),
            "CNH" => new CnhValidator(),
            "PASSAPORTE" => new PassaporteValidator(),
            _ => null // Retorna nulo se o tipo não for conhecido pela fábrica
        };
    }
}