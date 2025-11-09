using CampusNet.Controllers;
using CampusNet.Models;

namespace CampusNet.ConsoleApp
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var graph = new Graph();
            var controller = new CampusController(graph);
            controller.Run();
        }
    }
}
