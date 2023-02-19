namespace IdentityPortal.EntityFramework.Options;

public class TableConfiguration
{
    public TableConfiguration(string name)
    {
        Name = name;
    }

    public TableConfiguration(string name, string schema)
    {
        Name = name;
        Schema = schema;
    }

    /// <summary>
    /// Gets or sets the table name
    /// </summary>
    /// <value>The table name</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the schema name
    /// </summary>
    /// <value>The schema name</value>
    public string? Schema { get; set; }
}
