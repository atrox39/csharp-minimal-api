using EjerciciosProgramacion.Models;
using FluentValidation;


namespace EjerciciosProgramacion.Dto
{
  public class RegisterDto : AbstractValidator<User>
  {
    public RegisterDto()
    {
      RuleFor(x => x.name).NotEmpty().NotNull();
      RuleFor(x => x.email).NotEmpty().NotNull().EmailAddress();
      RuleFor(x => x.password).NotEmpty().NotNull();
    }
  }
}
