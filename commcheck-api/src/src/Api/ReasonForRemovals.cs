namespace CommsCheck;

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


