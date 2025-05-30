﻿namespace SmartRide.Common.Responses.Errors;

public static class UserErrors
{
    public static readonly string Module = "User";

    public static readonly ResponseInfo ID_EMPTY = new()
    {
        Code = $"{Module}.ID_EMPTY",
        Message = "User ID can not be empty."
    };

    public static readonly ResponseInfo ID_INVALID = new()
    {
        Code = $"{Module}.ID_INVALID",
        Message = "User ID must be a valid GUID."
    };

    public static readonly ResponseInfo ID_NOT_FOUND = new()
    {
        Code = $"{Module}.ID_NOT_FOUND",
        Message = "User with ID {UserId} not found."
    };

    public static readonly ResponseInfo FIRSTNAME_EMPTY = new()
    {
        Code = $"{Module}.FIRSTNAME_EMPTY",
        Message = "Firstname can not be empty."
    };

    public static readonly ResponseInfo FIRSTNAME_TOO_LONG = new()
    {
        Code = $"{Module}.FIRSTNAME_TOO_LONG",
        Message = "Firstname can not be longer than 50 characters."
    };

    public static readonly ResponseInfo FIRSTNAME_INVALID = new()
    {
        Code = $"{Module}.FIRSTNAME_INVALID",
        Message = "Firstname can only contain alphanumeric characters or whitespaces and must start with an uppercase letter."
    };

    public static readonly ResponseInfo LASTNAME_TOO_LONG = new()
    {
        Code = $"{Module}.LASTNAME_TOO_LONG",
        Message = "Lastname can not be longer than 50 characters."
    };

    public static readonly ResponseInfo LASTNAME_INVALID = new()
    {
        Code = $"{Module}.LASTNAME_INVALID",
        Message = "Lastname can only contain alphanumeric characters or whitespaces and must start with an uppercase letter."
    };

    public static readonly ResponseInfo EMAIL_EMPTY = new()
    {
        Code = $"{Module}.EMAIL_EMPTY",
        Message = "Email can not be empty."
    };

    public static readonly ResponseInfo EMAIL_TOO_LONG = new()
    {
        Code = $"{Module}.EMAIL_TOO_LONG",
        Message = "Email can not be longer than 255 characters."
    };

    public static readonly ResponseInfo EMAIL_INVALID = new()
    {
        Code = $"{Module}.EMAIL_INVALID",
        Message = "Email must be a valid email address."
    };

    public static readonly ResponseInfo EMAIL_NOT_FOUND = new()
    {
        Code = $"{Module}.EMAIL_NOT_FOUND",
        Message = "Email {Email} not found."
    };

    public static readonly ResponseInfo EMAIL_ALREADY_EXISTS = new()
    {
        Code = $"{Module}.EMAIL_ALREADY_EXISTS",
        Message = "Email {Email} has already been used."
    };

    public static readonly ResponseInfo PHONE_EMPTY = new()
    {
        Code = $"{Module}.PHONE_EMPTY",
        Message = "Phone number can not be empty."
    };

    public static readonly ResponseInfo PHONE_TOO_LONG = new()
    {
        Code = $"{Module}.PHONE_TOO_LONG",
        Message = "Phone number can not be longer than 45 characters."
    };

    public static readonly ResponseInfo PHONE_INVALID = new()
    {
        Code = $"{Module}.PHONE_INVALID",
        Message = "Phone number must be a valid E.164 phone number."
    };

    public static readonly ResponseInfo PHONE_NOT_FOUND = new()
    {
        Code = $"{Module}.PHONE_NOT_FOUND",
        Message = "Phone number {Phone} not found."
    };

    public static readonly ResponseInfo PHONE_ALREADY_EXISTS = new()
    {
        Code = $"{Module}.PHONE_ALREADY_EXISTS",
        Message = "Phone number {Phone} has already been used."
    };

    public static readonly ResponseInfo PASSWORD_TOO_SHORT = new()
    {
        Code = $"{Module}.PASSWORD_TOO_SHORT",
        Message = "Password must be at least 8 characters long."
    };

    public static readonly ResponseInfo PASSWORD_TOO_LONG = new()
    {
        Code = $"{Module}.PASSWORD_TOO_LONG",
        Message = "Password can not be longer than 150 characters."
    };

    public static readonly ResponseInfo PASSWORD_INVALID = new()
    {
        Code = $"{Module}.PASSWORD_INVALID",
        Message = "Password must include at least one letter, one number, and one special character."
    };
}
