﻿using Application.Common.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services;

public  class PasswordHasher : IPasswordHasher
{
    public  string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public  bool VerifyPassword(string hashedPassword, string password)
    {
        var hashOfInput = HashPassword(password);
        return hashedPassword == hashOfInput;
    }
}
