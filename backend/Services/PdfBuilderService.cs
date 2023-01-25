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
        var images = Directory.GetFiles(path);
        var photos = images.Where(f => f.Contains("photo"));
        var banner = images.First(f => f.Contains("banner"));

        var htmlContent = @"<!DOCTYPE html>
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
      background: #c0c0c0;
    }
    table {
      width: 100%;
      border: none;
    }
    td {
      width: 30%;
      text-align: center;
      word-wrap: break-word;
      line-height: 0;
    }
    .banner {
      width: 20%
    }
    img {
      width: 100%;
    }
    .rotate {
      color: white;
      writing-mode: vertical-rl;
      transform: rotate(180deg);
      font-size: 20px;
      font-weight: 700;
    }
  </style>
</head>
<body>
  <div class='wrapper'>
    <table cellspacing='0' cellpadding='0'>
      @Table
    </table>
  </div>
</body>
</html>";

        var table = "";
        var numberOfPhotos = photos.Count();
        var bannerText = "";
        if (text is not null)
        {
          bannerText = @$"<td class='banner' rowspan='{numberOfPhotos}' background='@BannerImage'><span class='rotate'>{text}</span></td>";
        }

        foreach (var (photo, index) in photos.Select((value, i) => (value, i)))
        {
            var jpgBinaryData = File.ReadAllBytes(photo);
            var imgDataUri = @"data:image/png;base64," + Convert.ToBase64String(jpgBinaryData);
            if (index == 0 && !string.IsNullOrEmpty(bannerText))
            {
              table += @$"<tr><td><img src='{imgDataUri}'></td>{bannerText}<td><img src='{imgDataUri}'></td>{bannerText}</tr>";
            }
            else
            {
              table += @$"<tr><td><img src='{imgDataUri}'></td><td><img src='{imgDataUri}'></td></tr>";
            }
        }

        var bannerBinaryData = File.ReadAllBytes(banner);
        var bannerDataUri = @"data:image/png;base64," + Convert.ToBase64String(bannerBinaryData);

        htmlContent = ReplaceAllOccurrence(htmlContent, "@Table", table);
        htmlContent = ReplaceAllOccurrence(htmlContent, "@BannerImage", bannerDataUri);
        
        var renderer = new ChromePdfRenderer
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
        var pdfDocument = renderer.RenderHtmlAsPdf(htmlContent);
        pdfDocument.SaveAs($"{path}/result.pdf");
    }
    
    private static string ReplaceAllOccurrence(string source, string find, string replace)
    {
        return source.Replace(find, replace);
    }
}