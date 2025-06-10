using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace QOC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly string arabicFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "i18n", "ar.json");
        private readonly string englishFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "i18n", "en.json");

        [HttpPost("update")]
        public async Task<IActionResult> UpdateTranslation([FromBody] TranslationUpdateRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Key))
                return BadRequest("Invalid data");

            // اقرأ الملفات
            var arabicJson = await System.IO.File.ReadAllTextAsync(arabicFilePath);
            var englishJson = await System.IO.File.ReadAllTextAsync(englishFilePath);

            var arabicData = JsonSerializer.Deserialize<Dictionary<string, object>>(arabicJson);
            var englishData = JsonSerializer.Deserialize<Dictionary<string, object>>(englishJson);

            if (arabicData == null || englishData == null)
                return StatusCode(500, "Failed to load translation files");

            // عدل القيمة
            UpdateNestedValue(arabicData, request.Key, request.ArabicTranslation);
            UpdateNestedValue(englishData, request.Key, request.EnglishTranslation);

            // اكتب الملفات من جديد
            await System.IO.File.WriteAllTextAsync(arabicFilePath, JsonSerializer.Serialize(arabicData, new JsonSerializerOptions { WriteIndented = true }));
            await System.IO.File.WriteAllTextAsync(englishFilePath, JsonSerializer.Serialize(englishData, new JsonSerializerOptions { WriteIndented = true }));

            return Ok(new { message = "Translation updated successfully" });
        }

        [HttpGet("translations")]
        public async Task<IActionResult> GetTranslations()
        {
            try
            {
                // اقرأ الملفات
                var arabicJson = await System.IO.File.ReadAllTextAsync(arabicFilePath);
                var englishJson = await System.IO.File.ReadAllTextAsync(englishFilePath);

                var translations = new
                {
                    Arabic = JsonSerializer.Deserialize<Dictionary<string, object>>(arabicJson),
                    English = JsonSerializer.Deserialize<Dictionary<string, object>>(englishJson)
                };

                return Ok(translations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reading translation files: {ex.Message}");
            }
        }

        private void UpdateNestedValue(Dictionary<string, object> data, string path, string newValue)
        {
            var keys = path.Split('.');
            Dictionary<string, object> current = data;

            for (int i = 0; i < keys.Length; i++)
            {
                if (i == keys.Length - 1)
                {
                    current[keys[i]] = newValue;
                }
                else
                {
                    if (current[keys[i]] is JsonElement element && element.ValueKind == JsonValueKind.Object)
                    {
                        current[keys[i]] = JsonSerializer.Deserialize<Dictionary<string, object>>(element.GetRawText());
                    }

                    current = current[keys[i]] as Dictionary<string, object>;
                }
            }
        }
    }

    public class TranslationUpdateRequest
    {
        public string Key { get; set; } = "";
        public string ArabicTranslation { get; set; } = "";
        public string EnglishTranslation { get; set; } = "";
    }
}
