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
            
            //app.Run(context => context.Response.WriteAsync("YO"));
            app
                .UseStaticFiles(new StaticFileOptions { FileSystem = new PhysicalFileSystem(Path.Combine(AssemblyDirectory, "static")), RequestPath = new PathString("/static") })
                .Use(MyMiddleware.DoIt())
                .UseNancy()
                ;
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