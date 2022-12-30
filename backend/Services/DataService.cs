using System.Text.RegularExpressions;
using backend.Models;
using Microsoft.VisualBasic;
using Constants = backend.Cors.Constants;

namespace backend.Services;

public interface IDataService
{
    string GetDataPdf(); // change returned type to pdf or other shit
    void SaveData(Data data);
}

public class DataService : IDataService
{
    public string GetDataPdf()
    {
        return "hello world";
    }

    public void SaveData(Data data)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), Constants.StorageName, data.FullDateTime);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var regex = new Regex(@"^[\w/\:.-]+;base64,");
        var base64 = regex.Replace(data.Photo,string.Empty);
        
        var image = Convert.FromBase64String(base64);
        var dateTimeNow = DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
        var fileName = $"{path}/{dateTimeNow}.jpg";
        File.WriteAllBytes(fileName, image);
    }
    
    // var cleandata = data.Photo.Replace("data:image/png;base64,", "");
    // var data1 = System.Convert.FromBase64String(cleandata);
    // var ms = new MemoryStream(data1);
    // var img = System.Drawing.Image.FromStream(ms);
    // img.Save(path + "zdj.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);


    // var stream = new FileStream(path, FileMode.Create);

    // var fileInfo = new FileInfo(model.File.FileName);
    // string fileName = model.FileName + fileInfo.Extension;
    //
    // string fileNameWithPath = Path.Combine(path, fileName);
    //
    // using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
    // {
    //     model.File.CopyTo(stream);
    // }
    // var image = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw==");
    // string converted = data.Photo.Replace('-', '+');
    // converted = converted.Replace('_', '/');
        
    // var cleandata = data.Photo.Replace("data:image/png;base64,", "");
    // public Image LoadImage()
    // {
    //     //data:image/gif;base64,
    //     //this image is a single pixel (black)
    //     byte[] bytes = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw==");
    //
    //     Image image;
    //     using (MemoryStream ms = new MemoryStream(bytes))
    //     {
    //         image = Image.FromStream(ms);
    //     }
    //
    //     return image;
    // }
}