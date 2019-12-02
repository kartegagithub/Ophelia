namespace Ophelia.Web.View.Mvc.Models
{
    public interface IAuditable
    {
        AuditModel History { get; set; }
    }
}
