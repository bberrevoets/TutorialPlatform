using TutorialPlatform.Models;

namespace TutorialPlatform.Areas.Identity.Data;

public static class Seeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Tutorials.Any())
            return;

        var tutorial = new Tutorial
        {
            Title = "Getting Started with ROS2",
            Description = "Learn the basics of ROS2 using a beginner-friendly tutorial.",
            Chapters = new List<Chapter>
            {
                new()
                {
                    Title = "Introduction to ROS2",
                    Order = 1,
                    HtmlContent =
                        "<h2>Welcome to ROS2</h2><p>ROS2 is a flexible framework for writing robot software. It builds on ROS1 but introduces major improvements in performance and architecture.</p>"
                },
                new()
                {
                    Title = "Installing ROS2",
                    Order = 2,
                    HtmlContent =
                        "<h2>Installing ROS2</h2><p>Use Ubuntu 22.04 and run the following commands:</p><pre><code>sudo apt update<br/>sudo apt install ros-humble-desktop</code></pre>"
                },
                new()
                {
                    Title = "Writing Your First Node",
                    Order = 3,
                    HtmlContent =
                        "<h2>Your First Node</h2><p>Let’s create a simple publisher node in Python or C++ that publishes to a topic.</p>"
                }
            }
        };

        context.Tutorials.Add(tutorial);
        context.SaveChanges();
    }
}