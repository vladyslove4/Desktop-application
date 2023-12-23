using CsvHelper;
using CsvHelper.Configuration;
using DesktopApp.Model;
using DesktopApp.Model.EntityDto;
using DesktopApp.Service;
using System.Globalization;

namespace DesktopApp.Tests.DesktopApp.Service.Tests
{
    public class CsvServiceTests
    {
        [Fact]
        public async Task ExportCSV_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            var csvService = new CsvService();
            var testData = new List<StudentDto>
        {
            new StudentDto { FirstName = "John", LastName = "Doe" },
            new StudentDto { FirstName = "Jane", LastName = "Smith" }
        };

            // Act
            var result = await csvService.ExportCSV(testData);

            // Assert
            Assert.True(result);
            
            File.Delete("ListOfStudent.CSV");
        }

        [Fact]
        public void ImportCSV_WithValidFilePath_ShouldReturnListOfStudentCSV()
        {
            // Arrange
            var csvService = new CsvService();
            var testData = new List<StudentCSV>
        {
            new StudentCSV { FirstName = "John", LastName = "Doe" },
            new StudentCSV { FirstName = "Jane", LastName = "Smith" }
        };
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
            var filePath = "test.csv";

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, csvConfiguration))
            {
                csv.WriteRecords(testData);
            }

            // Act
            var result = csvService.ImportCSV(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("John", result[0].FirstName);
            Assert.Equal("Doe", result[0].LastName);
            Assert.Equal("Jane", result[1].FirstName);
            Assert.Equal("Smith", result[1].LastName);

            
            File.Delete(filePath);
        }

        [Fact]
        public void ImportCSV_WithInvalidFilePath_ShouldReturnNull()
        {
            // Arrange
            var csvService = new CsvService();
            var filePath = "nonexistent.csv";

            // Act
            var result = csvService.ImportCSV(filePath);

            // Assert
            Assert.Null(result);
        }
    }
}
