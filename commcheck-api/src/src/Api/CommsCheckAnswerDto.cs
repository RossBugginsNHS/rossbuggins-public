namespace CommsCheck;

public readonly record struct CommsCheckAnswerDto(    
    DateOnly RelativeDate,
    DateTime StartedAt,
    DateTime UpdatedAt,
    int UpdatedCount,
    CommAllowed App, 
    CommAllowed Email, 
    CommAllowed SMS, 
    CommAllowed Postal,
    string AppReason,
    string EmailReason,
    string SMSReason,
    string PostalReason)
    {
            public static CommsCheckAnswerDto FromCommsCheckAnswer(CommsCheckAnswer answer)
    {
        return new CommsCheckAnswerDto(
            answer.RelativeDate,
            answer.StartedAt,
            answer.UpdatedAt,
            answer.UpdatedCount,
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

        private static CommAllowed GetEnum(IEnumerable<IRuleOutcome> outcomes, string method) =>
        outcomes.FirstOrDefault(x=> x.Method == method) switch
        {
            null => CommAllowed.Unknown,
            IRuleOutcome {IsAllowed: true} => CommAllowed.Allowed,
            _ => CommAllowed.Blocked
        };
    

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
