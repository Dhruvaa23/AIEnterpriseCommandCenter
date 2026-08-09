namespace AIEnterpriseCommandCenter.Web.Helpers
{
    public static class TimeAgoHelper
    {
        public static string GetTimeAgo(DateTime dateTime)
        {
            var span = DateTime.Now - dateTime;

            if (span.TotalSeconds < 60)
                return "Just now";

            if (span.TotalMinutes < 60)
                return $"{(int)span.TotalMinutes} min ago";

            if (span.TotalHours < 24)
                return $"{(int)span.TotalHours} hr ago";

            if (span.TotalDays < 7)
                return $"{(int)span.TotalDays} day{((int)span.TotalDays > 1 ? "s" : "")} ago";

            if (span.TotalDays < 30)
                return $"{(int)(span.TotalDays / 7)} week{((int)(span.TotalDays / 7) > 1 ? "s" : "")} ago";

            if (span.TotalDays < 365)
                return $"{(int)(span.TotalDays / 30)} month{((int)(span.TotalDays / 30) > 1 ? "s" : "")} ago";

            return $"{(int)(span.TotalDays / 365)} year{((int)(span.TotalDays / 365) > 1 ? "s" : "")} ago";
        }
    }
}
