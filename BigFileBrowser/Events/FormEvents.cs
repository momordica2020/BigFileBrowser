using BigFileBrowser.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigFileBrowser.Events
{
    public delegate void sendStringEvent(string str);

    public delegate void sendSearchResultEvent(SearchResult items);

    public delegate void sendStringArrayEvent(string[] str);
}
