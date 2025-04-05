using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportApi.Application.Interfaces
{
    public interface IReportConsumerService
    {
        Task SendReportRequest(Guid reportId);
    }
}
