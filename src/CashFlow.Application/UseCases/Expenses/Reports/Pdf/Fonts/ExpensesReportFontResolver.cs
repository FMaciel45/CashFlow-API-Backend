using PdfSharp.Fonts; // Para interfaces de resolução de fontes
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
public class ExpensesReportFontResolver : IFontResolver
{
    // Implementação customizada de IFontResolver para PDFSharp

    // Método para obter os dados da fonte (arquivo .ttf)
    public byte[]? GetFont(string faceName)
    {
        // Tenta ler o arquivo de fonte solicitado
        var stream = ReadFontFile(faceName);

        // Se não encontrado, usa a fonte padrão 
        stream ??= ReadFontFile(FontHelper.DEFAULT_FONT);

        // Prepara buffer para os dados da fonte
        var length = (int)stream!.Length;
        var data = new byte[length];

        // Lê os dados do stream para o array de bytes
        stream.Read(buffer: data, offset: 0, count: length);

        return data;
    }

    // Método para resolver qual fonte usar baseado nos parâmetros
    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }

    // Método privado para ler o arquivo de fonte dos recursos embutidos
    private Stream? ReadFontFile(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Constrói o caminho do recurso embutido
        return assembly.GetManifestResourceStream($"CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts.{faceName}.ttf");
    }
}