using ClosedXML.Excel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Esfsg.Application.Helpers
{
    public static class ExcelHelper<T>
    {
        public static byte[] ExportarParaExcel(IEnumerable<T> dados, string titulo = "Relatório")
        {
            if (dados == null || !dados.Any())
                throw new ArgumentNullException(nameof(dados));

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(titulo);

            var propriedades = typeof(T).GetProperties();

            for (int i = 0; i < propriedades.Length; i++)
            {
                var displayAttr = propriedades[i].GetCustomAttribute<DisplayAttribute>();
                worksheet.Cell(1, i + 1).Value = displayAttr?.Name ?? propriedades[i].Name;
            }

            int row = 2;
            foreach (var item in dados)
            {
                for (int col = 0; col < propriedades.Length; col++)
                {
                    var valor = propriedades[col].GetValue(item);
                    worksheet.Cell(row, col + 1).SetValue(valor?.ToString() ?? string.Empty);
                }
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream.ToArray();
        }
    }
}
