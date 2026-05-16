using System.Globalization;
using HackathonValidador.Models;

namespace HackathonValidador.Validators;

public class PassaporteValidator : DocumentoValidator
{
    public override string TipoDoc => "PASSAPORTE";

    public override string Validar(DocumentoExtraido doc)
    {
        var statusBase = base.Validar(doc);
        if (statusBase != "OK") return statusBase;

        if (doc is PassaporteExtraido passaporte)
        {
            if (string.IsNullOrWhiteSpace(passaporte.DataValidade))
                return "REJEITADO: Data de validade não encontrada.";

            if (string.IsNullOrWhiteSpace(passaporte.PaisEmissor))
                return "REJEITADO: País emissor não encontrado.";

            if (!DateTime.TryParseExact(passaporte.DataValidade, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validade))
            {
                return "REJEITADO: Formato da data de validade inválido. Use dd/MM/yyyy.";
            }

            if (validade < DateTime.Today)
                return "REJEITADO: O Passaporte encontra-se vencido.";
        }

        return "APROVADO: Passaporte internacional validado.";
    }
}