using LabelPrintingApp.Models;
using PuppeteerSharp;
using NetBarcode;
using System.Collections.Generic;
using System.Threading.Tasks;
using PuppeteerSharp.Media;

namespace LabelPrintingApp.Services
{
    public class BatchLabelGenerator
    {
        private readonly LabelGenerator _labelGenerator;

        public BatchLabelGenerator(LabelGenerator labelGenerator)
        {
            _labelGenerator = labelGenerator;
        }

        public async Task<List<string>> GenerateBatchPdfAsync(List<Artikel> artikels)
        {
            
            var outputFiles = new List<string>();
            await new BrowserFetcher().DownloadAsync();
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

            foreach (var artikel in artikels)
            {
                var barcode = new Barcode(artikel.EanCode, NetBarcode.Type.EAN13);
                string base64Barcode = barcode.GetBase64Image();

                string htmlTemplate = $@"
                <html>
                    <body style='margin:0;padding:0;'>
                        <div style='width:100%;height:100%;border:1px solid black;padding:10px;font-family:sans-serif;'>
                            <h2>Artikel: {artikel.Artikelnummer}</h2>
                            <p>Kleur: {artikel.Kleur}</p>
                            <p>Maat: {artikel.Maat}</p>
                            <img src='data:image/png;base64,{base64Barcode}' />
                        </div>
                    </body>
                </html>";

                var page = await browser.NewPageAsync();
                await page.SetContentAsync(htmlTemplate);

                string outputFile = $"label_{artikel.Artikelnummer}.pdf";
                await page.PdfAsync(outputFile, new PdfOptions
                {
                    Format = PaperFormat.A5,
                    PrintBackground = true,
                    MarginOptions = new MarginOptions { Top = "10px", Bottom = "10px" }
                });

                outputFiles.Add(outputFile);
                await page.CloseAsync();
            }

            await browser.CloseAsync();
            return outputFiles;
        }
    }
}
