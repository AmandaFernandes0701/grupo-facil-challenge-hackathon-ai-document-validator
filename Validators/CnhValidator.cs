using HackathonValidador.Models;

namespace HackathonValidador.Validators;

public class CnhValidator : DocumentoValidator
{
    public override string TipoDoc => "CNH";

    public override string Validar(DocumentoExtraido doc)
    {
        var statusBase = base.Validar(doc);
        if (statusBase != "OK") return statusBase;

        int idade = CalcularIdade(doc.DataNascimento);
        if (idade < 18)
            return $"REJEITADO: Menor de idade ({idade} anos) para CNH.";

        return "APROVADO: CNH validada com sucesso.";
    }
}