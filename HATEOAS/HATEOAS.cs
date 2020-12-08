using System.Collections.Generic;

namespace Desafio_API.HATEOAS
{
    public class HATEOAS
    {
          public string url;
        public string protocol = "https://";
        public List<Link> actions = new List<Link>();

        public HATEOAS(string url)
        {
            this.url = url;
        }

        public HATEOAS(string url, string protocol)
        {
            this.url = url;
            this.protocol = protocol;
        }

        public void AddAction(string rel, string method)
        {
            actions.Add(new Link(this.protocol + this.url,rel,method));
        }

        public Link[] GetActions(string sufix)
        {
            Link[] tempLinks = actions.ToArray();
            foreach(var link in tempLinks)
            {
                link.href = link.href+"/"+sufix;
            }
            return tempLinks;
        }
    }
}