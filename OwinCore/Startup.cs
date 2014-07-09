namespace OwinCore
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Microsoft.Owin.FileSystems;
    using Microsoft.Owin.StaticFiles;
    using Nancy;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var sds = GetFullRoot(Path.Combine(AssemblyDirectory, "static"));
            
            //app.Run(context => context.Response.WriteAsync("YO"));
            app
                .UseStaticFiles(new StaticFileOptions { FileSystem = new PhysicalFileSystem(Path.Combine(AssemblyDirectory, "static")), RequestPath = new PathString("/static") })
                .Use(MyMiddleware.DoIt())
                .UseNancy()
                ;
        }

        private static string GetFullRoot(string root)
        {
            var applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var fullRoot = Path.GetFullPath(Path.Combine(applicationBase, root));
            if (!fullRoot.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
            {
                // When we do matches in GetFullPath, we want to only match full directory names.
                fullRoot += Path.DirectorySeparatorChar;
            }
            return fullRoot;
        }

        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }

    public static class MyMiddleware
    {
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> DoIt()
        {
            return next => async env =>
            {
                Debug.WriteLine("incoming");
                await next(env);
                Debug.WriteLine("outgoing");
            };
        }
    }

    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => "Hi there"; 
        }
    }
}