namespace CommsCheck;

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
            GetEnum(answer.Outcomes, "App"),
            GetEnum(answer.Outcomes, "Email"),
            GetEnum(answer.Outcomes, "Sms"),
            GetEnum(answer.Outcomes, "Postal"),
            GetReason(answer.Outcomes, "App"),
            GetReason(answer.Outcomes, "Email"),
            GetReason(answer.Outcomes, "Sms"),
            GetReason(answer.Outcomes, "Postal")
            );  
    }

    private static CommAllowedEnum GetEnum(IEnumerable<IRuleOutcome> outcomes, string method)
    {
        var outCome = outcomes.Where(x=> x.Method == method).FirstOrDefault();
        if(outCome==null || outCome == default)
        {
            return CommAllowedEnum.Unknown;
        }
        else if(outCome.IsAllowed())
        {
            return CommAllowedEnum.Allowed;
        }
        else
        {
            return CommAllowedEnum.Blocked;
        }
    }

    private static string GetReason(IEnumerable<IRuleOutcome> outcomes, string method)
    {
        var outCome = outcomes.Where(x=> x.Method == method).FirstOrDefault();
        if(outCome==null || outCome == default)
        {
            return string.Empty;
        }
        else
        {
           return outCome.Reason;
        }
    }
}
