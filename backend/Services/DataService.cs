using System.Text.RegularExpressions;
using backend.Models;
using Microsoft.VisualBasic;
using Constants = backend.Cors.Constants;

namespace backend.Services;

public interface IDataService
{
    void SaveData(Data data);
}

public class DataService : IDataService
{
    private readonly IPdfBuilderService _pdfBuilderService;

    public DataService(IPdfBuilderService pdfBuilderService)
    {
        _pdfBuilderService = pdfBuilderService;
    }
    
    public void SaveData(Data data)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), Constants.StorageName, data.FullDateTime);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var regex = new Regex(@"^[\w/\:.-]+;base64,");
        var dateTimeNow = DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
        var base64 = "";

        if (data.Photo is not null)
        {
            base64 = regex.Replace(data.Photo,string.Empty);
        } 
        else if (data.Banner is not null)
        {
            base64 = regex.Replace(data.Banner,string.Empty);
        }

        if (string.IsNullOrEmpty(base64)) return;
        
        var image = Convert.FromBase64String(base64);
        var fileName = $"{path}/{dateTimeNow}.jpg";
        File.WriteAllBytes(fileName, image);

        if (data.Banner is not null)
        {
            _pdfBuilderService.BuildPdf();
        }
    }
}