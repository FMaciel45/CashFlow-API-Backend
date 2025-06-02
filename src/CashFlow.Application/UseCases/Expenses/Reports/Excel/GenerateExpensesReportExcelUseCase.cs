using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using ClosedXML.Excel; // Pacote para manipulação de Excel

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;
public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
{
    private const string CURRENCY_SYMBOL = "€"; // Símbolo monetário configurável
    private readonly IExpensesReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }

    // Método principal que gera o relatório em Excel
    public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUser = await _loggedUser.Get();

        // Filtra despesas pelo mês especificado
        var expenses = await _repository.FilterByMonth(loggedUser, month);

        // Retorna array vazio se não houver despesas
        if(expenses.Count == 0)
        {
            return [];
        }

        // Cria um novo workbook do Excel
        using var workbook = new XLWorkbook();

        // Configura metadados e estilo padrão do workbook
        workbook.Author = loggedUser.Name;
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";

        // Adiciona worksheet com nome formatado
        var worksheet = workbook.Worksheets.Add(month.ToString("Y"));

        // Insere cabeçalho da planilha
        InsertHeader(worksheet);

        // Começa na linha 2 (após cabeçalho)
        var raw = 2;

        foreach(var expense in expenses)
        {
            // Preenche cada coluna com os dados da despesa
            worksheet.Cell($"A{raw}").Value = expense.Title;
            worksheet.Cell($"B{raw}").Value = expense.Date;
            worksheet.Cell($"C{raw}").Value = expense.PaymentType.PaymentTypeToString();

            // Formata valor monetário
            worksheet.Cell($"D{raw}").Value = expense.Amount;
            worksheet.Cell($"D{raw}").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL} #,##0.00";
            
            worksheet.Cell($"E{raw}").Value = expense.Description;

            raw++; // Incrementa para próxima linha
        }

        // Ajusta largura das colunas ao conteúdo
        worksheet.Columns().AdjustToContents();

        // Salva em MemoryStream e converte para byte[]
        var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    // Método para configurar o cabeçalho da planilha
    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

        // Formatação do cabeçalho
        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6"); // Cor de fundo

        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    }
}