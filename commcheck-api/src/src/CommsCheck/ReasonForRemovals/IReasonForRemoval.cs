using System.Text.Json.Serialization;

namespace CommsCheck;

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
[JsonDerivedType(typeof(Death), typeDiscriminator: "Death")]
[JsonDerivedType(typeof(NoReasonForRemoval), typeDiscriminator: "NoReasonForRemoval")]
[JsonDerivedType(typeof(Embarkation), typeDiscriminator: "Embarkation")]
[JsonDerivedType(typeof(TransferedToScotland), typeDiscriminator: "TransferedToScotland")]
[JsonDerivedType(typeof(TransferedToNorthernIreland), typeDiscriminator: "TransferedToNorthernIreland")]
[JsonDerivedType(typeof(ArmedForces), typeDiscriminator: "ArmedForces")]
[JsonDerivedType(typeof(ArmedForcesLocal), typeDiscriminator: "ArmedForcesLocal")]
[JsonDerivedType(typeof(ServiceDependent), typeDiscriminator: "ServiceDependent")]
[JsonDerivedType(typeof(ServiceDependentLocal), typeDiscriminator: "ServiceDependentLocal")]
[JsonDerivedType(typeof(TemporaryResidentNotReturned), typeDiscriminator: "TemporaryResidentNotReturned")]
[JsonDerivedType(typeof(FromResidentialInsitute), typeDiscriminator: "FromResidentialInsitute")]
[JsonDerivedType(typeof(PracticeRequestImmediateRemoval), typeDiscriminator: "PracticeRequestImmediateRemoval")]
[JsonDerivedType(typeof(PracticeRequest), typeDiscriminator: "PracticeRequest")]
[JsonDerivedType(typeof(PatientRequest), typeDiscriminator: "PatientRequest")]
[JsonDerivedType(typeof(OutOfPracticeArea), typeDiscriminator: "OutOfPracticeArea")]
[JsonDerivedType(typeof(GoneAway), typeDiscriminator: "GoneAway")]
[JsonDerivedType(typeof(Other), typeDiscriminator: "Other")]
[JsonDerivedType(typeof(LogicalDeletion), typeDiscriminator: "LogicalDeletion")]
[JsonDerivedType(typeof(PractiseDissolution), typeDiscriminator: "PractiseDissolution")]
[JsonDerivedType(typeof(X), typeDiscriminator: "X")]
public interface IReasonForRemoval
{
   string Code { get; }
   public static Death Death => new Death();
   public static NoReasonForRemoval None => new NoReasonForRemoval();
   public static Embarkation Embarkation => new Embarkation();
   public static TransferedToScotland TransferedToScotland => new TransferedToScotland();
   public static TransferedToNorthernIreland TransferedToNorthernIreland => new TransferedToNorthernIreland();
   public static ArmedForces ArmedForces => new ArmedForces();
   public static ArmedForcesLocal ArmedForcesLocal => new ArmedForcesLocal();
   public static ServiceDependent ServiceDependent => new ServiceDependent();
   public static ServiceDependentLocal ServiceDependentLocal => new ServiceDependentLocal();
   public static TemporaryResidentNotReturned TemporaryResidentNotReturned => new TemporaryResidentNotReturned();
   public static FromResidentialInsitute FromResidentialInsitute => new FromResidentialInsitute();
   public static PracticeRequestImmediateRemoval PracticeRequestImmediateRemoval => new PracticeRequestImmediateRemoval();
   public static PracticeRequest PracticeRequest => new PracticeRequest();
   public static PatientRequest PatientRequest => new PatientRequest();
   public static OutOfPracticeArea OutOfPracticeArea => new OutOfPracticeArea();
   public static GoneAway GoneAway => new GoneAway();
   public static Cancellation Cancellation => new Cancellation();
   public static Other Other => new Other();
   public static LogicalDeletion LogincalDeletion => new LogicalDeletion();
   public static PractiseDissolution PractiseDissolution => new PractiseDissolution();
   public static X X => new X();
   bool IsEmpty { get; }

   bool NotSet { get; }

   public bool HasCode { get; }

   public static IReasonForRemoval FromEnum(ReasonForRemovals? reasonForRemoval) =>
    reasonForRemoval switch
    {
       null => IReasonForRemoval.None,
       ReasonForRemovals.None => IReasonForRemoval.None,
       ReasonForRemovals.AFL => IReasonForRemoval.ArmedForcesLocal,
       ReasonForRemovals.AFN => IReasonForRemoval.ArmedForces,
       ReasonForRemovals.CAN => IReasonForRemoval.Cancellation,
       ReasonForRemovals.CGA => IReasonForRemoval.GoneAway,
       ReasonForRemovals.DEA => IReasonForRemoval.Death,
       ReasonForRemovals.DIS => IReasonForRemoval.PractiseDissolution,
       ReasonForRemovals.EMB => IReasonForRemoval.Embarkation,
       ReasonForRemovals.LDN => IReasonForRemoval.LogincalDeletion,
       ReasonForRemovals.NIT => IReasonForRemoval.TransferedToNorthernIreland,
       ReasonForRemovals.OPA => IReasonForRemoval.OutOfPracticeArea,
       ReasonForRemovals.ORR => IReasonForRemoval.Other,
       ReasonForRemovals.RDI => IReasonForRemoval.PracticeRequestImmediateRemoval,
       ReasonForRemovals.RDR => IReasonForRemoval.PracticeRequest,
       ReasonForRemovals.RFI => IReasonForRemoval.FromResidentialInsitute,
       ReasonForRemovals.RPR => IReasonForRemoval.PatientRequest,
       ReasonForRemovals.SCT => IReasonForRemoval.TransferedToScotland,
       ReasonForRemovals.SDN => IReasonForRemoval.ServiceDependent,
       ReasonForRemovals.SDL => IReasonForRemoval.ServiceDependentLocal,
       ReasonForRemovals.TRA => IReasonForRemoval.TemporaryResidentNotReturned,
       ReasonForRemovals.X => IReasonForRemoval.X,
       _ => throw new ArgumentOutOfRangeException(nameof(reasonForRemoval))
    };
}
