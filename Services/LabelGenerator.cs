using PuppeteerSharp;
using LabelPrintingApp.Models;
using NetBarcode;
using System.Threading.Tasks;
using PuppeteerSharp.Media;

namespace LabelPrintingApp.Services
{
    public class LabelGenerator
    {
        public async Task<string> GeneratePdfAsync(Artikel artikel, string outputFile)
        {
            // Barcode genereren
            var barcode = new Barcode(artikel.EanCode, NetBarcode.Type.EAN13);
            string base64Barcode = barcode.GetBase64Image();


            // HTML template
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

            // PuppeteerSharp browser starten
            await new BrowserFetcher().DownloadAsync();
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            var page = await browser.NewPageAsync();

            await page.SetContentAsync(htmlTemplate);
            await page.PdfAsync(outputFile, new PdfOptions
            {
                Format = PaperFormat.A5,
                PrintBackground = true,
                MarginOptions = new MarginOptions { Top = "10px", Bottom = "10px" }
            });

            await browser.CloseAsync();
            return outputFile;
        }
    }
}
