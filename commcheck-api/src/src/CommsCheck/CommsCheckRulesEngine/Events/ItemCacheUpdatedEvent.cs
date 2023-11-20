
namespace CommsCheck;
using MediatR;

public class ItemCacheUpdatedEvent(CommsCheckAnswer answer) : INotification
{
    public CommsCheckAnswer Answer=> answer;
}
