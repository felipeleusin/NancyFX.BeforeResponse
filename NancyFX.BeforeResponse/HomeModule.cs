namespace NancyFX.BeforeResponse
{
    using Nancy;

    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = async (p, c) => "Hi There! Post anything to /test";
        }
    }
}