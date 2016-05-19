namespace NancyFX.BeforeResponse
{
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}