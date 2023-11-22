using System.CommandLine;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Schema;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


var dobOption = new Option<DateOnly>(
    name: "--dob",
    description: "Date of birth (yyyy-mm-dd)")
{
    IsRequired = true
};

var baseUrl = new Option<Uri>(
    name: "--host",
    description: "Base url. defaults to https://commchecks.azurewebsites.net");
baseUrl.SetDefaultValue(new Uri("https://commchecks.azurewebsites.net"));

var rfrCode = new Option<string>(
    name: "--rfr",
    description: "Reason for removal code")
    {
        IsRequired = true
    };

var rootCommand = new RootCommand("Commcheck cli v1");
rootCommand.AddOption(dobOption);
rootCommand.AddOption(baseUrl);
rootCommand.AddOption(rfrCode);

rootCommand.SetHandler(async (dob, url, rfr) => 
    { 
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHttpClient<CommsCheck>(options=>
        {
            options.BaseAddress = url;
        });
        var host = builder.Build();
        var check = host.Services.GetRequiredService<CommsCheck>();
        var content = await check.Check(dob, Enum.Parse<ReasonForRemovals>(rfr));
    },
    dobOption, baseUrl, rfrCode);

return await rootCommand.InvokeAsync(args);



public class CommsCheck(HttpClient client, ILogger<CommsCheck> logger)
{
    public async Task<CommsCheckAnswerResponseDto>  Check(
        DateOnly dob, 
        ReasonForRemovals rfr)
    {
        var response = await client.PostAsJsonAsync(
            "check", 
            CommsCheckQuestionRequestDto.DobOnly(
                dob,
                rfr));

       
        var location = response.Headers.Location;

        var content = await Result(location);

        var options = new System.Text.Json.JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        options.Converters.Add(new JsonStringEnumConverter());

        var str = JsonSerializer.Serialize(content, options);

        logger.LogInformation("{str}", str);
        return content;
    }

    public async Task<CommsCheckAnswerResponseDto> Result(Uri location)
    {
        var response = await client.GetAsync(location);
        var options = new System.Text.Json.JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(new JsonStringEnumConverter());

        var o = await response.Content.ReadFromJsonAsync<CommsCheckAnswerResponseDto>(options);
        return o;
    }
}



public readonly record struct CommsCheckQuestionRequestDto(
    DateOnly DateOfBirth, 
    DateOnly DateOfSmsMostRecentUpdate,
    DateOnly DateOfEmailMostRecentUpdate,
    DateOnly DateOfAppMostRecentUpdate,
    DateOnly DateOfPostalMostRecentUpdate,
    DateOnly DateOfReasonForRemovalMostRecentUpdate,
    DeathStatus? DeathStatusValue,
    ReasonForRemovals? RfR)
{

    public static CommsCheckQuestionRequestDto DobOnly(
        DateOnly dateOfBirth, 
        ReasonForRemovals?reasonForRemoval) =>
        new CommsCheckQuestionRequestDto(
            dateOfBirth,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DateOnly.MinValue,
            DeathStatus.None,
            reasonForRemoval);
}



[Flags]
public enum ReasonForRemovals
{
    /// <summary>
    /// None
    /// </summary> 
    None = 0,

    /// <summary>
    /// REASON_DEATH
    /// </summary>    
    DEA = 1, 

    /// <summary>
    /// REASON_EMBARKATION
    /// </summary>   
    EMB= 2,  

    /// <summary>
    /// REASON_TRANSFERRED_TO_SCOTLAND
    /// </summary> 
    SCT = 4, 

    /// <summary>
    /// REASON_TRANSFERRED_TO_NORTHERN_IRELAND
    /// </summary> 
    NIT = 8, 

    /// <summary>
    /// REASON_ARMEDFORCES_FORCESN
    /// </summary> 
    AFN = 16, 

    /// <summary>
    /// REASON_ARMEDFORCES_LOCALN
    /// </summary> 
    AFL = 32, 

