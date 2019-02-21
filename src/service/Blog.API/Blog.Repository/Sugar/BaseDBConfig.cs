using System.IO;
using Blog.Common.Secrets;

namespace Blog.Repository.Sugar
{
    public class BaseDBConfig
    {
        //public static string ConnectionString = "server=.;uid=sa;pwd=.;database=siegrain.blog";
        public static string ConnectionString = KeyVault.ConnectionString;
    }
}
