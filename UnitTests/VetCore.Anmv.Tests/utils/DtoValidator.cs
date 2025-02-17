using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace VetCore.Anmv.Tests.utils;

// validate some specific type that mess around recursivity
public class DtoValidatorTests
{
    [Fact]
    public void Datetime_recursive_validation_ok()
    {
        //Arrange
        // A Date could be recursively validated indefinitely
        var date = DateTimeOffset.UtcNow;
        date = date.Date.Date.Date.Date.Date.Date.Date.Date.Date.Date.Date; // etc...
        var dto = new MiscUnitTestDtoDate()
        {
            Date = date,
        };

        //Act
        var res = dto.IsDtoValidAccordingToAttributes();

        //Assert
        Assert.True(res);
    }

    [Fact]
    public void MiscUnitTestDtoValidate_custom_validation_success()
    {
        //Arrange
        var dto = new MiscUnitTestDtoValidate
        {
            InvalidIfPair = 23,
        };

        //Act
        var res = dto.IsDtoValidAccordingToAttributes();

        //Assert
        Assert.True(res);
    }

    [Fact]
    public void MiscUnitTestDtoValidate_custom_validation_failed()
    {
        //Arrange
        var dto = new MiscUnitTestDtoValidate
        {
            InvalidIfPair = 22, // pair not allowed
        };

        //Act
        var res = dto.IsDtoValidAccordingToAttributes();

        //Assert
        Assert.False(res);
    }
}

public sealed record MiscUnitTestDtoDate
{
    [JsonPropertyName("date")] public required DateTimeOffset Date { get; init; }
}

public sealed record MiscUnitTestDtoValidate : IValidatableObject
{
    [JsonPropertyName("value")] public required int InvalidIfPair { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (InvalidIfPair % 2 == 0)
        {
            yield return new ValidationResult("Prop 'InvalidIfPair' is pair... It is not allowed", [nameof(InvalidIfPair)]);
        }
    }
}

/// <summary>
/// Helpers for validating attributes on DTO
/// </summary>
public static class DtoValidator
{
    /// <summary>
    /// Return true if the dto is valid according to annotation attributes, false otherwise
    /// </summary>
    public static bool IsDtoValidAccordingToAttributes<T>(this T dto)
    {
        if (dto == null)
            return false;

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(dto);
        var processedObjects = new HashSet<ValidationKey>();

        // Step 1: Validation of the main object using annotation attributes
        if (!Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true))
        {
            return false;
        }

        // Step 2: Validation of IValidatableObject if implemented
        if (dto is IValidatableObject validatable)
        {
            var results = validatable.Validate(validationContext);
            if (results.Any())
            {
                return false;
            }
        }

        // Step 3: Validation of complex properties recursively
        return dto.GetType()
            .GetPropertiesToValidate()
            .All(o => ValidateComplexPropertyRecursive(o, dto, processedObjects));
    }

    private static PropertyInfo[] GetPropertiesToValidate(this Type type)
    {
        return type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && !p.PropertyType.IsPrimitive && p.PropertyType != typeof(string))
            .ToArray();
    }

    private static bool ValidateComplexPropertyRecursive(PropertyInfo propInfo, object parent, HashSet<ValidationKey> processedObjects)
    {
        var currentValue = propInfo.GetValue(parent);
        if (currentValue == null)
            return true;

        // Create a validation key
        var validationKey = new ValidationKey(
            propInfo.Name,
            propInfo.PropertyType,
            parent.GetType()
        );

        // If the object has already been processed, stop recursion
        if (!processedObjects.Add(validationKey))
            return true;

        if (currentValue is IDictionary dictionary)
        {
            // Recursive validation on the properties of dictionary values
            foreach (var value in dictionary.Values)
            {
                if (value == null)
                    continue;

                // Apply validation on each field
                return ReturnValidateProvidedComplexObject(processedObjects, value, value.GetType());
            }

            // Empty dictionary => valid
            return true;
        }

        // Recursive validation of complex properties
        return ReturnValidateProvidedComplexObject(processedObjects, currentValue, currentValue.GetType());
    }

    private static bool ReturnValidateProvidedComplexObject(HashSet<ValidationKey> processedObjects, object currentValue, Type typeToValidate)
    {
        var validationContext = new ValidationContext(currentValue);
        var validationResults = new List<ValidationResult>();

        // Step 1: Validate annotation attributes
        if (!Validator.TryValidateObject(currentValue, validationContext, validationResults, validateAllProperties: true))
        {
            return false;
        }

        // Step 2: Validate IValidatableObject if implemented
        if (currentValue is IValidatableObject validatable)
        {
            var results = validatable.Validate(validationContext);
            if (results.Any())
            {
                return false;
            }
        }

        // Step 3: Recursively validate nested properties
        return typeToValidate
            .GetPropertiesToValidate()
            .All(p => ValidateComplexPropertyRecursive(
                propInfo: p,
                parent: currentValue,
                processedObjects: processedObjects));
    }

    private record ValidationKey(string PropertyName, Type PropertyType, Type ParentType)
    {
        public override int GetHashCode()
        {
            return HashCode.Combine(
                PropertyName.GetHashCode(),
                PropertyType.GetHashCode(),
                ParentType.GetHashCode()
            );
        }
    }
}