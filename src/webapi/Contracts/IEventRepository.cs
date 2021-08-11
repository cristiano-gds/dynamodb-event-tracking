using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Runtime.EventStreams.Internal;
using webapi.Model;
using webapi.Domain;

namespace webapi.Contracts
{
    public interface IEventRepository
    {
         Task<string> Add(EventModel entity);

         Task Update(Event entity);

         Task<Event> Single(string Id);

         Task<List<Event>> FindByExternalCode(string externalCode);
    }
}