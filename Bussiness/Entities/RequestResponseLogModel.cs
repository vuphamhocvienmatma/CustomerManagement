using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Entities
{
    public class RequestResponseLog
    {
        [Key]
        public string LogId { get; set; }           /*Guid.NewGuid().ToString()*/
        public string Node { get; set; }            /*project name*/
        public string UserId { get; set; }
        public string TraceId { get; set; }         /*HttpContext TraceIdentifier*/
        public DateTime? RequestDateTimeUtc { get; set; }
        public string RequestPath { get; set; }
        public string RequestQuery { get; set; }
        public string RequestMethod { get; set; }
        public string RequestHost { get; set; }
        public string RequestHeaders { get; set; }
        public string RequestBody { get; set; }
        public DateTime? ResponseDateTimeUtc { get; set; }
        public string ResponseStatus { get; set; }
        public string ResponseHeaders { get; set; }
        public string ResponseBody { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }

        public RequestResponseLogModel()
        {
            LogId = Guid.NewGuid().ToString();
        }
    }
}
