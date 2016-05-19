namespace NancyFX.BeforeResponse
{
    using Nancy;
    using Nancy.ModelBinding;

    public abstract class ValidatedModule<T> : NancyModule, IValidateInput<T>
    {
        public T Input { get; set; }

        protected ValidatedModule()
        {
            this.ValidateInput();
        }
    }

    public interface IValidateInput<T> : INancyModule
    {
        T Input { get; set; }
    }

    public static class ValidateInputExtensions
    {
        public static void ValidateInput<T>(this IValidateInput<T> module)
        {
            module.Before.AddItemToStartOfPipeline(ctx =>
            {
                module.Input = module.BindAndValidate<T>();
                if (!module.ModelValidationResult.IsValid)
                {
                    return new ErrorResponse(module.ModelValidationResult.ToError(), ctx.Environment);
                }

                return null;
            });
        }
    }
}