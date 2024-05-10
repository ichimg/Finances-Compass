using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IHangfireService
    {
        Task<string> ScheduleDeadlineEmails(DebtAssignment debtAssignment, ReceiverInfoDto receiverInfoDto);
        Task DeleteScheduledJob(string jobId);
    }
}