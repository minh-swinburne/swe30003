using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace frontend.Models
{
    // Base request model
    public class BaseRequest
    {
        [JsonPropertyName("createdTimeFrom")]
        public DateTime? CreatedTimeFrom { get; set; }

        [JsonPropertyName("createdTimeTo")]
        public DateTime? CreatedTimeTo { get; set; }

        [JsonPropertyName("updatedTimeFrom")]
        public DateTime? UpdatedTimeFrom { get; set; }

        [JsonPropertyName("updatedTimeTo")]
        public DateTime? UpdatedTimeTo { get; set; }
    }

    // Role DTO
    public class RoleDTO
    {
        [JsonPropertyName("roleId")]
        public Guid RoleId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    // Request DTOs

    public class CreateUserRequestDTO : BaseRequest
    {
        [JsonPropertyName("firstName")]
        [Required]
        public required string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [JsonPropertyName("phone")]
        [Required]
        public required string Phone { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("picture")]
        public string? Picture { get; set; }

        [JsonIgnore]
        public bool AcceptTerms { get; set; }
    }

    public class DeleteUserRequestDTO : BaseRequest
    {
        [JsonPropertyName("userId")]
        [Required]
        public required Guid UserId { get; init; }
    }

    public class GetUserByEmailRequestDTO : BaseRequest
    {
        [JsonPropertyName("email")]
        [Required]
        [EmailAddress]
        public required string Email { get; init; }
    }

    public class GetUserByIdRequestDTO : BaseRequest
    {
        [JsonPropertyName("userId")]
        [Required]
        public required Guid UserId { get; init; }
    }

    public class GetUserByPhoneRequestDTO : BaseRequest
    {
        [JsonPropertyName("phone")]
        [Required]
        public required string Phone { get; init; }
    }

    public class ListUserRequestDTO : BaseRequest
    {
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("roles")]
        public List<RoleDTO>? Roles { get; set; }

        [JsonPropertyName("matchAllRoles")]
        public bool MatchAllRoles { get; set; } = false;

        [JsonPropertyName("orderBy")]
        public string? OrderBy { get; set; }

        [JsonPropertyName("ascending")]
        public bool Ascending { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 10;

        [JsonPropertyName("pageNo")]
        public int PageNo { get; set; } = 1;
    }

    public class UpdateUserRequestDTO : BaseRequest
    {
        [JsonPropertyName("userId")]
        [Required]
        public Guid UserId { get; set; }

        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("picture")]
        public string? Picture { get; set; }
    }

    // Response DTOs

    public class BaseUserResponse
    {
        [JsonPropertyName("userId")]
        public required Guid UserId { get; set; }
    }

    public class CreateUserResponseDTO : BaseUserResponse
    {
        [JsonPropertyName("firstName")]
        public required string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("phone")]
        public required string Phone { get; set; }

        [JsonPropertyName("picture")]
        public string? Picture { get; set; }

        [JsonPropertyName("roles")]
        public List<RoleDTO> Roles { get; set; } = [];
    }

    public class DeleteUserResponseDTO : BaseUserResponse
    {
        [JsonPropertyName("success")]
        public required bool Success { get; set; }
    }

    public class GetUserResponseDTO : BaseUserResponse
    {
        [JsonPropertyName("firstName")]
        public required string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("phone")]
        public required string Phone { get; set; }

        [JsonPropertyName("picture")]
        public string? Picture { get; set; }

        [JsonPropertyName("roles")]
        public required List<RoleDTO> Roles { get; set; }
    }

    public class ListUserResponseDTO : BaseUserResponse
    {
        [JsonPropertyName("firstName")]
        public required string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("phone")]
        public required string Phone { get; set; }

        [JsonPropertyName("picture")]
        public string? Picture { get; set; }

        [JsonPropertyName("roles")]
        public required List<RoleDTO> Roles { get; set; }
    }

    public class UpdateUserResponseDTO : BaseUserResponse
    {
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("picture")]
        public string? Picture { get; set; }
    }

    // API Response wrapper
    public class ApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, List<string>>? Errors { get; set; }
    }

    // Pagination response
    public class PaginatedResponse<T>
    {
        [JsonPropertyName("items")]
        public List<T> Items { get; set; } = new List<T>();

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("pageNo")]
        public int PageNo { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }
    }
}

