using System.Globalization;
using HackathonValidador.Models;

namespace HackathonValidador.Validators;

public abstract class DocumentoValidator
{
    // Cada subclasse dirá qual tipo ela valida (RG, CNH, etc)
    public abstract string TipoDoc { get; }

    // Método principal que todas as subclasses usarão
    public virtual string Validar(DocumentoExtraido doc)
    {
        // 1. REGRA: Legibilidade
        if (!doc.IsLegivel) return "REJEITADO: Documento ilegível.";

        // Validação para evitar NullReferenceException se a IA não retornar o tipo.
        if (string.IsNullOrWhiteSpace(doc.TipoIdentificado))
        {
            return "REJEITADO: Não foi possível identificar o tipo do documento na imagem.";
        }

        // 2. REGRA: MATCH DE TIPO
        // Verifica se a IA encontrou o mesmo tipo que a classe espera.
        // Usamos 'Contains' porque a IA pode escrever "Carteira de Habilitação" em vez de apenas "CNH".
        bool tipoBate = doc.TipoIdentificado.Contains(TipoDoc, StringComparison.OrdinalIgnoreCase) ||
                        TipoDoc.Contains(doc.TipoIdentificado, StringComparison.OrdinalIgnoreCase);

        if (!tipoBate)
        {
            return $"REJEITADO: Divergência de tipo. O sistema esperava '{TipoDoc}', mas a imagem enviada é um(a) '{doc.TipoIdentificado}'.";
        }

        // 3. REGRA: Campos Obrigatórios Universais
        if (string.IsNullOrWhiteSpace(doc.NomeCompleto)) return "REJEITADO: Nome não encontrado.";
        if (string.IsNullOrWhiteSpace(doc.NumeroDocumento)) return "REJEITADO: Número não encontrado.";

        // 4. REGRA: Sanidade
        if (!ValidarDataNaoFutura(doc.DataNascimento))
            return "REJEITADO: Data de nascimento inválida (no futuro).";

        return "OK"; // Se passar por tudo isso, vai para a validação específica do filho
    }

    // Métodos auxiliares que as subclasses podem usar
    protected bool ValidarDataNaoFutura(string? dataStr)
    {
        // Se a data não foi informada, não podemos validar, então consideramos OK (não é uma data futura).
        if (string.IsNullOrWhiteSpace(dataStr)) return true;

        if (DateTime.TryParseExact(dataStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime data))
            return data <= DateTime.Now;

        return false;
    }

    protected int CalcularIdade(string? dataNascStr)
    {
        if (DateTime.TryParseExact(dataNascStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataNascimento))
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - dataNascimento.Year;
            if (dataNascimento.Date > hoje.AddYears(-idade)) idade--;
            return idade;
        }
        return 0;
    }
}