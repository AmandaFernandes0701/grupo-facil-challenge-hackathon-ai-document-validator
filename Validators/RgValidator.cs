using HackathonValidador.Models;

namespace HackathonValidador.Validators;

public class RgValidator : DocumentoValidator
{
    public override string TipoDoc => "RG";

    public override string Validar(DocumentoExtraido doc)
    {
        var statusBase = base.Validar(doc);
        if (statusBase != "OK") return statusBase;

        if (doc is RgExtraido rg)
        {
            if (string.IsNullOrWhiteSpace(rg.DataEmissao))
                return "REJEITADO: Data de emissão obrigatória para RG não encontrada.";
        }
        return "APROVADO: RG validado com sucesso.";
    }
}