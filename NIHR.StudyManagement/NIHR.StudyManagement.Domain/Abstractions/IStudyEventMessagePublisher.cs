using NIHR.StudyManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIHR.StudyManagement.Domain.Abstractions
{
    public interface IStudyEventMessagePublisher
    {
        Task PublishAsync(string eventType,
            string sourceSystemName,
            GovernmentResearchIdentifier governmentResearchIdentifier,
            CancellationToken cancellationToken);
    }
}
