using Sitecore.ContentSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreBlog.Models
{
    public class SearchModel
    {
        [IndexField("_name")]
        public virtual string ItemName { get; set; }


        [IndexField("title_t")]
        public virtual string Title { get; set; }

        public class SearchResult
        {
            public string ItemName { get; set; }
            public string Title { get; set; }
 
        }
        /// <summary>
        /// Custom search result model for binding to front end
        /// </summary>
        public class SearchResults
        {
            public List<SearchResult> Results { get; set; }
        }

    }
}