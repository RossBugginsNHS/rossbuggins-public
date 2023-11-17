namespace CommsCheck;

using MediatR;

public interface ICommsCheckEvent : INotification
{
    Guid CommCheckCorrelationId{get;}
}