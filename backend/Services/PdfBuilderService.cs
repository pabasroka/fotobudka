namespace backend.Services;

using IronPdf;

public interface IPdfBuilderService
{
    void BuildPdf(string path, string? text);
}

public class PdfBuilderService : IPdfBuilderService
{
    public void BuildPdf(string path, string? text)
    {
        string[] images = Directory.GetFiles(path);
        IEnumerable<string> photos = images.Where(f => f.Contains("photo"));
        string banner = images.First(f => f.Contains("baner"));

        string ImgHtml = "";
        string htmlContent = @"<!DOCTYPE html>
<html lang='en'>
<head>
  <meta charset='UTF-8'>
  <meta http-equiv='X-UA-Compatible' content='IE=edge'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <title>Pdf</title>
  <style>
    body {
      margin: 0;
      padding: 0;
      width: 21cm;
      height: 29.7cm;
    }
    .wrapper {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      width: 100%;
      height: 100%;
    }
    table {
      width: 100%;
    }
    td {
      width: 50%;
    }
    img {
      width: 5cm;
    }
    .banner {
      transform: rotate(90deg);
    color: green;
    }
  </style>
</head>
<body>
  <div class='wrapper'>
    <h1>@Title</h1>
    <table>
      @Table
    </table>
  </div>
</body>
</html>";
        
        // if (text is not null)
        // {
        //   htmlContent = ReplaceFirstOccurrence(htmlContent, "@Title", text);
        // }

        string table = "";
        int numberOfPhotos = photos.Count();
        string bannerText = "";
        if (text is not null)
        {
          bannerText = @$"<td class='banner' rowspan='{numberOfPhotos}' background='@BannerImage'>{text}</td>";
        }

        foreach (var (photo, index) in photos.Select((value, i) => (value, i)))
        {
            byte[] jpgBinaryData = File.ReadAllBytes(photo);
            string ImgDataURI = @"data:image/png;base64," + Convert.ToBase64String(jpgBinaryData);
            if (index == 0 && !string.IsNullOrEmpty(bannerText))
            {
              table += @$"<tr><td><img src='{ImgDataURI}'></td>{bannerText}<td><img src='{ImgDataURI}'></td>{bannerText}</tr>";
            }
            else
            {
              table += @$"<tr><td><img src='{ImgDataURI}'></td><td><img src='{ImgDataURI}'></td></tr>";
            }
        }
        
        byte[] bannerBinaryData = File.ReadAllBytes(banner);
        string bannerDataURI = @"data:image/png;base64," + Convert.ToBase64String(bannerBinaryData);
        
        htmlContent = ReplaceFirstOccurrence(htmlContent, "@Table", table);
        htmlContent = ReplaceFirstOccurrence(htmlContent, "@BannerImage", bannerDataURI);
        
        ChromePdfRenderer renderer = new ChromePdfRenderer
        {
            RenderingOptions =
            {
                PaperSize = IronPdf.Rendering.PdfPaperSize.A4,
                MarginTop = 0,
                MarginLeft = 0,
                MarginRight = 0,
                MarginBottom = 0
            }
        };
        PdfDocument pdfDocument = renderer.RenderHtmlAsPdf(htmlContent);
        pdfDocument.SaveAs($"{path}/result.pdf");
    }
    
    private static string ReplaceFirstOccurrence(string source, string find, string replace)
    {
      int place = source.IndexOf(find, StringComparison.Ordinal);
      return source.Remove(place, find.Length).Insert(place, replace);
    }
}