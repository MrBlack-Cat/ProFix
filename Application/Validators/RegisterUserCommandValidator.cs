﻿//using Application.CQRS.Users.Commands.Requests;
//using FluentValidation;

//namespace Application.Validators.Users;

//public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
//{
//    public RegisterUserCommandValidator()
//    {
//        RuleFor(x => x.UserName)
//            .NotEmpty().WithMessage("Username is required")
//            .MinimumLength(3).WithMessage("Username must be at least 3 characters long");

//        RuleFor(x => x.Email)
//            .NotEmpty().WithMessage("Email is required")
//            .EmailAddress().WithMessage("Invalid email format");

//        RuleFor(x => x.Password)
//            .NotEmpty().WithMessage("Password is required")
//            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

//        RuleFor(x => x.UserRoleId)
//            .GreaterThan(0).WithMessage("UserRoleId must be provided");
//    }
//}
