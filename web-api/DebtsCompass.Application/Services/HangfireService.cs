using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Entities.EmailDtos;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain;
using Hangfire;
using DebtsCompass.Domain.Interfaces;

namespace DebtsCompass.Application.Services
{
    public class HangfireService : IHangfireService
    {
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly IEmailService emailService;

        public HangfireService(IBackgroundJobClient backgroundJobClient, IEmailService emailService)
        {
            this.backgroundJobClient = backgroundJobClient;
            this.emailService = emailService;
        }

        public async Task<string> ScheduleDeadlineEmails(DebtAssignment debtAssignment, ReceiverInfoDto receiverInfoDto)
        {
            DebtEmailInfoDto debtEmailInfoDto = Mapper.DebtAssignmentToDebtEmailInfoDto(debtAssignment);
            ReceiverInfoDto creatorUserReceiverInfoDto = Mapper.UserToReceiverInfoDto(debtAssignment.CreatorUser);
            LoanEmailInfoDto loanEmailInfoDto = Mapper.DebtAssignmentToLoanEmailInfoDto(debtAssignment);
            TimeSpan oneDayBeforeDeadline = (debtAssignment.Debt.DeadlineDate - DateTime.UtcNow.Date).Subtract(TimeSpan.FromDays(1));

            string jobId = backgroundJobClient.Schedule(() => 
            emailService.SendDeadlineEmails(receiverInfoDto, debtEmailInfoDto, creatorUserReceiverInfoDto, loanEmailInfoDto), 
            oneDayBeforeDeadline);

            return jobId;
        }

        public async Task DeleteScheduledJob(string jobId)
        {
            if (jobId is null)
            {
                return;
            }

            backgroundJobClient.Delete(jobId);
        }
    }
}
