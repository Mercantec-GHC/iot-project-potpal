using Microsoft.VisualBasic;

public class Config
{
    public static string SMTP_ADDRESS => ParseVariable("EmailSettings", "SMTP_ADDRESS");
    public static string SMTP_PORT => ParseVariable("EmailSettings", "SMTP_PORT");
    public static string SMTP_USER => ParseVariable("EmailSettings", "SMTP_USER");
    public static string SMTP_PASSWORD => ParseVariable("EmailSettings", "SMTP_PASSWORD");
    public static string EMAIL_TEMPLATES_PATH => ParseVariable("EmailSettings", "EMAIL_TEMPLATES_PATH");

    private static Config? _instance = null;
    private static WebApplicationBuilder? _app;

    private Config(WebApplicationBuilder app)
    {
        _app = app;
    }

    public static void Init(WebApplicationBuilder app)
    {
        if (_instance == null)
        {
            _instance = new Config(app);
        }
    }


    private static string ParseVariable(string sectionName, string key)
    {
        if (_app == null)
        {
            throw new Exception("Config is not initialized");
        }

        var section = _app.Configuration.GetSection(sectionName);
        
        if (!section.Exists())
        {
            string errorMsg = $"Section {sectionName} does not exist";
            throw new Exception(errorMsg);
        }

        string variable = section[key] ?? "";



        if (string.IsNullOrEmpty(variable))
        {
            string errorMsg = $"In section {sectionName} key {key} is not set";
            throw new Exception(errorMsg);
        }
        return variable;
    }

}