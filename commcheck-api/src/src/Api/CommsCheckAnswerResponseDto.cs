namespace CommsCheck;

public readonly record struct CommsCheckAnswerResponseDto(
    string ResultId, 
    string RequestString, 
    CommAllowed App, 
    CommAllowed Email, 
    CommAllowed SMS, 
    CommAllowed Postal,
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

    private static CommAllowed GetEnum(IEnumerable<IRuleOutcome> outcomes, string method)
    {
        var outCome = outcomes.FirstOrDefault(x=> x.Method == method);
        if(outCome==null)
        {
            return CommAllowed.Unknown;
        }
        else if(outCome.IsAllowed())
        {
            return CommAllowed.Allowed;
        }
        else
        {
            return CommAllowed.Blocked;
        }
    }

    private static string GetReason(IEnumerable<IRuleOutcome> outcomes, string method)
    {
        var outCome = outcomes.FirstOrDefault(x=> x.Method == method);
        if(outCome==null)
        {
            return string.Empty;
        }
        else
        {
           return outCome.Reason;
        }
    }
}