    /// <summary>
    /// REASON_SERVICEDEPENDENT_FORCESN
    /// </summary> 
    SDN = 64, 

    /// <summary>
    /// REASON_SERVICEDEPENDENT_LOCALN
    /// </summary> 
    SDL = 128, 

    /// <summary>
     /// REASON_TEMPORARY_RESIDENT_NOT_RETURNED
    /// </summary> 
    TRA = 256,

    /// <summary>
    /// REASON_REMOVAL_FROM_RESIDENTIAL_INSTITUTE
    /// </summary> 
    RFI = 512, 

    /// <summary>
    /// REASON_PRACTICE_REQUEST_IMMEDIATE_REMOVAL
    /// </summary> 
    RDI = 1024, 

    /// <summary>
    /// REASON_PRACTICE_REQUEST
    /// </summary> 
    RDR = 2048,

    /// <summary>
    /// REASON_PATIENT_REQUEST
    /// </summary> 
    RPR = 4096,

    /// <summary>
    /// REASON_OUT_OF_PRACTICE_AREA
    /// </summary> 
    OPA = 8192,
    
    /// <summary>
    /// REASON_GONE_AWAY
    /// </summary> 
    CGA = 16384,
    
    /// <summary>
    /// REASON_CANCELLATION
    /// </summary> 
    CAN = 32768,

    /// <summary>
    /// REASON_OTHER
    /// </summary> 
    ORR = 65536,

    /// <summary>
    /// REASON_LOGICAL_DELETION
    /// </summary> 
    LDN = 131072,

    /// <summary>
    /// REASON_PRACTICE_DISSOLUTION
    /// </summary> 
    DIS = 262144,

    /// <summary>
    /// REASON_X
    /// </summary> 
    X = 524288
}


public enum DeathStatus
{
    /// <summary>
    /// No death status recorded.
    /// </summary>
    None = 0,

    /// <summary>
    /// Informal death status. Dealth certificate.
    /// </summary>
    INFORMAL = 1,

    /// <summary>
    /// Updated from registrars office.
    /// </summary>
    FORMAL = 2
}

public readonly record struct CommsCheckAnswerResponseDto(
    string ResultId, 
    CommsCheckQuestionRequestDto Request,
    CommsCheckAnswerDto Response);
    

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
    string PostalReason);

public enum CommAllowed
{
    Allowed,
    Blocked,
    Unknown
}



public record RuleOutcomesDto(params RuleResultSummary[] Summaries); 

public readonly record struct RuleResultSummary(
    string RuleSet,
    string Method,
    string MethodToLog,
    bool Enabled,
    bool Success, 
    string RuleName, 
    string RuleExpression,
    string SuccessEvent,
    string ErrorMessage,
    string ExceptionMessage);

    public readonly record struct CommsCheckItem(
    DateOnly UtcDateCheckItemCreated,
    DateOnly DateOfBirth,
    DateOnly DateOfSmsMostRecentUpdate,
    DateOnly DateOfEmailMostRecentUpdate,
    DateOnly DateOfAppMostRecentUpdate,
    DateOnly DateOfPostalMostRecentUpdate,
    DateOnly DateOfReasonForRemovalMostRecentUpdate,
    object ReasonForRemoval,
    object DeathStatus,
    CommsCheckQuestionRequestDtoCopy CopyOfSource);

    public readonly record struct CommsCheckQuestionRequestDtoCopy(DateOnly DateOfBirth,
    DateOnly DateOfSmsMostRecentUpdate,
    DateOnly DateOfEmailMostRecentUpdate,
    DateOnly DateOfAppMostRecentUpdate,
    DateOnly DateOfPostalMostRecentUpdate,
    DateOnly DateOfReasonForRemovalMostRecentUpdate,
    DeathStatus? DeathStatusValue,
    ReasonForRemovals? RfR
    );