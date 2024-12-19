namespace VetCore.Anmv.Utils.Validations;

/// <summary>
/// Group all validation error
/// </summary>
public sealed class ValidationErrors
{
    private readonly List<string> _errors = [];
    public int Count => _errors.Count;

    public void Add(string message)
    {
        _errors.Add(message);
    }

    public IReadOnlyList<string> GetErrors() => _errors.ToArray();

    public string PrintErrors(string separator)
    {
        return string.Join(separator, _errors);
    }
}
