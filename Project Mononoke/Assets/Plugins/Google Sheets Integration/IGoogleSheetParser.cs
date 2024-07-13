using System.Threading.Tasks;

namespace Plugins.GoogleSheetsIntegration
{
    public interface IGoogleSheetParser
    {
        public Task Parse(string header, string token);
    }
}