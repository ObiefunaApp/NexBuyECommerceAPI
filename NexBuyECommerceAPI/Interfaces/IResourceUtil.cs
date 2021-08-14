using NexBuyECommerceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Interfaces
{
    public interface IResourceUtil
    {

        string CreatePaginationResourceUri(string actionName, ResourceParameters ResourceParameters, ResourceUriType type);
        IEnumerable<LinkDto> CreateLinksFoPaginations(string actionName, ResourceParameters ResourceParameters, bool hasNext, bool hasPrevious);
        IEnumerable<LinkDto> CreateLinksForContoller(IList<ControllerLink> ActionLinks);

    }
}
