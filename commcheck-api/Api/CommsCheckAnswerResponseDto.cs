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
            answer.App.IsAllowed() ? CommAllowedEnum.Allowed: CommAllowedEnum.Blocked,
            answer.Email.IsAllowed() ? CommAllowedEnum.Allowed: CommAllowedEnum.Blocked,
            answer.SMS.IsAllowed() ? CommAllowedEnum.Allowed: CommAllowedEnum.Blocked,
            answer.Postal.IsAllowed() ? CommAllowedEnum.Allowed: CommAllowedEnum.Blocked,
            answer.App.Reason,
            answer.Email.Reason,
            answer.SMS.Reason,
            answer.Postal.Reason
            );
        
    }
}
