using LabelPrintingApp.Models;
using LabelPrintingApp.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PrintLabelsModel : PageModel
{
    private readonly BatchLabelGenerator _batchLabelGenerator;

    public PrintLabelsModel(BatchLabelGenerator batchLabelGenerator)
    {
        _batchLabelGenerator = batchLabelGenerator;
    }

    public async Task OnGetAsync()
    {
        // Testdata
        var artikels = new List<Artikel>
        {
            new Artikel { Artikelnummer="ART1", Kleur="Rood", Maat="L", EanCode="123456789012" },
            new Artikel { Artikelnummer="ART2", Kleur="Blauw", Maat="M", EanCode="987654321098" }
        };

        // PDF genereren (testmodus)
        var pdfFiles = await _batchLabelGenerator.GenerateBatchPdfAsync(artikels);

        // Alleen feedback in console / logs
        foreach (var file in pdfFiles)
        {
            Console.WriteLine($"PDF gegenereerd: {file}");
        }
    }
    
}
