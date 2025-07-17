using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Esfsg.Application.Helpers
{
    public static class PdfHelper<T>
    {
        public static byte[] ExportarParaPdf(IEnumerable<T> dados, string titulo = "Relatório")
        {
            if (dados == null || !dados.Any())
                throw new ArgumentNullException(nameof(dados));

            var propriedades = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var nomesCabecalho = propriedades
            .Select(p =>
            {
                var displayAttr = p.GetCustomAttributes(typeof(DisplayAttribute), false)
                                   .FirstOrDefault() as DisplayAttribute;
                return displayAttr?.Name ?? p.Name;
            })
            .ToList();

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text(titulo)
                        .SemiBold()
                        .FontSize(20)
                        .FontColor(Colors.Black)
                        .AlignCenter();

                    page.Content()
                        .Table(table =>
                        {
                            // Define colunas iguais para cada propriedade
                            table.ColumnsDefinition(columns =>
                            {
                                foreach (var _ in propriedades)
                                {
                                    columns.RelativeColumn();
                                }
                            });

                            // Cabeçalho da tabela
                            table.Header(header =>
                            {
                                foreach (var prop in nomesCabecalho)
                                {
                                    header.Cell()
                                          .Background(Colors.Grey.Lighten2)
                                          .Padding(2)
                                          .Text(prop)
                                          .FontSize(9)
                                          .WrapAnywhere()
                                          .SemiBold();
                                }
                            });

                            // Linhas de dados
                            foreach (var item in dados)
                            {
                                foreach (var prop in propriedades)
                                {
                                    var valor = prop.GetValue(item)?.ToString() ?? "";
                                    table.Cell()
                                         .Padding(5)
                                         .Text(valor)
                                         .FontSize(9)
                                         .WrapAnywhere();
                                }
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Página ");
                            text.CurrentPageNumber();
                            text.Span(" de ");
                            text.TotalPages();
                        });
                });
            });

            return documento.GeneratePdf();
        }
    }
}
