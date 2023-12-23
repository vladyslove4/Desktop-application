using IronPdf;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DesktopApp.Model.EntityDto;
using DesktopApp.Model;
using System.Linq;

namespace DesktopApp.Service
{
    public class PdfService
    {
        List<StudentPDF> studentPDFs = new List<StudentPDF>();

        public async Task<bool> ExportPDF(List<StudentDto> data, string groupName, string courseName, CancellationToken cancellationToken)
        {
            var studentPDFs = ConvertDataToPDFmodel(data);

            var renderer = new ChromePdfRenderer();
            var pdf = renderer.RenderHtmlAsPdf($"<html><body><h1>List of students, group: {groupName}, course: {courseName}</h1><ul>{string.Join("", studentPDFs.Select(s => $"<li>{s.Name}</li>"))}</ul></body></html>");

            try
            {
                string rootFolderPath = AppDomain.CurrentDomain.BaseDirectory;
                string pdfFileName = Path.Combine(rootFolderPath, "StudentListPDF.pdf");

                await Task.Run(() =>
                {
                    pdf.SaveAs(pdfFileName);

                }, cancellationToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private List<StudentPDF> ConvertDataToPDFmodel(List<StudentDto> data)
        {
            studentPDFs.Clear();

            foreach (var student in data)
            {
                studentPDFs.Add(new StudentPDF { Name = $"{student.FirstName} {student.LastName}" });
            }
            return studentPDFs;
        }
    }
}