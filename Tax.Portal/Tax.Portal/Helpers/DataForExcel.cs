using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Instrumentation;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Globalization;
using System.Web.Mvc;
using System.IO;

namespace Tax.Portal.Helpers
{
    // to send exceptions as json we define [HandleJsonException] attribute
    public class ExceptionInformation
    {
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
    }

    public class HandleJsonExceptionAttribute : ActionFilterAttribute
    {
        // next class example are from the http://www.dotnetcurry.com/ShowArticle.aspx?ID=496
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest() || filterContext.Exception == null) return;
            filterContext.HttpContext.Response.StatusCode =
                (int)System.Net.HttpStatusCode.InternalServerError;

            var exInfo = new List<ExceptionInformation>();
            for (var ex = filterContext.Exception; ex != null; ex = ex.InnerException)
            {
                exInfo.Add(new ExceptionInformation
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    StackTrace = ex.StackTrace
                });
            }
            filterContext.Result = new System.Web.Mvc.JsonResult { Data = exInfo };
            filterContext.ExceptionHandled = true;
        }
    }

    public class ExcelResult : ActionResult
    {
        private readonly DataForExcel _data;
        private readonly string _fileName;

        public ExcelResult(string[] headers, List<string[]> data, string fileName, string sheetName)
        {
            _data = new DataForExcel(headers, data, sheetName);
            _fileName = fileName;
        }

        public ExcelResult(string[] headers, DataForExcel.DataType[] colunmTypes, List<string[]> data, string fileName, string sheetName)
        {
            _data = new DataForExcel(headers, colunmTypes, data, sheetName);
            _fileName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ClearContent();
            response.ClearHeaders();
            response.Cache.SetMaxAge(new TimeSpan(0));

            using (var stream = new MemoryStream())
            {
                _data.CreateXlsxAndFillData(stream);

                //Return it to the client - strFile has been updated, so return it. 
                response.AddHeader("content-disposition", "attachment; filename=" + _fileName);

                // see http://filext.com/faq/office_mime_types.php
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.ContentEncoding = Encoding.UTF8;
                stream.WriteTo(response.OutputStream);
            }
            response.Flush();
            response.Close();
        }
    }

    public class DataForExcel
    {
        public enum DataType
        {
            String,
            Integer
        }
        private readonly string[] _headers;
        private readonly DataType[] _colunmTypes;
        private readonly List<string[]> _data;
        private readonly string _sheetName = "Grid1";
        private readonly SortedSet<string> _os = new SortedSet<string>();
        private string[] _sharedStrings;

        private static string ConvertIntToColumnHeader(int index)
        {
            var sb = new StringBuilder();
            while (index > 0)
            {
                if (index <= 'Z' - 'A') // index=0 -> 'A', 25 -> 'Z'
                    break;
                sb.Append(ConvertIntToColumnHeader(index / ('Z' - 'A' + 1) - 1));
                index = index % ('Z' - 'A' + 1);
            }
            sb.Append((char)('A' + index));
            return sb.ToString();
        }

        private static Row CreateRow(UInt32 index, IList<string> data)
        {
            var r = new Row { RowIndex = index };
            for (var i = 0; i < data.Count; i++)
                r.Append(new OpenXmlElement[] { CreateTextCell(ConvertIntToColumnHeader(i), index, data[i]) });

            return r;
        }

        private Row CreateRowWithSharedStrings(UInt32 index, IList<string> data)
        {
            var r = new Row { RowIndex = index };
            for (var i = 0; i < data.Count; i++)
                r.Append(new OpenXmlElement[] { CreateSharedTextCell(ConvertIntToColumnHeader(i), index, data[i]) });

            return r;
        }

        private Row CreateRowWithSharedStrings(UInt32 index, IList<string> data, IList<DataType> colunmTypes)
        {
            var r = new Row { RowIndex = index };
            for (var i = 0; i < data.Count; i++)
                if (colunmTypes != null && i < colunmTypes.Count && colunmTypes[i] == DataType.Integer)
                    r.Append(new OpenXmlElement[] { CreateNumberCell(ConvertIntToColumnHeader(i), index, data[i]) });
                else
                    r.Append(new OpenXmlElement[] { CreateSharedTextCell(ConvertIntToColumnHeader(i), index, data[i]) });

            return r;
        }

        private static Cell CreateTextCell(string header, UInt32 index, string text)
        {
            // create Cell with InlineString as a child, which has Text as a child
            return new Cell(new InlineString(new Text { Text = text }))
            {
                // Cell properties
                DataType = CellValues.InlineString,
                CellReference = header + index
            };
        }

        private Cell CreateSharedTextCell(string header, UInt32 index, string text)
        {
            for (var i = 0; i < _sharedStrings.Length; i++)
            {
                if (String.Compare(_sharedStrings[i], text, StringComparison.Ordinal) == 0)
                {
                    return new Cell(new CellValue { Text = i.ToString(CultureInfo.InvariantCulture) })
                    {
                        // Cell properties
                        DataType = CellValues.SharedString,
                        CellReference = header + index
                    };
                }
            }
            // create Cell with InlineString as a child, which has Text as a child
            throw new InstanceNotFoundException();
        }

        private static Cell CreateNumberCell(string header, UInt32 index, string numberAsString)
        {
            // create Cell with CellValue as a child, which has Text as a child
            return new Cell(new CellValue { Text = numberAsString })
            {
                // Cell properties
                CellReference = header + index
            };
        }

        private void FillSharedStringTable(IEnumerable<string> data)
        {
            foreach (var item in data)
                _os.Add(item);
        }

        private void FillSharedStringTable(IList<string> data, IList<DataType> colunmTypes)
        {
            for (var i = 0; i < data.Count; i++)
                if (colunmTypes == null || i >= colunmTypes.Count || colunmTypes[i] == DataType.String)
                    _os.Add(data[i]);
        }

        public DataForExcel(string[] headers, List<string[]> data, string sheetName)
        {
            _headers = headers;
            _data = data;
            _sheetName = sheetName;
        }

        public DataForExcel(string[] headers, DataType[] colunmTypes, List<string[]> data, string sheetName)
        {
            _headers = headers;
            _colunmTypes = colunmTypes;
            _data = data;
            _sheetName = sheetName;
        }

        private void FillSpreadsheetDocument(SpreadsheetDocument spreadsheetDocument)
        {
            // create and fill SheetData
            var sheetData = new SheetData();

            // first row is the header
            sheetData.AppendChild(CreateRow(1, _headers));

            //const UInt32 iAutoFilter = 2;
            // skip next row (number 2) for the AutoFilter
            //var i = iAutoFilter + 1;
            UInt32 i = 2;

            // first of all collect all different strings in OrderedSet<string> _os
            foreach (var dataRow in _data)
                if (_colunmTypes != null)
                    FillSharedStringTable(dataRow, _colunmTypes);
                else
                    FillSharedStringTable(dataRow);
            _sharedStrings = _os.ToArray();

            foreach (var dataRow in _data)
                sheetData.AppendChild(_colunmTypes != null
                                          ? CreateRowWithSharedStrings(i++, dataRow, _colunmTypes)
                                          : CreateRowWithSharedStrings(i++, dataRow));

            var sst = new SharedStringTable();
            foreach (var text in _os)
                sst.AppendChild(new SharedStringItem(new Text(text)));

            // add empty workbook and worksheet to the SpreadsheetDocument
            var workbookPart = spreadsheetDocument.AddWorkbookPart();
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

            var shareStringPart = workbookPart.AddNewPart<SharedStringTablePart>();
            shareStringPart.SharedStringTable = sst;

            shareStringPart.SharedStringTable.Save();

            // add sheet data to Worksheet
            worksheetPart.Worksheet = new Worksheet(sheetData);
            worksheetPart.Worksheet.Save();

            // fill workbook with the Worksheet
            spreadsheetDocument.WorkbookPart.Workbook = new Workbook(
                    new FileVersion { ApplicationName = "Microsoft Office Excel" },
                    new Sheets(
                        new Sheet
                        {
                            Name = _sheetName,
                            SheetId = (UInt32Value)1U,

                            // generate the id for sheet
                            Id = workbookPart.GetIdOfPart(worksheetPart)
                        }
                    )
                );
            spreadsheetDocument.WorkbookPart.Workbook.Save();
            spreadsheetDocument.Close();
        }

        public void CreateXlsxAndFillData(Stream stream)
        {
            // Create workbook document
            using (var spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                FillSpreadsheetDocument(spreadsheetDocument);
            }
        }
    }
}