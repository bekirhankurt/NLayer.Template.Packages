﻿using System.Security.Cryptography;

namespace Security.EmailAuthenticator;

public class EmailAuthenticatorHelper : IEmailAuthenticatorHelper
{
    public Task<string> CreateEmailActivationKey()
    {
        var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        return Task.FromResult(key);
    }

    public Task<string> CreateEmailActivationCode()
    {
        var code = RandomNumberGenerator.GetInt32(Convert.ToInt32(Math.Pow(10, 6))).ToString().PadLeft(6, '0');
        return Task.FromResult(code);
    }
}