using CsvHelper;
using CsvHelper.Configuration;
using DesktopApp.Model;
using DesktopApp.Model.EntityDto;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopApp.Service;

public class CsvService
{
    private readonly string _csvFilePath = "ListOfStudent.CSV";

    List<StudentCSV> studentCSVs = new List<StudentCSV>();
    
    public async Task<bool> ExportCSV(List<StudentDto> data)
    {
        var studentCSVs = ConvertDataToCSVmodel(data);
        using (var writer = new StreamWriter(_csvFilePath))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            await csv.WriteRecordsAsync(studentCSVs);
        }
        return true;
    }
    public  List<StudentCSV>? ImportCSV(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            
            return null;
        }
        try
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = csv.GetRecords<StudentCSV>().ToList();
                return records;
            }
        }
        catch (System.Exception)
        {

            return null;
        }
        
    }
    private List<StudentCSV> ConvertDataToCSVmodel(List<StudentDto> data)
    {
        studentCSVs.Clear();

        foreach (var student in data)
        {
            studentCSVs.Add(new StudentCSV { FirstName = student.FirstName, LastName = student.LastName });
        }
        return studentCSVs;
    }
}