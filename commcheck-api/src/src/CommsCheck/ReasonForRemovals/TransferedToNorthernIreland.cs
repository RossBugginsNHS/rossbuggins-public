
namespace CommsCheck;

public readonly record struct TransferedToNorthernIreland : IReasonForRemoval
{
    public string Code => "NIT";

    public bool NotSet => false;
    public bool HasCode => true;
    public bool IsEmpty => NotSet;
}


