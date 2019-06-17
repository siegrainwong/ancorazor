using System;
using System.Collections.Generic;
using System.Text;

namespace Ancorazor.API.Messages.Settings
{
    public class GitmentViewModel
    {
        public string GithubId { get; set; }
        public string RepositoryName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
