public readonly record struct CommsCheckAnswerResponseDto(
    string ResultId, 
    string RequestString, 
    CommAllowedEnum App, 
    CommAllowedEnum Email, 
    CommAllowedEnum SMS, 
    CommAllowedEnum Postal,
    string AppReason,
    string EmailReason,
    string SMSReason,
    string PostalReason)
{
    public static CommsCheckAnswerResponseDto FromCommsCheckAnswer(CommsCheckAnswer answer)
    {
        return new CommsCheckAnswerResponseDto(
            answer.ResultId, 
            answer.RequestString, 
            answer.Outcomes.Where(x=>x.Method=="App").First().IsAllowed() ? CommAllowedEnum.Allowed: CommAllowedEnum.Blocked,
            answer.Outcomes.Where(x=>x.Method=="Email").First().IsAllowed() ? CommAllowedEnum.Allowed: CommAllowedEnum.Blocked,
            answer.Outcomes.Where(x=>x.Method=="Sms").First().IsAllowed() ? CommAllowedEnum.Allowed: CommAllowedEnum.Blocked,
            answer.Outcomes.Where(x=>x.Method=="Postal").First().IsAllowed() ? CommAllowedEnum.Allowed: CommAllowedEnum.Blocked,
            answer.Outcomes.Where(x=>x.Method=="App").First().Reason,
            answer.Outcomes.Where(x=>x.Method=="Email").First().Reason,
            answer.Outcomes.Where(x=>x.Method=="Sms").First().Reason,
            answer.Outcomes.Where(x=>x.Method=="Postal").First().Reason
            );
        
    }
}
