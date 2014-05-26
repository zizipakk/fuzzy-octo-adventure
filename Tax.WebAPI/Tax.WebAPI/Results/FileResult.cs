using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Tax.WebAPI.Results
{
    public class FileResult : IHttpActionResult
    {
        byte[] fileContent;
        string fileName;
        string mimeType;

        //public FileResult(byte[] content, string fileName, string mimeType = "application/octet-stream")
        public FileResult(byte[] content, string fileName, string mimeType)
        {
            this.fileContent = content;
            this.fileName = fileName;
            this.mimeType = mimeType;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage();
            response.Content = new StreamContent(new MemoryStream(fileContent));

            var contentHeaders = response.Content.Headers;
            contentHeaders.ContentType = new MediaTypeHeaderValue(mimeType);
            contentHeaders.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            contentHeaders.ContentDisposition.FileName = Path.GetFileName(fileName);

            return Task.FromResult(response);
        }
    }
}