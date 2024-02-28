namespace NIHR.StudyManagement.Api.Configuration
{
    public class StudyManagementApiSettings
    {
        public JwtBearerSettings JwtBearer { get; set; }
        public DatabaseSettings Data { get; set; }

        public StudyManagementApiSettings()
        {
            JwtBearer = new JwtBearerSettings();
            Data = new DatabaseSettings();
        }
    }
}
