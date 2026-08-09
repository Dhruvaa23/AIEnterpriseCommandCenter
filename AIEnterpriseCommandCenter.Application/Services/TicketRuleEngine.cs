

using AIEnterpriseCommandCenter.Application.DTOs.AI;

namespace AIEnterpriseCommandCenter.Application.Services;

public static class TicketRuleEngine
{
    public static TicketAnalysisDto? Analyze(string issue)
    {
        issue = issue.ToLower();

        //----------------------------------
        // Blue Screen
        //----------------------------------

        if (issue.Contains("blue screen") ||
            issue.Contains("bsod"))
        {
            return new TicketAnalysisDto
            {
                Title = "Blue Screen Error",
                Category = "Hardware",
                Priority = "High",
                SuggestedSolution = "Boot into Safe Mode and uninstall the latest Windows update.",
                Confidence = 99
            };
        }

        //----------------------------------
        // Password
        //----------------------------------

        if (issue.Contains("forgot password") ||
            issue.Contains("password") ||
            issue.Contains("reset password"))
        {
            return new TicketAnalysisDto
            {
                Title = "Password Reset Request",
                Category = "Account",
                Priority = "Low",
                SuggestedSolution = "Reset your password using the self-service portal or contact the administrator.",
                Confidence = 99
            };
        }

        //----------------------------------
        // Login
        //----------------------------------

        if (issue.Contains("login") ||
            issue.Contains("sign in"))
        {
            return new TicketAnalysisDto
            {
                Title = "User Login Issue",
                Category = "Account",
                Priority = "Medium",
                SuggestedSolution = "Verify your username and password. Unlock the account if necessary.",
                Confidence = 98
            };
        }

        //----------------------------------
        // Printer
        //----------------------------------

        if (issue.Contains("printer"))
        {
            return new TicketAnalysisDto
            {
                Title = "Printer Not Working",
                Category = "Hardware",
                Priority = "Medium",
                SuggestedSolution = "Check printer connection, restart the printer and reinstall drivers.",
                Confidence = 98
            };
        }

        //----------------------------------
        // WiFi / Internet
        //----------------------------------

        if (issue.Contains("wifi") ||
            issue.Contains("internet") ||
            issue.Contains("network"))
        {
            return new TicketAnalysisDto
            {
                Title = "Network Connectivity Issue",
                Category = "Network",
                Priority = "High",
                SuggestedSolution = "Restart the router, verify network connectivity and check DNS settings.",
                Confidence = 97
            };
        }

        //----------------------------------
        // VPN
        //----------------------------------

        if (issue.Contains("vpn"))
        {
            return new TicketAnalysisDto
            {
                Title = "VPN Connection Failed",
                Category = "Network",
                Priority = "High",
                SuggestedSolution = "Verify VPN credentials, internet connection and reconnect.",
                Confidence = 98
            };
        }

        //----------------------------------
        // Email / Outlook
        //----------------------------------

        if (issue.Contains("email") ||
            issue.Contains("outlook"))
        {
            return new TicketAnalysisDto
            {
                Title = "Email Service Issue",
                Category = "Software",
                Priority = "Medium",
                SuggestedSolution = "Restart Outlook, verify mailbox connectivity and recreate the profile if required.",
                Confidence = 97
            };
        }

        //----------------------------------
        // Laptop / Computer
        //----------------------------------

        if (issue.Contains("laptop") ||
            issue.Contains("computer"))
        {
            return new TicketAnalysisDto
            {
                Title = "Computer Hardware Issue",
                Category = "Hardware",
                Priority = "Medium",
                SuggestedSolution = "Restart the computer and check all hardware connections.",
                Confidence = 96
            };
        }

        //----------------------------------
        // Slow / Freeze
        //----------------------------------

        if (issue.Contains("slow") ||
            issue.Contains("freeze") ||
            issue.Contains("hang"))
        {
            return new TicketAnalysisDto
            {
                Title = "System Performance Issue",
                Category = "Software",
                Priority = "Medium",
                SuggestedSolution = "Close unnecessary applications, restart the system and check disk usage.",
                Confidence = 96
            };
        }

        //----------------------------------
        // Application Crash
        //----------------------------------

        if (issue.Contains("crash") ||
            issue.Contains("application"))
        {
            return new TicketAnalysisDto
            {
                Title = "Application Crash",
                Category = "Software",
                Priority = "High",
                SuggestedSolution = "Restart or reinstall the application and check for updates.",
                Confidence = 97
            };
        }

        //----------------------------------
        // Keyboard / Mouse
        //----------------------------------

        if (issue.Contains("keyboard") ||
            issue.Contains("mouse"))
        {
            return new TicketAnalysisDto
            {
                Title = "Input Device Issue",
                Category = "Hardware",
                Priority = "Low",
                SuggestedSolution = "Reconnect the device and update the drivers.",
                Confidence = 98
            };
        }

        //----------------------------------
        // Monitor
        //----------------------------------

        if (issue.Contains("monitor") ||
            issue.Contains("display"))
        {
            return new TicketAnalysisDto
            {
                Title = "Display Issue",
                Category = "Hardware",
                Priority = "Medium",
                SuggestedSolution = "Check the monitor cable, power supply and display settings.",
                Confidence = 97
            };
        }

        return null;
    }
}