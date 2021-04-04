using System.Collections.Generic;

namespace TransactionAPI.Models.JsonResponses
{
    /// <summary>
    /// Specifies the response template for a user request
    /// </summary>
    public class ResponseJson
    {
        public int Code { get; set; }
        public Description Description { get; set; }
    }
    public class Description 
    {
        public int Error { get; set; }
        public List<Details> Details { get; set;}
        public string Content { get; set; }
    }

    public class Details
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
