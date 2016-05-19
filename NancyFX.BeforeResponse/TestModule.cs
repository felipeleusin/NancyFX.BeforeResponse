namespace NancyFX.BeforeResponse
{
    using FluentValidation;

    public class TestRequest
    {
        public string Name { get; set; }

        public class Validator : AbstractValidator<TestRequest>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }
        }
    }

    public class TestModule : ValidatedModule<TestRequest>
    {
        public TestModule()
        {
            Post["/test"] = async (p, c) => $"Name is ${Input.Name}!";
        }
    }
}