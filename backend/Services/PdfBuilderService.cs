namespace backend.Services;

using IronPdf;

public interface IPdfBuilderService
{
    void BuildPdf();
}

public class PdfBuilderService : IPdfBuilderService
{
    public void BuildPdf()
    {
        var Renderer = new ChromePdfRenderer(); // Instantiates Chrome Renderer
        var pdf = Renderer.RenderHtmlAsPdf(" <h1> ~Hello World~ </h1> Made with IronPDF!");
        pdf.SaveAs("html_saved.pdf"); // Saves our PdfDocument object as a PDF
    }
}