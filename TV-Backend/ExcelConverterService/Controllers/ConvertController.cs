using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Threading;

namespace ExcelConverterService.Controllers
{
    [Route("api/convert")]
    
    [ApiController]
    public class ConvertController : Controller
    {
        [STAThread]
        [HttpPost("jpg")]
        public IActionResult xslToJpg()
        {
            //string excelPath = null;
            //using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            //{
            //    excelPath = await reader.ReadToEndAsync();
            //}


            var res = ExcelToJpg();
            return Json(res);
        }
        [STAThread]
        private string ExcelToJpg()
        {
            Excel.Application excelApp = new Excel.Application
            {
                Visible = true
            };
            Excel.Workbook workbook = null;
            string savePath = null;
            try
            {
                workbook = excelApp.Workbooks.Open(@"C:\Users\User\Documents\GitHub\KinectTV-Backend\TV-Backend\ExcelConverterService\IIT_1k_19_20_vesna.xlsx");
                Excel.Worksheet worksheet = workbook.Worksheets[1];
                string startRange = "A1";
                Excel.Range endRange = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                Excel.Range range = worksheet.get_Range(startRange, endRange);
                while (!range.Copy())
                {
                    Thread.Sleep(500);
                }

                var data = Clipboard.ContainsImage();
                Image imgRange1 = Clipboard.GetImage();
                savePath = Path.Combine(@"C:\Users\User\Desktop\", $"{Path.GetFileNameWithoutExtension(@"C:\Users\User\Documents\GitHub\KinectTV-Backend\TV-Backend\ExcelConverterService\IIT_1k_19_20_vesna.xlsx")}.jpg");
                imgRange1.Save(savePath, ImageFormat.Jpeg);
                Console.WriteLine("Specified range converted to image successfully. Press Enter to continue.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                workbook.Application.CutCopyMode = Excel.XlCutCopyMode.xlCopy;
                workbook?.Close(false);
                excelApp?.Quit();
                excelApp = null;
            }
            return savePath;
        }
    }
}